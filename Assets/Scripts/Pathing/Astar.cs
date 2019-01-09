using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* implementation based on Tutorial series https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW with some modifications.
/// </summary>
public static class Astar{

	public static List<Node> FindPath(Node startNode, Node endNode, int maxPriorityQueueSize)
    {
        if (startNode == null || endNode == null || maxPriorityQueueSize <= 0)
            return null;

        PriorityQueue<Node> openSet = new PriorityQueue<Node>(maxPriorityQueueSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == endNode)
                return RetracePath(startNode, endNode);

            foreach (Node neighbour in currentNode.GetNeighbours())
            {
                if (closedSet.Contains(neighbour))
                    continue;

                float newMovementCostToNeighbour = currentNode.gCost + currentNode.GetDistance(neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = neighbour.GetDistance(endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }

        return null;
    }

    private static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}
