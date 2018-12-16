using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sousEau : MonoBehaviour {


    public GameObject camera1;
    public GameObject solPiscine;
	private Color couleurNormale;
    private Color couleurSousEau;

    public int nbrDechets; 
    public float densiteMinimum = 0.2f;
    public float densiteMaximum = 0.6f;
    private int nbrDechetsMax;     
    private float densiteMiniIntermediaire; // permet de créer des paliers
    private float densiteBrouillard = 0.3f;

    private bool cycleEnCours;

    public Text pourcentageProprete;

    void Start () {

        nbrDechetsMax = solPiscine.GetComponent<aleatoireWaste>().nbMaxWaste;

        couleurNormale = new Color (0.5f, 0.5f, 0.5f, 0.5f);
		couleurSousEau = new Color (0.22f, 0.65f, 0.77f, 0.5f); 

        setDensiteMin();
        if (densiteMiniIntermediaire >= densiteMaximum)
        {
            densiteBrouillard = densiteMaximum;
        }
        else
        {
            densiteBrouillard = densiteMiniIntermediaire + ((densiteMaximum - densiteMinimum) * 0.2f);
        }
        pourcentageProprete.text = calculPoursentage();
    }   

    public string calculPoursentage()
    {
       
        float pourcentageDetail = ((densiteMaximum - densiteBrouillard) * 100) / (densiteMaximum-densiteMinimum);
        int pourcentage = (int)pourcentageDetail;
        return "Propreté : " + pourcentage + "%" ;
    }

    void Update () {

        //Change l'intensité du brouillard suivant la saleté de la piscine
        setDensiteMin();

        cycleEnCours = gameObject.GetComponent<robotDeplacement>().cycleEnCours;
        if (densiteBrouillard > densiteMiniIntermediaire && cycleEnCours) 
        {
            densiteBrouillard -= 0.0001f;
        }
        pourcentageProprete.text = calculPoursentage();

        //Affiche ou non la brouillard suivant si le robot est sous l'eau ou non
        //Le robot et toujours dans l'eau donc on regarde si la camera sous l'eau est activé
        if (camera1.activeSelf)
        {
            setSousEau();
        }
        else
        {
            setNormal();
        }

    }

    public void setNormal () {
		RenderSettings.fogColor = couleurNormale;
		RenderSettings.fogDensity = 0.02f;

	}

    public void setSousEau () {
		RenderSettings.fogColor = couleurSousEau;
		RenderSettings.fogDensity = densiteBrouillard; // 0.3f
    }

    public void setDensiteMin()
    {
        nbrDechets = GameObject.FindGameObjectsWithTag("waste").Length;

        if (nbrDechets >= nbrDechetsMax) densiteMiniIntermediaire = densiteMaximum;
        else if (nbrDechets >= nbrDechetsMax * 0.80) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.2f);
        else if (nbrDechets >= nbrDechetsMax * 0.60) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.4f);
        else if (nbrDechets >= nbrDechetsMax * 0.40) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.6f);
        else if (nbrDechets >= nbrDechetsMax * 0.20) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.8f);
        else if (nbrDechets >= nbrDechetsMax * 0.10) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.9f);
        else if (nbrDechets >= nbrDechetsMax * 0.05) densiteMiniIntermediaire = densiteMaximum - ((densiteMaximum - densiteMinimum) * 0.95f);
        else if (nbrDechets == 0) densiteMiniIntermediaire = densiteMinimum;
    }
}
