using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AutoAgentArrive : Agent
{
    public Vector3 steeringForce = Vector3.zero;
    public Vector3 acceleration;
    public Vector3 velocity;

    public Quaternion turning;

    public Rigidbody rBody;

    public GameObject target;

    public float maxSpeed = 5;

    public float seekScalar = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate a new Steering force vector
        steeringForce = CalcSteering();

        velocity = steeringForce;

        // Use Velocity the same as in Vehicle

    }
    private void FixedUpdate()
    {
        //  Use velocity to calc next position
        Vector3 nextPosition = transform.position + (velocity * Time.fixedDeltaTime);

        //
        //  Calc current turning to look in the direction of Velocity
        //
        Quaternion nextRotation = Quaternion.LookRotation(velocity, Vector3.up);

        //  Move and Rotate the Vehicle
        rBody.Move(nextPosition, nextRotation);

        velocity = Vector3.zero;
    }
    public override Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        // Return seek steering force
        return seekingForce * Time.deltaTime;
    }
    public Vector3 Seek(GameObject target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Seek(target.transform.position);
    }

    public override Vector3 CalcSteering()
    {
        // Return the steering force for seeking every frame
        // This Seek force could be conditionally applied
        // For instance – only seek when a mouse button/key is
        //   held/pressed.
        return Arrive(target, 10) * seekScalar;
    }
    
    public override Vector3 Arrive(Vector3 target, float radius)
    {

        Vector3 distance = target - transform.position;

        float dMag = distance.magnitude;

        if (dMag < radius)
        {
            distance = distance.normalized * Mathf.Lerp(0, maxSpeed, (dMag/radius));
            
        }
        else
        {
            distance = distance.normalized * maxSpeed;
        }



        Vector3 steering = distance - (transform.position + (velocity * Time.deltaTime));
        steering = Vector3.Min(steering, (steering.normalized * maxSpeed));
   
        return distance;
    }

    public Vector3 Arrive(GameObject target, float radius)
    {
        return Arrive(target.transform.position, radius);
    }


}
