using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Agent))]
public class SteeringBehavior : MonoBehaviour
{
    Agent agent;


    //Seek
    bool isSeekOn = false;
    bool isPathOn = false;
    Vector3 seekOnTargetPos;
    float seekOnStopDistance;


    //Wander -> not needed?
    bool isWanderOn = false;
    public float wanderRadius = 10.0f;
    public float wanderDistance = 10.0f;
    public float wanderJitter = 1.0f;
    public Vector3 wanderTarget = Vector3.zero;
    Vector3[] path;
    int currentWayPoint;

    void Start()
    {
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
            return Seek(seekOnTargetPos);
        }
        //Else this is the last waypoint
        else
        {
            //Return Arrive(Paths.CurrentWayPoint)
            return seekOnTargetPos;
        }
    }

}
