using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State<Enemy>
{
    public override void Execute(Enemy enemy)
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.moveToPos[enemy.randomPos].position, enemy.speed * Time.deltaTime);
        enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.forward);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.15f);

        if(Vector3.Distance(enemy.transform.position, enemy.moveToPos[enemy.randomPos].position) < 0.2f)
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
    }
}
