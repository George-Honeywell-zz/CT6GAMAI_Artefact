using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Enemy>
{

    public GameObject Player; 
    private Material material;


    Color red = new Vector4(1.0f, 0.0f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);
        //Debug.Log(angle);

        if (angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("In Sight!");
            //material.color = red;
            enemy.transform.LookAt(enemy.Player.transform);
            enemy.transform.position += enemy.transform.forward * enemy.speed * Time.deltaTime;
        }

        if (angle > 45.0 && seeDistance > 10.0)
        {
            Debug.Log("Not in Sight");
            enemy.m_State = new Patrol();
        }
    }
}
