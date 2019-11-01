﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State<Enemy>
{

    private Material material;
    Color yellow = new Vector4(1.0f, 1.0f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        //material.color = yellow;

        Vector3 targetDirection = enemy.transform.position - enemy.Player.transform.position;
        float angle = Vector3.Angle(targetDirection, enemy.Player.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, enemy.Player.transform.forward);

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

        if(angle < 60.0 && seeDistance < 20.0)
        {
            Debug.Log("Enetering 'ALERT' State");
            enemy.m_State = new Alert();
        }
    }
}
