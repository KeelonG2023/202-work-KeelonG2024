using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTurn : MonoBehaviour
{
    public float turnIntensity;

    private float turnDir;
    private bool turned = false;


    // Start is called before the first frame update
    void Start()
    {
        turnDir = (Random.Range(0, 2) * 2 - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && turned == false)
        {
            this.transform.Rotate(0, 0, turnIntensity * turnDir);

            turned = true;

        }
    }
}
