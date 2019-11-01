using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    State<Enemy> currentState;
    public Enemy Agent;
    public Patrol patrol;
    States state;

    public enum States
    {
        Patrol, Alert, Chase
    }

    //public void StateUpdate()
    //{
    //    state = patrol;
    //    currentState = state.patrol;
    //    currentState.Execute(Agent);
    //}
}
