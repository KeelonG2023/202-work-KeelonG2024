using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // this speed is multiplied by deltaTime to get the amount of units it should spin per second
    public float rotSpeed;
    private JointMotor2D motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<HingeJoint2D>().motor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            print("hi");
            print(motor);
            motor.motorSpeed = rotSpeed;

            GetComponent<HingeJoint2D>().motor = motor;
        }
    }
}

