using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class House : Entity
{
    private TMP_Text text;
    public int storedFruits;
    public List<GameObject> occupants;
    public bool checkForFood = false;
    public int maxOccupancy = 4;
    public Game gm;

    public int foodPerHumanoid = 1;
    public int childCost = 2;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject;
        attraction = 0;
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        text.text = $"Occupants: {occupants.Count}/{maxOccupancy} \n Stored Fruit: {storedFruits}";
    }

    public void EnterHome(GameObject occupant)
    {
        occupants.Add(occupant);
        occupant.SetActive(false);
        UpdateText();
    }


    public void CheckForFood()
    {
        if (occupants.Count > 0)
        {
            int foodReqCount = occupants.Count * foodPerHumanoid;

            while (foodReqCount > storedFruits && occupants.Count > 0)
            {
                gm.KillEntity(occupants[0].GetComponent<Humanoid>());
                occupants.RemoveAt(0);
            }
            if (occupants.Count > 0)
            {
                storedFruits -= foodReqCount;

                if (storedFruits >= childCost && occupants.Count < maxOccupancy)
                {
                    for (int i = 0; i < Mathf.Floor(foodReqCount / storedFruits); ++i)
                    {
                        if (occupants.Count < maxOccupancy)
                        {
                            GameObject child = Instantiate(gm.humanObject);

                            child.SetActive(false);
                            child.transform.position = transform.position;
                            occupants.Add(child);

                            Humanoid ch = child.GetComponent<Humanoid>();

                            ch.gm = gm;
                            ch.home = this.gameObject;

                            gm.entities.Add(ch);
                            gm.humanoids.Add(child);

                            storedFruits -= childCost;
                        }
                    }


                }

            }



            checkForFood = true;

            UpdateText();
        }
     
    }

    public void WakeUp()
    {
        checkForFood = false;
        while(occupants.Count > 0)
        {
            occupants[0].SetActive(true);
            occupants.RemoveAt(0);
        }
        UpdateText();
    }
}
