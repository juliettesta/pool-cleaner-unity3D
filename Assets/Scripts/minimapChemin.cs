using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class minimapChemin : MonoBehaviour {
    public GameObject robot;
    GameObject robot2d;
    int largeurMiniMap = 97;
    int longueurMiniMap = 152;
    int largeurPiscine = 10;
    int longueurPiscine = 15;
    private int comptFrames = 0;

    private void Start()
    {
        robot2d = new GameObject("robot2d"); //Create the GameObject
        Image NewImage = robot2d.AddComponent<Image>(); //Add the Image Component script
        robot2d.GetComponent<RectTransform>().SetParent(this.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        robot2d.SetActive(true); //Activate the GameObject
        robot2d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 5);
        robot2d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 5);
        robot2d.transform.localPosition = new Vector3(-longueurMiniMap / longueurPiscine * robot.transform.position.z, largeurMiniMap / largeurPiscine * robot.transform.position.x);
        robot2d.transform.rotation = Quaternion.Euler(0, 0, -robot.transform.rotation.eulerAngles.y);
    }

    private void Update()
    {
        if (!robot.GetComponent<robotDeplacement>().cycleEnCours)
            return;
        robot2d.transform.localPosition = new Vector3(-longueurMiniMap / longueurPiscine * robot.transform.position.z, largeurMiniMap / largeurPiscine * robot.transform.position.x);
        robot2d.transform.rotation = Quaternion.Euler(0, 0, -robot.transform.rotation.eulerAngles.y);
        comptFrames++;
        //On ne laisse pas de marque à chaque fois car ça ne se voit pas visuellement et ça prend un peu de mémoire vive.
        if (comptFrames == 3) 
        {
            laisseMarque();
            comptFrames = 0;
        }
        robot2d.transform.SetAsLastSibling();
    }

    /// <summary>
    /// laisse une image blanche au passage du robot
    /// </summary>
    private void laisseMarque()
    {
        GameObject marque = new GameObject("imgMarque"); //Create the GameObject
        Image NewImage = marque.AddComponent<Image>(); //Add the Image Component script
        NewImage.color = new Color32(236,87,128,255);
        marque.GetComponent<RectTransform>().SetParent(this.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        marque.SetActive(true); //Activate the GameObject
        marque.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 9);
        marque.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 9);
        marque.transform.localPosition = new Vector3(-longueurMiniMap / longueurPiscine * robot.transform.position.z, largeurMiniMap / largeurPiscine * robot.transform.position.x);
        marque.transform.rotation = Quaternion.Euler(0, 0, -robot.transform.rotation.eulerAngles.y);
    }
}
