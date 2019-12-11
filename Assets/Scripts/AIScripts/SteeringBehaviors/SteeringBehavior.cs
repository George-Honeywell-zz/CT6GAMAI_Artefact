using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Agent))]
public class SteeringBehavior : MonoBehaviour
{
    Agent agent;
    private Rigidbody rigidBody;

    [Header("Seek")]
    //Seek
    public bool isSeekOn = true;
    Vector3 seekOnTargetPos;
    public float seekOnStopDistance;

    [Header("Arrive")]
    public Vector3 toTarget;
    public Vector3 distanceToTarget;
    Vector3 targetPosition;
    public bool isArriveOn;

    [Header("Wander")]
    //Wander -> not needed?
    public bool isWanderOn = false;
    public float wanderRadius = 10.0f; 
    public float wanderDistance = 10.0f;
    public float wanderJitter = 1.0f;
    public Vector3 wanderTarget = Vector3.zero;
    Vector3[] path;
    int currentWayPoint;

    //Obstalce Avoidance Variables
    [Header("Obstalce Avoidance")]
    public LayerMask layerMask;
    public bool isObstalceOn = false;
    public Vector3 desiredVelocity = Vector3.zero;
    public Vector3 steeringForce = Vector3.zero;
    public Vector2 obstacleForce;
    GameObject[] objects;
    public ProjectedCube projectedCube;
    GameObject obstacleClosestGameObject;
    public float obstacleBoxSize;


    [Header("Path Following")]
    public bool isPathOn = false;
    // ~ Does this have to be called in START or AWAKE?



    void Start()
    {
        // ~ Uncommenting the below line of code, seems to break the code...
        // ~ Somehow it seems to 'remove' the brackets???

        //~This Destroys all of the obstacles in the scene.
        objects = GameObject.FindGameObjectsWithTag("Obstacle");
        //int objCount = objects.Length;
        //foreach (GameObject obj in objects)
        //{
        //    Debug.Log("<color=red>X Position: </color>" + obj.transform.position.x);
        //    Debug.Log("<color=blue>Number of Obstalces: </color>" + objCount);
        //    //Destroy(obj.gameObject);
        //}

        agent = GetComponent<Agent>();
        wanderTarget += new Vector3(Random.Range(-wanderRadius, wanderRadius) * wanderJitter, 0, Random.Range(-wanderRadius, wanderRadius) * wanderJitter);

    }

    

    public Vector3 Calculate()
    {
        Vector3 velocitySum = Vector3.zero;

        if (isSeekOn)
        {
            if (Vector3.Distance(transform.position, seekOnTargetPos) <= seekOnStopDistance)
            {
                isSeekOn = false;
                agent.velocity = Vector3.zero;
            }
            else
            {
                velocitySum += Seek(seekOnTargetPos);
            }
        }

        if (isWanderOn)
        {
            velocitySum += Wander();
        }

        if (isObstalceOn)
        {
            velocitySum += ObstacleAvoidance();
        }

        if (isArriveOn)
        {
            velocitySum += Arrive(targetPosition);
        }
        if (isPathOn)
        {
            velocitySum += PathFollowing();
        }
        return velocitySum;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        //<summary>
        //When the player gets within the defined view distance, then turn the seek behavior 'ON'
        //And seek the player
        //</summary>
        Debug.Log("Seek ON");
        Vector3 desiredVelocity = (targetPos - transform.position).normalized * agent.maxSpeed;
        return (desiredVelocity - agent.velocity);
    }

    Vector3 Arrive(Vector3 targetPosition)
    {
        //<summary>
        //Works like 'SEEK' but applies a scalar to the velocity to adjust its magnitude
        //based on the distance to the target
        //</summary>
        Vector3 toTarget = targetPosition - agent.transform.position;
        float distance = toTarget.magnitude;
        float slowingDistance;

        if(distance > 0)
        {
            slowingDistance = 0.005f;
            agent.speed = distance / slowingDistance;
            Mathf.Clamp(agent.speed, 0.0f, 1.0f);
            desiredVelocity = toTarget.normalized * agent.speed / distance;
        }

        return desiredVelocity - agent.velocity;
    }

    Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = (transform.position - targetPos).normalized * agent.maxSpeed;
        return (desiredVelocity - agent.velocity);
    }

    public void NewWanderTarget()
    {
        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        Debug.Log("Target Reassigned");
    }

    Vector3 Wander()
    {
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector3 targetLocal = wanderTarget;
        Vector3 targetWorld = transform.position + wanderTarget;
        targetWorld += transform.forward * wanderDistance;
        return targetWorld - transform.position;
    }

    public void SeekOn(Vector3 targetPos, float stoppingDistance = 0.01f)
    {

        isSeekOn = true;
        seekOnTargetPos = targetPos;
        seekOnStopDistance = stoppingDistance;
    }

    public void ArriveOn()
    {
        isArriveOn = true;
    }

    public void WanderOn()
    {
        isWanderOn = false;
        InvokeRepeating("NewWanderTarget", 0, 1);
    }

    public void WanderOff()
    {
        isWanderOn = false;
        agent.velocity = Vector3.zero; 
    }

    public void PathFollowingOn(Vector3[] path)
    {
        isPathOn = true;
        this.path = path;
        currentWayPoint = 0;
    }

    public void ObstalceAvoidanceOn()
    {
        isObstalceOn = true;
    }

    public void ObstacleAvoidanceOff()
    {
        isObstalceOn = false;
        agent.velocity = Vector3.zero;
    }

    Vector3 PathFollowing()
    {
        //If the agent is close enough to paths.currentWayPoint
        if ((transform.position - path[currentWayPoint]).magnitude < 1.5)
        {
            //paths.setnextwaypoint
            currentWayPoint++;
        }

        //If this isn't the last way point in the list
        if (path.Length <= currentWayPoint)
        {
            //Return Seek(Paths.CurrentWayPoint)
            return Seek(path[currentWayPoint]);
        }
        else
        {
            //Arrive at a wayPoint on the Path
            return Arrive(path[currentWayPoint]);
        }
    }

    Vector3 ObstacleAvoidance()
    {
        obstacleForce = new Vector2();

        //Project a detection box in front of the agent
        //In this case, a Box Collider has been created with a script attached to it
        projectedCube.transform.localScale = new Vector3(gameObject.GetComponent<Collider>().bounds.size.x,
            projectedCube.transform.localScale.y,
            obstacleBoxSize + (agent.velocity.magnitude / agent.maxSpeed) * obstacleBoxSize);


        projectedCube.transform.localPosition = new Vector3(0, 0, projectedCube.transform.localScale.z / 2);
        float boxSize = obstacleBoxSize + (agent.speed / agent.maxSpeed) * obstacleBoxSize;
        obstacleClosestGameObject = null;

        float distance = float.MaxValue;
        Collider closestObject = new Collider();
        bool foundObject = false;
        Vector2 localPositionOfCloesetObject = new Vector2();
        
        //Iterate through all "tagged" obstacles 
        foreach(GameObject item in projectedCube.collidedObjects)
        {
            if(Vector2.Distance(transform.position, item.transform.position) < distance && (item.CompareTag("Obstacle")) && item != gameObject)
            {
                //Convert obstacles to LOCAL SPACE from WORLD SPACE.
                Vector2 localPosition = transform.InverseTransformPoint(item.transform.position);
                var expandedRadius = item.GetComponent<Collider>().bounds.size.magnitude + agent.GetComponent<Collider>().bounds.size.magnitude;

                //Check if Objects intersect with detection box
                if (Mathf.Abs(localPosition.y) < expandedRadius)
                {
                    float _X = localPosition.x;
                    float _Y = localPosition.y;
                    float SqrtPart = Mathf.Sqrt(expandedRadius * expandedRadius - _Y * _X);

                    float ip = _X - SqrtPart;

                    if(ip <= 0.0)
                        ip = _X + SqrtPart;

                    //Use the closest intersection point to the agent
                    if(ip < distance)
                    {
                        //Find what obstacle the intersection point belongs to and use that as the closest obstacle
                        distance = ip;
                        foundObject = true;
                        closestObject = item.GetComponent<Collider>();
                        obstacleClosestGameObject = item.gameObject;
                        localPositionOfCloesetObject = localPosition;
                    }
                }
            }
        }

        //If we have a closest obstacle
        Vector2 steeringForce = new Vector2();
        if (foundObject)
        {
            Debug.Log("<color=red>Obstacle Found!</color>");
            //The further away the obstalce is, the smaller the ForceMultipler
            float forceMultiplier = 1.0f + (boxSize - localPositionOfCloesetObject.x) / boxSize;
            float magnitude = closestObject.bounds.size.magnitude;

            //Get the Y Direction that is perpendicular to the obstacle relative to the agent's location
            steeringForce.y = (magnitude - localPositionOfCloesetObject.y) * forceMultiplier;

            float breakingWeight = 0.005f;
            steeringForce.x = (magnitude - localPositionOfCloesetObject.x) * breakingWeight;

            //Convert Steering Force back to WORLD SPACE and return it
            Vector3 transformForce = transform.TransformPoint(new Vector3(steeringForce.x, 0, steeringForce.y));
            obstacleForce = new Vector2(transformForce.z, transformForce.x);
            //obstacleForce = new Vector2(transformForce.z, transformForce.x);
            return new Vector2(transformForce.z, transformForce.x);
        }

        //If no obstacle was found, return a zeroed steering force
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(transform.position, boundingSphereRadius);
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstalceDistance);
        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawWireCube(transform.forward, new Vector3(2.5f, 2.5f, 20.0f));





        if (agent == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + desiredVelocity);


        //if(SteeringBehavior.Rigidbody != null)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        //}
    }
}
