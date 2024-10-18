using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitNinjaGameManager : MonoBehaviour
{
    public bool gameActive;
    public List<GameObject> currentObjects;

    //a list of fruit types and their weights
    public List<GameObject> FruitTypes;
    public List<int> FruitWeight;

    //how long to wait between each fruit spawn, on average
    public float fruitTendancy = 5;
    //how far apart the minimum and maximum time can be between each fruit spawn
    public float fruitSpawnNoise = 0.5f;

    public float localGravity = 1;

    private float timeUntilNextFruit;

    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (fruitTendancy - fruitSpawnNoise)), fruitTendancy + fruitSpawnNoise);

        text = GameObject.Find("ButtonText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            timeUntilNextFruit -= Time.deltaTime;

            if (timeUntilNextFruit <= 0)
            {

                 GameObject Fruit = Instantiate(PickFruit(), new Vector3(UnityEngine.Random.Range(-8f, 8f), -6, 0), new Quaternion());

                currentObjects.Add(Fruit);

                Fruit.GetComponent<FruitScript>().StartFruit(GetComponent<FruitNinjaGameManager>(), UnityEngine.Random.Range(Fruit.transform.position.x * -0.75f, Fruit.transform.position.x * -1.25f), UnityEngine.Random.Range(7, 10));

                timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (fruitTendancy - fruitSpawnNoise)), fruitTendancy + fruitSpawnNoise);
            }

        }
    }
  

    private GameObject PickFruit()
    { 
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject obj in FruitTypes) 
        { 
            for (int i = 0; i < FruitWeight[FruitTypes.IndexOf(obj)]; i++)
            {
                list.Add(obj);
            }
            
        }

        return list[list.Count - 1];

    }

    public void ToggleGame()
    {
        gameActive = !gameActive;

        text.text = "Toggle Game State (" + gameActive.ToString() + ")";

        if (gameActive == false) 
        { 
            foreach(GameObject obj in currentObjects)
            {
                Destroy(obj);
            }
 
        }
        else
        {
            timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (fruitTendancy - fruitSpawnNoise)), fruitTendancy + fruitSpawnNoise);
        }
    }
}
