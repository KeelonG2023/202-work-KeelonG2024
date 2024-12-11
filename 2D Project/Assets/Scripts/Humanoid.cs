using UnityEngine;

public class Humanoid : Entity
{
    public Vector3 steeringForce = Vector3.zero;
    public Vector3 outsideForces;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Quaternion turning;
    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public Rigidbody2D rBody;

    public GameObject target;
    public Vector3 randomWander;

    public float maxSpeed;
    public float maxRange;
    public float seekScalar = 1.0f;
    public int fruits;

    public Game gm;   
    public GameObject home;
    

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        attraction = 3;

        parent = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void FixedUpdate()
    {
        // Calculate a new Steering force vector
        steeringForce = CalcSteering();

        // Add Steering force to Acceleration
        acceleration += steeringForce;

        // Limit how Acceleration
        velocity += acceleration * Time.deltaTime;

        velocity += acceleration * Time.deltaTime;


        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //  Use velocity to calc next position
        nextPosition = transform.position + (velocity * Time.fixedDeltaTime);

        //
        //  Calc current turning to look in the direction of Velocity
        //
        nextRotation = Quaternion.LookRotation(velocity, Vector3.up);

        nextRotation = new Quaternion(0, 0, nextRotation.z, nextRotation.w);

        if ((nextPosition - transform.position).x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if ((nextPosition - transform.position).x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        //  Move and Rotate the Vehicle
        rBody.MovePosition(nextPosition);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * nextRotation, 1f);
        rBody.MoveRotation(nextRotation);
        GetComponent<LineRenderer>().SetPosition(0, transform.position); 

        if (target)
        {
            GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(1, randomWander);
        }
        

    }


    public Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity + outsideForces - velocity;

        // Return seek steering force
        return seekingForce;

    }

    public Vector3 Seek(GameObject target)
    {
        // Call the other version of Seek 
        //   which returns the seeking steering force
        //  and then return that returned vector. 
        return Seek(target.transform.position);
    }

    public Vector3 CalcSteering()
    {
        if (target)
        {
            if ((transform.position - target.transform.position).magnitude < 1)
            {
                //if arrived at a tree
                if (target.GetComponents<Tree>().Length > 0)
                {
                    TakeFruit(target);
                    
                }
                else if (target.GetComponents<House>().Length > 0)
                {
                    
                    House hou = target.GetComponent<House>();

                    hou.storedFruits+= fruits;
                    fruits = 0;
                    hou.UpdateText();

                    

                    if (!gm.day)
                    {
                        hou.EnterHome(gameObject);
                        target = null;
                        return nextPosition;
                    }
                    else
                    {
                        target = null;
                        randomWander = new Vector3(Random.Range(-gm.bounds.x, gm.bounds.x), Random.Range(-gm.bounds.y, gm.bounds.y));
                        return Seek(randomWander);
                    }
                }

            }
            if (target)
            {
                return Seek(target) * seekScalar;
            }
            else
            {
                randomWander = new Vector3(Random.Range(-gm.bounds.x, gm.bounds.x), Random.Range(-gm.bounds.y, gm.bounds.y));
                return Seek(randomWander);
            }
            
        }
        else
        {
            if (randomWander == new Vector3(0, 0, 0) || (transform.position - randomWander).magnitude < 1)
            {
                randomWander = new Vector3(Random.Range(-gm.bounds.x, gm.bounds.x), Random.Range(-gm.bounds.y, gm.bounds.y));
            }
            return Seek(randomWander);
        }
    }
    public Vector3 Arrive(Vector3 target, float radius)
    {
        Vector3 distance = target - transform.position;
        float dMag = distance.magnitude;

        if (dMag < radius)
        {
            distance = distance.normalized * Mathf.Lerp(0, maxSpeed, (dMag / 100));
        }
        else
        {
            distance = distance.normalized * maxSpeed;
        }

        Vector3 steering = distance - velocity;
        steering = Vector3.Min(steering, (steering.normalized * maxSpeed));

        Vector3 nextPos = transform.position + (steering * Time.deltaTime);

        return nextPos;
    }

    public Vector3 Arrive(GameObject target, float radius)
    {
        return Arrive(target.transform.position, radius);
    }

    
    public void TakeFruit (GameObject tree)
    {
        Tree tr = tree.GetComponent<Tree>();
        if (tr.fruits > 0)
        {
            tr.TakeFruit();
            fruits++;

            target = home;
            return;
        }
        target = null;
    }


    

}
