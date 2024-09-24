using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float time;

    //used to specify an object to filter triggers to. if an interest is specified the object will NOT change
    //the color of other objects that are not the interest
    public GameObject interest;

    private float timer;
    private bool timerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color (GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, (255 * (timer / time))/255);
            if (timer <= 0 ) 
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && timerStarted == false)
        {
            //if an interest is specified check to see
            if (interest != null && collision.gameObject != interest)
            {

                return;
            }

            timer = time;
            timerStarted = true;
        }
    }
}
