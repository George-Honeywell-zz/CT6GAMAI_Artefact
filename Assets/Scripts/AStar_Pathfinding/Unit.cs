﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{ 
    Enemy enemy;
    public Transform target;
    public float speed = 5.0f;
    //- Do not edit the variable below in the inspector -//
    Vector3[] path;
    public int targetIndex;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    //public void FollowPath()
    //{
    //    Vector3 currentWayPoint = path[0];

    //    while (true)
    //    {
    //        if (transform.position == currentWayPoint)
    //        {
    //            targetIndex++;
    //            if (targetIndex >= path.Length)
    //            {
    //                break;
    //            }
    //            currentWayPoint = path[targetIndex];
    //        }
    //        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);

    //        return;
    //    }
    //}

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];

        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if( i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                    
                }
            }
        }
    }

}
