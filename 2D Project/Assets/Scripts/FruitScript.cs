using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitScript : MonoBehaviour
{

    public float xvel;
    public float yvel;

    public bool active = false;

    private FruitNinjaGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        { 

            Vector3 pos = this.transform.position;

            this.transform.position = new Vector3(pos.x + xvel * (Time.deltaTime), pos.y + (yvel * Time.deltaTime), 0);

            yvel -= gameManager.localGravity * Time.deltaTime;

            if (this.transform.position.y < -15)
            {
                Destroy(this.gameObject);
            }
        
        }
    }

    public void StartFruit(FruitNinjaGameManager manager, float startxvel, float startyvel)
    {
        gameManager = manager;
        xvel = startxvel;
        yvel = startyvel;

        active = true;
    }
}


