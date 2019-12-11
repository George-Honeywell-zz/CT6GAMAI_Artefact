using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State<Enemy>
{

    public override void Execute(Enemy enemy)
    {
        Debug.Log("In -PATROL- State");

        Vector3 targetDirection = enemy.Player.transform.position - enemy.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);

        if (angle < 45.0 && seeDistance < 20.0)
        {
            enemy.m_State = new Alert();
        }
    }

    void Update(Enemy enemy)
    {
        
    }
}
