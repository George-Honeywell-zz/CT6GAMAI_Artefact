﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public Transform seeker;
    public Transform[] target = new Transform[5];
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        List<Node> path = new List<Node>();
        grid.path = path;
        for (int i = 0; i < 5; i++)
        {
            FindPath(seeker.position, target[i].position);
        }
    }

    void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);


        //<summary>
        //Currently the slowest 
        //</summary>
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);


            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        //List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            grid.path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //path.Reverse();

        //grid.path = path;

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}