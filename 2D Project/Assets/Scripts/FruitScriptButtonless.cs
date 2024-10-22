using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitScriptButtonless : MonoBehaviour
{
    public bool isbomb;

    public float xvel;
    public float yvel;

    public int rot;

    public bool active = false;

    private FruitNinjaGameManagerButtonless gameManager;

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

            this.transform.Rotate(new Vector3(0, 0, rot * Time.deltaTime));

            if (this.transform.position.y < -15)
            {
                Destroy(this.gameObject);
                gameManager.Remove(gameObject);
                if (!isbomb)
                {
                    gameManager.AddToLives(-1);
                }
                
            }
        
        }
    }

    public void StartFruit(FruitNinjaGameManagerButtonless manager, float startxvel, float startyvel)
    {
        gameManager = manager;
        xvel = startxvel;
        yvel = startyvel;
        rot = UnityEngine.Random.Range(-15, 15);
        active = true;
    }
}


