using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an enemy node which is placed around to Ghost House to help enemies get out of the Ghost House without PacMan being able to get in.
/// </summary>
public class EnemyNode : MonoBehaviour {

    [Header("Enemy Node")]
    [SerializeField] private EnemyNode _nextNode;
    [SerializeField] private EnemyNode _exitNode;
    [SerializeField] private Node[] _nodeNodes;

    /// <summary>
    /// This method returns the next enemy node that the enemy is supposed to go to.
    /// </summary>
    /// <returns>Returns the next enemy node that the enemy is supposed to go to.</returns>
    public EnemyNode GetNextNode()
    {
        return _nextNode;
    }

    /// <summary>
    /// This method returns the exit node that the enemy is supposed to use when trying to exit the Ghost House.
    /// </summary>
    /// <returns>Returns the exit node that the enemy is supposed to use when trying to exit the Ghost House.</returns>
    public EnemyNode GetExitNode()
    {
        return _exitNode;
    }

    /// <summary>
    /// This method gets a random Node node and returns it to let the enemy into the Node system rather than the EnemyNode system.
    /// </summary>
    /// <returns>Returns a random Node node to let the enemy into the Node system rather than the EnemyNode system.</returns>
    public Node GetNodeNode()
    {
        if (_nodeNodes.Length > 0)
            return _nodeNodes[Random.Range(0, _nodeNodes.Length)];
        else
            return null;
    }
}
