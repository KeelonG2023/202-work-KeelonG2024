using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitNinjaGameManagerButtonless : MonoBehaviour
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

    public float localGravity = 5;

    //what are the chances of an "ambush" wave? basically multiple fruits spawn at the same time
    public float ambushchance = 0.25f;

    public int maxAmbushFruit = 3;

    private float timeUntilNextFruit;

    private float localFruitTendancy;

    private int score;
    private int lives;

    private TMP_Text scoretext;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (fruitTendancy - fruitSpawnNoise)), fruitTendancy + fruitSpawnNoise);

        scoretext = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        button = GameObject.Find("StartButton").GetComponent<Button>();

        localFruitTendancy = fruitTendancy;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            timeUntilNextFruit -= Time.deltaTime;

            if (timeUntilNextFruit <= 0)
            {
                float num = UnityEngine.Random.Range(0f, 1f);

                float fruitAmount = 1;


                if (num < ambushchance)
                {
                    while (fruitAmount < maxAmbushFruit)
                    {
                        fruitAmount++;

                        num = UnityEngine.Random.Range(0f, 1f);

                        if (num > ambushchance * 2)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < fruitAmount; i++)
                {
                    GameObject Fruit = Instantiate(PickFruit(), new Vector3(UnityEngine.Random.Range(-8f, 8f), -6, 0), new Quaternion());

                    currentObjects.Add(Fruit);

                    Fruit.GetComponent<FruitScriptButtonless>().StartFruit(GetComponent<FruitNinjaGameManagerButtonless>(), UnityEngine.Random.Range(Fruit.transform.position.x * -0.75f, Fruit.transform.position.x * -1.25f), UnityEngine.Random.Range(7, 10));
                }

                

                timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (localFruitTendancy - fruitSpawnNoise)), localFruitTendancy + fruitSpawnNoise);
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

        return list[UnityEngine.Random.Range(0, list.Count - 1)];

    }

    public void ToggleGame()
    {
        gameActive = !gameActive;

        if (gameActive == false) 
        { 
            foreach(GameObject obj in currentObjects)
            {
                Destroy(obj);
            }
            button.transform.localScale = new Vector3(1, 1, 1);
            localFruitTendancy = fruitTendancy;
        }
        else
        {
            button.transform.localScale = new Vector3(0, 1, 1);
            lives = 3;
            score = 0;
            scoretext.text = "Score: " + score + "\n Lives: " + lives;
            timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (localFruitTendancy - fruitSpawnNoise)), localFruitTendancy + fruitSpawnNoise);
        }
    }
    public void ToggleGame(bool type)
    {
        gameActive = type;

        if (gameActive == false)
        {
            foreach (GameObject obj in currentObjects)
            {
                Destroy(obj);
            }
            button.transform.localScale = new Vector3(1, 1, 1);
            localFruitTendancy = fruitTendancy;
        }
        else
        {
            lives = 3;
            score = 0;
            button.transform.localScale = new Vector3(0, 1, 1);
            scoretext.text = "Score: " + score + "\n Lives: " + lives;
            timeUntilNextFruit = UnityEngine.Random.Range(Math.Max(0, (localFruitTendancy - fruitSpawnNoise)), localFruitTendancy + fruitSpawnNoise);
        }
    }
    public void Remove(GameObject obj)
    {
        if (currentObjects.Contains(obj))
        { currentObjects.Remove(obj); }
    }

    public void AddToScore(int amount)
    {
        score += amount;
        scoretext.text = "Score: " + score + "\n Lives: " + lives;

        localFruitTendancy = fruitTendancy - ((fruitTendancy * 0.9f) * score / 100);
    }

    public void AddToLives(int amount)
    {
        lives += amount;
        scoretext.text = scoretext.text = "Score: " + score + "\n Lives: " + lives;

        if (lives <= 0)
        {
            ToggleGame(false);
        }
    }
}
