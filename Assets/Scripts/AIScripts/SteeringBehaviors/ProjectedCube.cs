using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectedCube : MonoBehaviour
{
    public List<GameObject> collidedObjects;

    private void OnTriggerEnter(Collider other)
    {
        collidedObjects.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        collidedObjects.Remove(other.gameObject);
    }

/// <summary>
///  
/// </summary>
/// <param name="agent"></param>
/// <returns></returns>
    public Agent Check(Agent agent)
    {
        float distance = float.MaxValue;
        Agent ret = agent;
        ret = null;
        foreach(var item in collidedObjects)
        {
            var Ref = item.GetComponent<Agent>();
            if (Ref){
                var d = Vector3.Distance(agent.transform.position, item.transform.position);
                if(d < distance)
                {
                    distance = d;
                    ret = Ref;
                }
            }
        }
        return ret;
    }
}
