using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    State<Enemy> m_State;

    public float speed;
    public int randomPos;
    public float waitTime;
    public float startWaitTime;
    public Transform[] moveToPos;
}
