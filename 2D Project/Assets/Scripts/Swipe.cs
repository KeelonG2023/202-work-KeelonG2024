using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector3 prevPosition;
    private Vector3 currentPosition;


    private bool swipeActive = false;

    //a float determining how long to wait between takes of recording swipe data so it doesnt record too fast 
    public float buffer = 0.1f;

    private float currentBuffer;
    // Start is called before the first frame update
    void Start()
    {
        currentBuffer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            swipeActive = true;
        }
        else
        {
            swipeActive = false;
        }

        currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentBuffer += Time.deltaTime;

        if (swipeActive)
        {

            if (currentBuffer >= buffer) 
            {
                prevPosition = currentPosition;
                currentBuffer = 0;
            }
            
            Debug.DrawLine(new Vector3(prevPosition.x, prevPosition.y, 0), new Vector3(currentPosition.x, currentPosition.y, 0));
           
        }
        
    }
}
