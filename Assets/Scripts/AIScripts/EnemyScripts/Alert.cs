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
        Debug.Log("Agent in 'ALERT' State");

          //Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
          Vector3 targetDirection = enemy.Player.transform.position - enemy.transform.position;


        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);
        enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.forward);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.15f);
        enemy.transform.LookAt(enemy.Player.transform);

        if (angle > 60.0 && seeDistance > 20.0)
        {
            Debug.Log("Change to PATROL State");
            enemy.m_State = new Patrol();
        }

        if(angle <= 45.0 && seeDistance < 15.0)
        {
            Debug.Log("Entering CHASE State");
            enemy.m_State = new Chase();
        }
    }
}
