using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aleatoireWaste : MonoBehaviour {

    public GameObject prefab;
    public Vector3 center;
    public Vector3 size;
    public int nbMaxWaste;
    public int nbMinWaste;

    private int nbrWaste;

    // Use this for initialization
    void Awake () {
        nbrWaste = Random.Range(nbMinWaste, nbMaxWaste+1);

        for (int i= 0; i < nbrWaste; i++) aleatoirePrefab();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void aleatoirePrefab()
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x/2, size.x/2), 0 , Random.Range(-size.z / 2, size.z / 2));
        Instantiate(prefab, pos, Quaternion.identity);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
