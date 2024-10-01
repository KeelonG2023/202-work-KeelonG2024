using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnManager : MonoBehaviour
{
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;
    public GameObject spawn5;

    public float objectWeight, colorWeight;


    private List<GameObject> spawns;
    private List<Color> colorList;

    private List<GameObject> spawned;

    public GameObject weightedObjectType;
    public Color weightedColor;

    public float minX, minY, maxX, maxY;


    // Start is called before the first frame update
    void Start()
    {
        objectWeight = 0.75f;
        colorWeight = 0.5f;

        minX = 0.5f; minY = 0.5f;
        maxX = 1f; maxY = 1f;

        spawns = new List<GameObject>() { spawn1, spawn2, spawn3, spawn4, spawn5 };
        colorList = new List<Color>() { Color.red, Color.yellow, Color.blue, Color.white, Color.green, Color.cyan };

        weightedObjectType = spawns[Random.Range(0, spawns.Count - 1)];
        weightedColor = colorList[Random.Range(0, colorList.Count - 1)];

        spawned = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject PickRandomObject()
    {
        //Weight determines the likelihood of the next object being picked being the same as the last
        if (Random.value < objectWeight)
        {
            return weightedObjectType.gameObject;
        }
        else
        {
            GameObject result = spawns[Random.Range(0, spawns.Count - 1)];
            weightedObjectType = result.gameObject;
            return result;
        }
        
    }
    public Color PickRandomColor()
    {
        //Weight determines the likelihood of the next object being picked being the same as the last (ditto Object)
        if (Random.value < colorWeight)
        {
            return weightedColor;
        }
        else
        {
            Color color = colorList[Random.Range(0, colorList.Count - 1)];
            weightedColor = color;
            return color;
        }
    }
    public Quaternion PickRandomRotation()
    {
        return Random.rotation;
    }

    public Vector2 PickRandomScale()
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void Spawn(GameObject obj, Vector2 pos) 
    {
        GameObject newObj = Instantiate(obj);

        newObj.transform.position = pos;
        newObj.transform.rotation = PickRandomRotation();
        newObj.transform.localScale = PickRandomScale();

        newObj.GetComponent<SpriteRenderer>().color = PickRandomColor();

        spawned.Add(newObj);
    }


    public void Despawn()
    { 
        do
        {
            GameObject dest = spawned[0];
            spawned.RemoveAt(0);
            Destroy(dest);
        }
        while (spawned.Count > 0);
 
    }
}
