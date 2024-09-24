using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnTouch : MonoBehaviour
{
    //used to specify an object to filter triggers to. if an interest is specified the object will NOT change
    //the color of other objects that are not the interest
    public GameObject interest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        { 
            //if an interest is specified check to see
            if (interest != null && collision.gameObject != interest)
            {

                return;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;


        }
    }
}
