using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehavior))]
public class Agent : MonoBehaviour
{
    public Vector3 velocity;
    public float mass = 1;
    public float speed = 0.5f;
    public float maxSpeed = 1;
    public float maxForce = 1;
    public float maxTurnRate = 1.0f;
    public SteeringBehavior sb;

    void Start()
    {
        sb = GetComponent<SteeringBehavior>();
    }

    void Update()
    {
        Vector3 steeringForce = Vector3.ClampMagnitude(sb.Calculate(), maxForce);
        Vector3 acceleration = steeringForce / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity + steeringForce, maxSpeed);
        Debug.DrawRay(transform.position, velocity.normalized * 2, Color.green);

        if(velocity != Vector3.zero)
        {
            transform.position += velocity * Time.deltaTime;
            transform.forward = velocity.normalized;
        }
    }
}
