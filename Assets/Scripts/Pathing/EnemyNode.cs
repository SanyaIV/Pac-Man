using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNode : MonoBehaviour {

    [Header("Enemy Node")]
    [SerializeField] private EnemyNode _nextNode;
    [SerializeField] private EnemyNode _exitNode;
    [SerializeField] private Node[] _nodeNodes;

    public EnemyNode GetNextNode()
    {
        return _nextNode;
    }

    public EnemyNode GetExitNode()
    {
        return _exitNode;
    }

    public Node GetNodeNode()
    {
        if (_nodeNodes.Length > 0)
            return _nodeNodes[Random.Range(0, _nodeNodes.Length)];
        else
            return null;
    }
}
