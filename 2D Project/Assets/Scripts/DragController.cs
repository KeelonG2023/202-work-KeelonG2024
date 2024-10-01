using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewBehaviourScript : MonoBehaviour
{

    public RandomSpawnManager spawnScript;
    public Camera cam;
    public float distanceUntilSpawn;

    private Vector2 MousePosition;
    private Vector2 prevMouse;

    private float lastSpawnDistance;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        spawnScript = GetComponent<RandomSpawnManager>();
        Debug.Log(spawnScript);
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
            isActive = true;
        else
            isActive = false;
        
        if (prevMouse != null)
        {
            if ( Mathf.Abs((MousePosition - prevMouse).magnitude) > 0.1 && isActive) 
            {
                
                lastSpawnDistance += Mathf.Abs((MousePosition - prevMouse).magnitude);
                

                if (lastSpawnDistance >= distanceUntilSpawn) 
                {
                    GameObject newobj = spawnScript.PickRandomObject();

                    spawnScript.Spawn(newobj, MousePosition);
                    lastSpawnDistance = 0;
                }
            }
        }

        prevMouse = MousePosition;

        if (Input.GetKeyDown(KeyCode.R))
            spawnScript.Despawn();
    }
}
