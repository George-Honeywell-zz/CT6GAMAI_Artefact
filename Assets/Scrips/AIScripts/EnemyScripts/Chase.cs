using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Enemy>
{
    public GameObject Player;
    public ConeOfVision coneOfVis;
    private Material material;

    Color orange = new Vector4(1.0f, 0.6f, 0.0f);

    public override void Execute(Enemy enemy)
    {
        Vector3 targetDirection = coneOfVis.target.position - coneOfVis.transform.position;
        float angle = Vector3.Angle(targetDirection, coneOfVis.transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, coneOfVis.transform.forward);

        if(angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("In Sight!");
            material.color = orange;
            coneOfVis.transform.LookAt(Player.transform);
            coneOfVis.transform.position += coneOfVis.transform.forward * enemy.speed * Time.deltaTime;
        }

        if(angle > 45.0 && seeDistance > 10.0)
        {
            
        }
    }
}
