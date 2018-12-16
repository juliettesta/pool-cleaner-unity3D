using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class robotDeplacement : MonoBehaviour
{
    public Button on;
    public Button off;
    Rigidbody rb;
    public bool cycleEnCours = true;
    public int vitesseDeplacement = 2; //change entre 2 et 6 sec
    public float tempsDeplacement = 2.0f;
    private float tempsDeplacementSave; //Le temps de déplacement change autour de la valeur initiale
    public float tempsRotation = 1f;
    private float tempsRotationSave; //Le temps de rotation change autour de la valeur initiale
    public float tempsCycle = 60.0f;
    private float chronoDeplacementRotation; //chrono recommencé après chaque rotation
    private float chronoCycle; //chrono recommencé après chaque appuie sur bouton on
    private bool tourneCarMur = false; //permet de savoir si on tourne à cause du mur (du coup on recule et on termine prématurément la phase de déplacement)
    private bool debutVirage = true; //permet de savoir si on commence le chrono de rotation
    private bool droite; //direction du virage

    public Text affichageChrono;

    public GameObject roueAvant;
    public GameObject roueArriere;

    void Start()
    {
        droite = true;
        rb = GetComponent<Rigidbody>();
        reinitialiseChrono();
        affichageChrono.text = "";
        tempsDeplacementSave = tempsDeplacement;
        tempsRotationSave = tempsRotation;
    }

    //Getter, setter
    public bool TourneCarMur
    {
        get
        {
            return this.tourneCarMur;
        }
        set
        {
            this.tourneCarMur = value;
        }
    }
    public float ChronoDeplacementRotation
    {
        set
        {
            this.chronoDeplacementRotation = value;
        }
    }


    /// <summary>
    /// met le cycle et le robot en marche si on appuie sur le bouton on
    /// </summary>
    /// <param name="marche"></param>
    public void marche(bool marche=true)
    {
        if(!marche)
        {
            cycleEnCours = false;
            affichageChrono.text = "";
            return;
        }
        cycleEnCours = true;
        reinitialiseChrono();
    }

    /// <summary>
    /// Réinitialise le chrono du cycle si on appuie sur le bouton off
    /// </summary>
    public void reinitialiseChrono()
    {
        chronoDeplacementRotation = Time.time;
        chronoCycle = Time.time;
    }

    /// <summary>
    /// Affiche le temps restant du cycle sur l'UI
    /// </summary>
    /// <param name="temps"></param>
    /// <returns></returns>
    public string affichageCycle(float temps) {

        int minutes = (int)temps / 60;
        float secondes = (int)temps % 60;
        return minutes + ":" + secondes;
    }

    void Update()
    {
        //Afficher le temps restant du cycle
        if (cycleEnCours)
            affichageChrono.text = affichageCycle(tempsCycle - (Time.time - chronoCycle));
    }

    /// <summary>
    /// Mouvements du robot
    /// </summary>
    void FixedUpdate()
    {
        if (!cycleEnCours)
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
            return;
        }
        if (chronoDeplacementRotation + tempsDeplacement > Time.time && !tourneCarMur)
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * vitesseDeplacement);
            roueArriere.transform.Rotate(Vector3.up * Time.deltaTime * vitesseDeplacement * 40);
            roueAvant.transform.Rotate(Vector3.up * Time.deltaTime * vitesseDeplacement * 40);
        }
        else if (chronoDeplacementRotation + tempsDeplacement + tempsRotation > Time.time && !tourneCarMur)
            Tourne(false);
        // Si on tourne à cause du mur, le temps de déplacement n'est pas terminé, le chrono reprend à 0 et il ne reste qu'à écouler le temps de rotation
        else if (chronoDeplacementRotation + tempsRotationSave > Time.time && tourneCarMur)
            Tourne(true);
        else
        {
            chronoDeplacementRotation = Time.time;
            tourneCarMur = false;
            debutVirage = true;
            tempsDeplacement = Random.value * tempsDeplacementSave;
            tempsRotation = (Random.value) * tempsRotationSave;
            if (tempsRotation < 0.3f)
                tempsRotation = 0.3f;
            if (tempsDeplacement < 0.3f)
                tempsDeplacement = 0.3f;
        }

        if (chronoCycle + tempsCycle < Time.time)
        {
            affichageChrono.text = "";
            cycleEnCours = false;
        }
    }

    /// <summary>
    /// Le robot tourne d'un cran (fonction appelée à chaque frame)
    /// </summary>
    /// <param name="obstacle">Le bool obstacle nous dit si le robot tourne à cause d'un obstacle</param>
    private void Tourne(bool obstacle)
    {
        Vector3 m_EulerAngleVelocity;
        if (debutVirage)
        {
            droite = (Random.value < 0.5f) ? false : true;
            debutVirage = false;
        }
        //S'il y a un obstacle le robot recule
        if (obstacle)
        {
            rb.MovePosition(transform.position - transform.forward * Time.deltaTime * vitesseDeplacement / 4);
            roueArriere.transform.Rotate(-Vector3.up * Time.deltaTime * vitesseDeplacement * 20);
            roueAvant.transform.Rotate(-Vector3.up * Time.deltaTime * vitesseDeplacement * 20);
        }
        if (droite)
            m_EulerAngleVelocity = new Vector3(0, 120, 0);
        else
            m_EulerAngleVelocity = new Vector3(0, -120, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    /// <summary>
    /// Aspire les déchets
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay(Collider collision)
    {
        if (!cycleEnCours)
            return;
        if (collision.gameObject.tag == "waste")
            collision.transform.position = Vector3.Lerp(collision.transform.position, transform.position, Time.deltaTime * 10);
    }

    /// <summary>
    /// Détruit les déchets
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "waste")
            Destroy(collision.gameObject);
    }

}
