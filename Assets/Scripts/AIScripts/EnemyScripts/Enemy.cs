using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public State<Enemy> m_State;
    public GameObject Player;
    //public float speed;
    //public int randomPos;
    //public float waitTime;
    //public float startWaitTime;
    //public Transform[] moveToPos;

    //<SUMMARY>
    //Steering Behavior Variables
    //</SUMMARY>
    //public Vector3 velocity;
    //public float mass = 1;
    //public float maxSpeed = 1;
    //public float maxForce = 1;
    //public float maxTurnRate = 1.0f;
    public Agent agent;

    void Start()
    {
        agent = GetComponent<Agent>();
        m_State = new Patrol();
        //waitTime = startWaitTime;
        //randomPos = Random.Range(0, moveToPos.Length);
    }

    void Update()
    {
        m_State.Execute(this);

    }
}
