using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Enemy>
{
    
    public override void Execute(Enemy enemy)
    {
        Debug.Log("~ CHASE STATE ~"); //Used for debugging

        Vector3 targetDirection = enemy.Player.transform.position - enemy.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);
        enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.forward);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.15f);

        if (angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("In Sight!");

            enemy.transform.LookAt(enemy.Player.transform);
            enemy.agent.sb.SeekOn(enemy.Player.transform.position);
        }

        if (angle > 45.0 && seeDistance > 10.0)
        {
            Debug.Log("Not in Sight");
            enemy.m_State = new Patrol();
        }
    }
}
