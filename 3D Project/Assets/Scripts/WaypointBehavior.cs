using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class WaypointBehavior : MonoBehaviour
{

    public GameObject target;

    public GameManager manager;

    public Material material;

    private void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        material.EnableKeyword("_Emission");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {   
        material.SetColor("_EmissionColor", manager.selectedColor);
        manager.UpdateObjects(gameObject);
    }

    public void SetColor(Color col)
    {
        material.SetColor("_EmissionColor", col);
    }
}
