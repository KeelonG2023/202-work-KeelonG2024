using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrive : MonoBehaviour
{
    // Reference to RigidBody on this GameObject
    public Rigidbody rBody;

    // Fields for Speed
    public float maxSpeedForwards, maxSpeedBackwards, maxSpeedSideways, minSpeed;

    // Fields for Acceleration/Deceleration
    public float accelerationRate, decelerationRate;

    // Fields for Turning
    public float turnSpeed;

    // Fields for Input
    public Vector3 movementDirection;

    // Fields for Movement Vectors
    public Vector3 velocity, acceleration;

    // Fields for Quaternions
    public Quaternion turning;


    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Forward = transform.forward;
        
        movementDirection.z = Input.GetAxisRaw("Vertical");

        // Reset acceleration
        acceleration = Vector3.zero;

        // Use Input to calc current acceleration for this frame
        acceleration = Forward * (movementDirection.z * accelerationRate);

        // Add Acceleration to Velocity
        velocity += acceleration * Time.deltaTime;


        if (movementDirection.z != 0)
        {
            movementDirection.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime;

            //Clamp rotation
            if (movementDirection.x > maxSpeedSideways)
            {
                movementDirection.x = maxSpeedSideways;
            }
                
            else if (movementDirection.x < maxSpeedSideways*-1)
            {
                movementDirection.x = maxSpeedSideways * -1;
            }

            // Clamp Velocity
            if (Input.GetAxisRaw("Vertical") == 1 && velocity.magnitude > maxSpeedForwards)
            {
                velocity.Normalize();
                velocity *= maxSpeedForwards;
            }
            if (Input.GetAxisRaw("Vertical") == -1 && velocity.magnitude > maxSpeedBackwards)
            {
                velocity.Normalize();
                velocity *= maxSpeedBackwards;
            }
        }
        //  Slow down while no input
        else if (velocity.magnitude != 0f)
        {
            //	Remove a percentage of the Velocity based on Time
            velocity *= 1f - (decelerationRate * Time.deltaTime);
            
            //  Stop the vehicle when it reaches a certain speed
            if (velocity.magnitude < minSpeed)
            {
                velocity = Vector3.zero;
            }
            
        }

        if (Input.GetAxisRaw("Horizontal") == 0 || Input.GetAxisRaw("Vertical") == 0)
        {
            movementDirection.x *= 1f - (decelerationRate * Time.deltaTime);

            if (Mathf.Abs(movementDirection.x) < minSpeed)
            {
                movementDirection.x = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        //  Calc current turning
        turning = Quaternion.Euler(0f,
        movementDirection.x * turnSpeed * Time.fixedDeltaTime, 0f);

        //  Calc current Turning
        Quaternion nextRotation = transform.rotation * turning;

        //  Turn the Vehicle's Velocity
        velocity = turning * velocity;

        //  Use velocity to calc next position
        Vector3 nextPosition = transform.position +
        (velocity * Time.fixedDeltaTime);


        //  Move and Rotate the Vehicle
        rBody.Move(nextPosition, nextRotation);


    }

}
