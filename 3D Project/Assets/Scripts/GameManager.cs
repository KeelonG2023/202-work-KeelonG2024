using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject waypoint;
    
    public List<GameObject> waypoints;
    private List<GameObject> unsortedWaypoints;
    
    public int waypointCount;

    public Vector3 startingPos;

    public Terrain playingField;

    private int bounds;

    public GameObject player;

    public Color currentSelectionColor;
    public Color selectedColor;
    public Color idlingColor;

    public GameObject selectedWaypoint;
    public GameObject arrow;

    private Vector3 startPos;

    public bool gameStarted;

    private float timer;

    private float timerStartCount = 60;

    private TMP_Text timertext;
    private GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        startPos = player.transform.position;

        timertext = GameObject.Find("Timer").GetComponent<TMP_Text>();
        startButton = GameObject.Find("StartButton");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && timer > 0) 
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);

            timertext.text = timer.ToString();

            if (timer == 0)
            {
                EndGame(false);
            }
        }

        
    }

    public GameObject FindNearest(List<GameObject> waypoints, Vector3 pos)
    {
        GameObject nearest = waypoints[0];
        float nearestSoFar = Mathf.Infinity;

        foreach (GameObject waypoint in waypoints) 
        { 
            float distance = (waypoint.transform.position - pos).magnitude;

            if (distance < nearestSoFar)
            {
                nearestSoFar = distance;
                nearest = waypoint;
            }
            
        }

        return nearest;
    }

    public void StartGame()
    {
        timer = timerStartCount;
        gameStarted = true;
        player.transform.position = startingPos;
        waypoints = new List<GameObject>();
        unsortedWaypoints = new List<GameObject>();

        bounds = ((int)playingField.GetComponent<Terrain>().terrainData.size.x / 2) - 20;

        for (int i = 0; i < waypointCount; i++)
        {
            GameObject newpoint = Instantiate(waypoint);
            newpoint.transform.position = new Vector3(Random.Range(bounds * -1, bounds), 0, Random.Range(bounds * -1, bounds));
            unsortedWaypoints.Add(newpoint);

        }

        GameObject nearest = FindNearest(unsortedWaypoints, player.transform.position);

        unsortedWaypoints.Remove(nearest);

        waypoints.Add(nearest);
        //sort the waypoints by nearest to the last waypoint
        for (int i = 0; i < waypointCount - 1; i++)
        {
            nearest = FindNearest(unsortedWaypoints, nearest.transform.position);

            unsortedWaypoints.Remove(nearest);

            waypoints.Add(nearest);
        }


        UpdateColors();
        arrow.GetComponent<Arrow>().setTarget(selectedWaypoint);
    }

    public void EndGame(bool win) 
    {
        gameStarted = false;

        while(waypoints.Count > 0)
        {
            Destroy(waypoints[0]);
            waypoints.RemoveAt(0);
        }
            

        if (win)
        {
            timertext.text = "You Win!";
        }
        else 
        {
            timertext.text = "You Lose..";
        
        }

        startButton.SetActive(true);
    }


    public void UpdateObjects(GameObject waypointToRemove) 
    {
        if (waypointToRemove == waypoints[0])
        {
            timer += 5;
            waypoints.RemoveAt(0);
            Destroy(waypointToRemove);

            if (waypoints.Count == 0 && timer > 0)
            {
                EndGame(true);
            }
            else
            {
               selectedWaypoint = waypoints[0];
            }
           
        }

        UpdateColors();

        arrow.GetComponent<Arrow>().setTarget(selectedWaypoint);

    }

    public void UpdateColors()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            WaypointBehavior wPoint;
            wPoint = waypoints[i].GetComponent<WaypointBehavior>();
            if (i == 0)
            {
                
                wPoint.SetColor(currentSelectionColor);
                selectedWaypoint = waypoints[i];
            }
            else
            {
                wPoint.SetColor(idlingColor);
            }
        }
        
    }
}
