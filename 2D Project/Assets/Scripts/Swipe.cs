using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Swipe : MonoBehaviour
{
    public FruitNinjaGameManagerButtonless gameManager;

    private Vector3 prevPosition;
    private Vector3 currentPosition;


    private bool swipeActive = false;

    //a float determining how long to wait between takes of recording swipe data so it doesnt record too fast 
    public float buffer = 0.1f;

    private float currentBuffer;

    private LineRenderer lineRenderer;
    private RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        currentBuffer = 0;

        lineRenderer = GetComponent<LineRenderer>();
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

        currentPosition = new Vector3(currentPosition.x, currentPosition.y, 0);

        if (swipeActive)
        {        
            if (currentBuffer >= buffer) 
            {
                prevPosition = currentPosition;
                currentBuffer = 0;
            }

            lineRenderer.SetPosition(0, new Vector3(prevPosition.x, prevPosition.y, 0));
            lineRenderer.SetPosition(1, new Vector3(currentPosition.x, currentPosition.y, 0));
            if (hit = Physics2D.Linecast(prevPosition, currentPosition))
            {
                print((currentPosition - prevPosition).magnitude);

                if ((currentPosition - prevPosition).magnitude > 0.4)
                {
                    if (hit.collider.gameObject.GetComponent<FruitScriptButtonless>().isbomb)
                    {
                        gameManager.ToggleGame(false);
                    }
                    else
                    {
                        gameManager.AddToScore(1);
                        gameManager.Remove(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                    }
                }

            }
        }
        
    }
}
