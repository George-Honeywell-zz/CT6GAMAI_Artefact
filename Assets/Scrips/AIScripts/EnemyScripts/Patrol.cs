using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State<Enemy>
{

    public override void Execute(Enemy enemy)
    {
        //material.color = yellow;
        Debug.Log("In -PATROL- State");

        //Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        Vector3 targetDirection = enemy.Player.transform.position - enemy.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.transform.forward);
        Vector3 forwardDirection = enemy.transform.TransformDirection(enemy.transform.forward);

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.moveToPos[enemy.randomPos].position, enemy.speed * Time.deltaTime);
        enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.forward);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.15f);
        enemy.transform.position += forwardDirection * Time.deltaTime;

        if (Vector3.Distance(enemy.transform.position, enemy.moveToPos[enemy.randomPos].position) < 0.2f)
        {
            if(enemy.waitTime <= 0)
            {
                enemy.randomPos = Random.Range(0, enemy.moveToPos.Length);
                enemy.waitTime = enemy.startWaitTime;
            }
            else
            {
                enemy.waitTime -= Time.deltaTime;
            }
        }

        if(angle < 45.0 && seeDistance < 20.0)
        {
            //Debug.Log("Enetering 'ALERT' State");
            enemy.m_State = new Alert();
        }
    }

    void Update(Enemy enemy)
    {
        
    }
}
