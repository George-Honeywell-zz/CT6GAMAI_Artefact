using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public State<Enemy> m_State;
    public GameObject Player;
    public float speed;
    public int randomPos;
    public float waitTime;
    public float startWaitTime;
    public Transform[] moveToPos;

    void Start()
    {
        m_State = new Patrol();
        waitTime = startWaitTime;
        randomPos = Random.Range(0, moveToPos.Length);
    }

    void Update()
    {
        m_State.Execute(this);
    }
}
