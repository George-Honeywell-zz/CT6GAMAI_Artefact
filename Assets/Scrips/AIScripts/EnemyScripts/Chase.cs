using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Enemy>
{
    State<Enemy> enemy;
    public GameObject Player; 
    private Material material;


    Color red = new Vector4(1.0f, 0.0f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.Player.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.Player.transform.forward);

        if (angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("In Sight!");
            material.color = red;
            enemy.transform.LookAt(Player.transform);
            enemy.Player.transform.position += enemy.Player.transform.forward * enemy.speed * Time.deltaTime;
        }

        if (angle > 45.0 && seeDistance > 10.0)
        {
            Debug.Log("Not in Sight");
            enemy.m_State = new Alert();
        }
    }
}
