using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    Agent agent;
    
    public LayerMask layerMask;
    public float boundingSphereRadius = 1;
    public float obstcaleDistance = 10.0f;
    public float forceConversion = 0.9f;
    public float forceConversionDuration = 1;
    public float floorAngle = 45;
    public float forceTimer = 0;
    public Vector3 oldSteeringForce = Vector3.zero;
    public Vector3 desiredVelocity = Vector3.zero;
    public Vector3 steeringForce = Vector3.zero;

    void Update()
    {
        //Cast a ray from the centre of the agent, in it's forward direction
        Ray ray = new Ray(transform.position, transform.forward);

        //Name A raycastHit
        RaycastHit hitInfo;

        //Set avoidanceForce to ZERO on all axis.
        Vector3 avoidanceForce = Vector3.zero;

        //Calculate the 'AVOIDANCE FORCE'
        if(Physics.SphereCast(ray, boundingSphereRadius, out hitInfo, obstcaleDistance, layerMask))
        {
            if(Vector3.Angle(hitInfo.normal, transform.up) > floorAngle)
            {
                //Reflect the Vector
                avoidanceForce = Vector3.Reflect(agent.velocity, hitInfo.normal);

                //Calculate the dot product between the Force and the Velocity
                if(Vector3.Dot(avoidanceForce, agent.velocity) < -0.9f)
                {
                    //Transform Right
                    avoidanceForce = transform.right;
                }
            }
        }

        if(avoidanceForce != Vector3.zero)
        {
            //Calculate the Desired Velocity
            desiredVelocity = (avoidanceForce).normalized * agent.maxSpeed;

            //Calculate the steering force
            steeringForce = desiredVelocity - agent.velocity;
            oldSteeringForce = steeringForce;
            forceTimer = 0;
        }
        else
        {
            steeringForce = Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, boundingSphereRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstcaleDistance);

        if(agent == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + desiredVelocity);

        //if (SteeringCore.Rigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }
    }
}
