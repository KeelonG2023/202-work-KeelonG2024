using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public List<Entity> entities;
    public List<GameObject> houses;
    public List<GameObject> humanoids;   
    public List<GameObject> trees;

    public GameObject houseObject;
    public GameObject humanObject;
    public GameObject treeObject;

    public int houseAmount;
    public int treeAmount;

    public float dayTime;
    public float nightTime;

    public bool day = true;
    public float phasetimer;

    public Vector2 bounds;

    public GameObject nightShade;

    public bool paths;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < houseAmount; i++) 
        {
            NewHouse(new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y)), true);
        }

        for (int i = 0; i < treeAmount; i++)
        {
            NewTree(new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y)));
        }

        phasetimer = dayTime;
    }

    // Update is called once per frame
    void Update()
    {
        phasetimer -= Time.deltaTime;


        foreach (Entity e in entities)
        {
            if (e is Humanoid)
            {
                ((Humanoid)e).outsideForces = Vector3.zero;

                foreach (Entity other in entities)
                {
                    Vector3 moveaway = (e.gameObject.transform.position - other.gameObject.transform.position);
                    //separate any required gameObjects;
                    if (moveaway.magnitude < other.effectdistance && other != e && other.gameObject != ((Humanoid)e).target)
                    {
                        if (moveaway != Vector3.zero)
                        {
                            moveaway = moveaway.normalized * 1 / moveaway.magnitude;

                            moveaway = Vector3.ClampMagnitude(moveaway, other.effectdistance);

                            ((Humanoid)e).outsideForces += moveaway * other.attraction; 
                        }
                        
                    }

                    if (other is Tree && ((Humanoid)e).target == null && moveaway.magnitude < ((Humanoid)e).maxRange && other)
                    {
                        if ( ((Tree)other).fruits > 0)
                        {
                            ((Humanoid)e).target = other.gameObject;
                        }
                    }
                }
            }
        }

        if (phasetimer <= 0)
        {
            day = !day;

            if (day)
            {
                phasetimer = dayTime;
                nightShade.SetActive(false);

                foreach (GameObject tree in trees)
                {
                    Tree t = tree.GetComponent<Tree>();

                    if (Random.Range(1, 4) < 4 && t.fruits < 1)
                    {
                        t.fruits = 1;
                    }
                }

                foreach (GameObject house in houses)
                {
                    House h = house.GetComponent<House>();

                    h.WakeUp();
                }

            }
            else
            {
                phasetimer = nightTime;
                nightShade.SetActive(true);

                foreach (GameObject human in humanoids)
                {
                    Humanoid h = human.GetComponent<Humanoid>();
                    h.target = h.home; 
                }
            }
        }
        //kill any humanoids not in houses after midnight
        else if (!day && phasetimer <= nightTime / 2)
        {
            int i = 0;
            while (i < humanoids.Count)
            {
                Humanoid h = humanoids[i].GetComponent<Humanoid>();

                if (humanoids[i].activeSelf)
                {
                    KillEntity(h);
                }
                else
                {
                    i++;
                }
            }

            foreach (GameObject house in houses)
            {
                House h = house.GetComponent<House>();

                if (!h.checkForFood)
                {
                    h.CheckForFood();
                }
            }
        }
    }

    public void KillEntity( Entity entity)
    {
        entities.Remove(entity);

        if (entity is Humanoid)
        {
            humanoids.Remove(((Humanoid)entity).gameObject);
        }
        if (entity is Tree)
        {
            trees.Remove(((Tree)entity).gameObject);
        }
        if (entity is House)
        {
            houses.Remove(((House)entity).gameObject);
        }

        Destroy(entity.gameObject);
    }

    public void NewHumanoid(Vector3 pos)
    {
        GameObject newHuman = Instantiate(humanObject);
        newHuman.GetComponent<Humanoid>().gm = this;
        newHuman.GetComponent<Humanoid>().home = FindNearestAvailibleHouse(pos);
        newHuman.GetComponent<Humanoid>().home.GetComponent<House>().currentSlots++;
        
        newHuman.transform.position = pos;

        entities.Add(newHuman.GetComponent<Humanoid>());
        humanoids.Add(newHuman);
    }

    public void NewHumanoid(GameObject home)
    {
        GameObject newHuman = Instantiate(humanObject);
        newHuman.GetComponent<Humanoid>().gm = this;
        newHuman.GetComponent<Humanoid>().home = home;
        newHuman.GetComponent<Humanoid>().home.GetComponent<House>().currentSlots++;
        
        newHuman.transform.position = home.transform.position;

        entities.Add(newHuman.GetComponent<Humanoid>());
        humanoids.Add(newHuman);
    }

    public void NewTree(Vector3 pos)
    {
        GameObject newTree = Instantiate(treeObject);

        newTree.transform.position = new Vector3(pos.x,pos.y);

        entities.Add(newTree.GetComponent<Tree>());
        trees.Add(newTree);
    }

    public void NewHouse(Vector3 pos, bool addChildren)
    {
        GameObject newHouse = Instantiate(houseObject);

        newHouse.transform.position = pos;

        entities.Add(newHouse.GetComponent<House>());
        newHouse.GetComponent<House>().gm = this;
        houses.Add(newHouse);

        if (addChildren) 
        { 
            for (int i = 0; i < 2; i++)
            {
                NewHumanoid(newHouse);
            }
        
        }
    }

    public GameObject FindNearestAvailibleHouse(Vector3 pos)
    {
        float distancetoBeat = Mathf.Infinity;
        GameObject nearestHouse = houses[0];
        foreach(GameObject h in houses) 
        {
            float mag = (h.transform.position - pos).magnitude;
            House hou = h.GetComponent<House>();
            if (mag < distancetoBeat && hou.currentSlots < hou.maxOccupancy)
            {
                nearestHouse = h;
                distancetoBeat = mag;
            }
        }
        return nearestHouse;
    }

    public void TogglePaths()
    {
        paths = !paths;

        foreach (GameObject h in humanoids)
        {
            LineRenderer hum = h.GetComponent<LineRenderer>();

            hum.enabled = paths;
        }
    }
}

