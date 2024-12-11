using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float camSpeed;
    public float maxSpeed;
    public Vector3 camMovement;

    public Game manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");

        if(movX != 0 || movY != 0)
        {
            camMovement.x += movX * camSpeed * Time.deltaTime;
            camMovement.y += movY * camSpeed * Time.deltaTime;
        }

        if(movX == 0 && camMovement.x != 0)
        {
            if (camMovement.x > 0)
                camMovement.x -= camSpeed * Time.deltaTime;
            if (camMovement.x < 0)
                camMovement.x += camSpeed * Time.deltaTime;
        }

        if(movY == 0 && camMovement.y != 0)
        {
            if (camMovement.y > 0)
                camMovement.y -= camSpeed * Time.deltaTime;
            if (camMovement.y < 0)
                camMovement.y += camSpeed * Time.deltaTime;
        }

        camMovement = Vector3.ClampMagnitude(camMovement, maxSpeed);

        transform.position += camMovement;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -manager.bounds.x, manager.bounds.x),
            Mathf.Clamp(transform.position.y, -manager.bounds.y, manager.bounds.y),
            transform.position.z);

        Vector3 mousePos = gameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            manager.NewTree(mousePos);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            manager.NewHouse(mousePos, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            manager.NewHumanoid(mousePos);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            manager.TogglePaths();
        }

    }
}
