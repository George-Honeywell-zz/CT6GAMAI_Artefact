using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public State<Enemy> m_State;
    public GameObject Player;
    public Agent agent;
    public AudioSource alert_sound;

    void Start()
    {
        agent = GetComponent<Agent>();
        m_State = new Patrol();
    }

    void Update()
    {
        m_State.Execute(this);
    }
}
