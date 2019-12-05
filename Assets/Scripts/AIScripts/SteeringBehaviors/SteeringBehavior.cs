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
    bool isSeekOn = false;
    bool isPathOn = false;
    Vector3 seekOnTargetPos;
    float seekOnStopDistance;

    [Header("Wander")]
    //Wander -> not needed?
    bool isWanderOn = false;
    public float wanderRadius = 10.0f;
    public float wanderDistance = 10.0f;
    public float wanderJitter = 1.0f;
    public Vector3 wanderTarget = Vector3.zero;
    Vector3[] path;
    int currentWayPoint;

    //Obstalce Avoidance Variables
    [Header("Obstalce Avoidance")]
    bool isObstalceOn = true;
    public LayerMask layerMask;
    public float boundingSphereRadius = 1.0f;
    public float obstalceDistance = 10.0f;
    [Range(0, 90)]
    public float floorAngle = 45.0f;
    public float forceTimer = 0;
    public Vector3 oldSteeringForce = Vector3.zero;
    public Vector3 desiredVelocity = Vector3.zero;
    
    public Vector3 Centre;
    public float maxDistance = 25.0f;

    //New Obstacle Avoidance Variables.
    public Vector3 steeringForce = Vector3.zero;
    public GameObject[] objects = GameObject.FindGameObjectsWithTag("Obstacle");
    public Vector3 obstaclePosition;


    void Start()
    {
        agent = GetComponent<Agent>();
        wanderTarget += new Vector3(Random.Range(-wanderRadius, wanderRadius) * wanderJitter, 0, Random.Range(-wanderRadius, wanderRadius) * wanderJitter);
        //public GameObject[] objects = GameObject.FindGameObjectsWithTag("Obstacle");
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

    public void WanderOn()
    {
        isWanderOn = true;
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
        ObstacleAvoidance();
    }

    public void ObstacleAvoidanceOff()
    {
        isObstalceOn = false;
        agent.velocity = Vector3.zero;
    }

    //Vector3 PathFollowing()
    //{
    //    //If the agent is close enough to paths.currentWayPoint
    //    if ((transform.position - path[currentWayPoint]).magnitude < 1.5)
    //    {
    //        //paths.setnextwaypoint
    //        currentWayPoint++;
    //    }

    //    //If this isn't the last way point in the list
    //    if (path.Length <= currentWayPoint)
    //    {
    //        //Return Seek(Paths.CurrentWayPoint)
    //        return Seek(path[currentWayPoint]);
    //    }
    //    else
    //    {
    //        return;
    //    }
    //Else this is the last waypoint
    //else
    //{
    //    //Return Arrive(Paths.CurrentWayPoint)
    //    return seekOnTargetPos;
    //}
    //}


    //Steering Behavior - Obstalce Avoidance
    #region Obstacle Avoidance
    //Vector3 ObstacleAvoidance()
    //{
    //    //Cast a ray from the centre of the agent, in it's forward direction
    //    Ray ray = new Ray(transform.position, transform.forward);

    //    //Name a raycastHit
    //    RaycastHit hitInfo;

    //    //Set avoidanceForce to ZERO for all axis
    //    Vector3 avoidanceForce = Vector3.zero;

    //    //Calculate the 'Avoidance Force'
    //    if (Physics.BoxCast(transform.position, new Vector3(2.5f, 2.5f, 20.0f), transform.forward, out hitInfo, transform.rotation, maxDistance))
    //    //if(Physics.SphereCast(ray, boundingSphereRadius, out hitInfo, obstalceDistance, layerMask))
    //    {
    //        if (Vector3.Angle(hitInfo.normal, transform.up) > floorAngle)
    //        {
    //            //Reflect the Vector
    //            avoidanceForce = Vector3.Reflect(agent.velocity, hitInfo.normal);

    //            //Calculate the dot product between the Force and Velocity
    //            if (Vector3.Dot(avoidanceForce, agent.velocity) < 0)
    //            {
    //                //Transform Right
    //                avoidanceForce = transform.right;
    //                Debug.Log(hitInfo);
    //            }
    //        }
    //    }

    //    if (avoidanceForce != Vector3.zero)
    //    {
    //        //Calculate desired velocity
    //        desiredVelocity = (avoidanceForce).normalized * agent.maxSpeed;
    //        desiredVelocity = avoidanceForce * agent.maxSpeed;

    //        //Calculate the steering Force
    //        steeringForce = desiredVelocity - agent.velocity;
    //        oldSteeringForce = steeringForce;
    //    }
    //    else
    //    {
    //        steeringForce = Vector3.zero;
    //    }
    //    return steeringForce;
    //}
    #endregion


    Vector3 ObstacleAvoidance()
    {
        //Variables
        float forceMultipler;
        float boxLength = 20.0f;

        //Project a detection box in front of the agent.
        RaycastHit hitInfo;
        Physics.BoxCast(transform.position, new Vector3(2.5f, 2.5f, 20.0f), transform.forward, out hitInfo, transform.rotation, maxDistance);

        //Iterate through all "tagged obstacle" and convert them to local space (relative to the vechicle's transform)       
        int objectCount = objects.Length;
        Debug.Log(objects);
        foreach (GameObject _object in objects)
        {
            obstaclePosition = transform.TransformPoint(obstaclePosition);
            Debug.Log("Help LOL");
        }

        ////Where do I get the obstacle.LocalPosition.X/Y from?
        ////Where do I get the obstacle.radius from?
        //forceMultipler = 1.0 + (boxLength - obstacle.LocalPosition.X) / boxLength;
        //steeringForce.y = (obstacle.radius - obstacle.LocalPosition.Y) * forceMultipler;

        //transform.InverseTransformPoint(_steeringForce);

        return steeringForce;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(transform.position, boundingSphereRadius);
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstalceDistance);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.forward, new Vector3(2.5f, 2.5f, 20.0f));




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
