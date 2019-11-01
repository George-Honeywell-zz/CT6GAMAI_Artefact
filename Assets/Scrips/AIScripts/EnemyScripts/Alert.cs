using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : State<Enemy>
{

    public GameObject Player;
    private Material material;

    Color orange = new Vector4(1.0f, 0.6f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        

        Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);
        //Debug.Log(angle);
        if (angle < 45.0 && seeDistance < 20.0)
        {
            Debug.Log("AI Agent is alert... Be careful!");   
        }

        if(angle > 45.0 && seeDistance > 20.0)
        {
            Debug.Log("Change to PATROL State");
            enemy.m_State = new Patrol();
        }

        if(angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("Change to Chase State");
            enemy.m_State = new Chase();
        }
    }
}
