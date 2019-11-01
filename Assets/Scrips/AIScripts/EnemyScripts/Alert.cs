using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : State<Enemy>
{
    State<Enemy> enemy;
    public GameObject Player;
    private Material material;

    Color orange = new Vector4(1.0f, 0.6f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.Player.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.Player.transform.forward);

        if (seeDistance >= 20.0)
        {
            Debug.Log("AI Agent is alert... Be careful!");
            material.color = orange;
            //enemy.transform.LookAt(Player.transform);
            //enemy.Player.transform.position += enemy.Player.transform.forward * enemy.speed * Time.deltaTime;
        }

        if(seeDistance > 20.0)
        {
            Debug.Log("Change to Patrol State");
            enemy.m_State = new Patrol();
        }
    }
}
