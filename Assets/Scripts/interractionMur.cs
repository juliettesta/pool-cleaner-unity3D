using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interractionMur : MonoBehaviour {
    public GameObject robot;

    /// <summary>
    /// Si le nez cogne un mur, il passe la variable tourneCarMur du robot à vrai pour le faire tourner et reculer
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "mur")
            if (!robot.GetComponent<robotDeplacement>().TourneCarMur)
            {
                robot.GetComponent<robotDeplacement>().TourneCarMur = true;
               
                robot.GetComponent<robotDeplacement>().ChronoDeplacementRotation = Time.time;
            }
    }

}
