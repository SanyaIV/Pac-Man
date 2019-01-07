using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [Header("Constants")]
    [HideInInspector] public const float MAX_CHECK_DISTANCE = 10f;

    [Header("Connected Nodes")]
    [SerializeField] private Node _up;
    [SerializeField] private Node _down;
    [SerializeField] private Node _left;
    [SerializeField] private Node _right;
    [SerializeField] private LayerMask _layerMask;
	
    public void FindNeighbours()
    {
        _up = RaycastForNode(Vector2.up);
        _down = RaycastForNode(Vector2.down);
        _left = RaycastForNode(Vector2.left);
        _right = RaycastForNode(Vector2.right);
    }

    private Node RaycastForNode(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + ((Vector3)direction.normalized * 0.5f), direction, MAX_CHECK_DISTANCE, _layerMask);

        if (hit.collider != null)
            return hit.collider.GetComponent<Node>();
        else
            return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (_up) Gizmos.DrawLine(transform.position, _up.transform.position);
        if (_down) Gizmos.DrawLine(transform.position, _down.transform.position);
        if (_left) Gizmos.DrawLine(transform.position, _left.transform.position);
        if (_right) Gizmos.DrawLine(transform.position, _right.transform.position);
    }

    public Node GetNodeInDirection(Vector2 direction)
    {
        if (direction.y > 0) return _up;
        if (direction.y < 0) return _down;
        if (direction.x < 0) return _left;
        if (direction.x > 0) return _right;
        else return null;
    }

    public Node GetRandomConnectedNode()
    {
        int direction = 0;
        bool running = true;

        while(running){
            direction = Random.Range(1, 5);

            if (direction == 1 && _up != null)
                return _up;
            if (direction == 2 && _down != null)
                return _down;
            if (direction == 3 && _left != null)
                return _left;
            if (direction == 4 && _right != null)
                return _right;
        }

        return null;
    }
}
