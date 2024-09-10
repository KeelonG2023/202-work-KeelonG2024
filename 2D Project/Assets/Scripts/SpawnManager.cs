using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject tospawn;
    public GameObject followCam;
    private GameObject spawned = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned == null) 
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Follow followScript = followCam.GetComponent<Follow>();
        spawned = Instantiate(tospawn, this.transform);

        followScript.target = spawned;
    }
}


