using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a Node which is used to navigation.
/// Each node keeps track of its neighbours.
/// </summary>
public class Node : MonoBehaviour, IPriorityQueueItem<Node> {

    [Header("Constants")]
    [HideInInspector] public const float MAX_CHECK_DISTANCE = 10f;
    [HideInInspector] public const float HALF = 0.5f;

    [Header("Connected Nodes")]
    [SerializeField] private Node _up;
    [SerializeField] private Node _down;
    [SerializeField] private Node _left;
    [SerializeField] private Node _right;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private List<Node> _neighbours;

    [Header("A*")]
    public float gCost;
    public float hCost;
    public float fCost { get { return gCost + hCost; } }
    public Node parent;

    [Header("Priority Queue")]
    private int priorityQueueIndex;
	
    /// <summary>
    /// This method is used to find the neighbouring nodes.
    /// </summary>
    public void FindNeighbours()
    {
        _neighbours = new List<Node>();

        if ((_up = RaycastForNode(Vector2.up)) != null)
            _neighbours.Add(_up);
        if ((_down = RaycastForNode(Vector2.down)) != null)
            _neighbours.Add(_down);
        if ((_left = RaycastForNode(Vector2.left)) != null)
            _neighbours.Add(_left);
        if ((_right = RaycastForNode(Vector2.right)) != null)
            _neighbours.Add(_right);

    }

    /// <summary>
    /// This method is used to get a node in a specific direction, if one exists and there is no wall in the way.
    /// </summary>
    /// <param name="direction">The direction in which to look for a Node</param>
    /// <returns>Returns a Node if one is found</returns>
    private Node RaycastForNode(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + ((Vector3)direction.normalized * HALF), direction, MAX_CHECK_DISTANCE, _layerMask);

        if (hit.collider != null)
            return hit.collider.GetComponent<Node>();
        else
            return null;
    }

    /// <summary>
    /// This method is called when a Node is selected in the editor.
    /// When a Node is selected it will draw a line to its neighbours.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (_up) Gizmos.DrawLine(transform.position, _up.transform.position);
        if (_down) Gizmos.DrawLine(transform.position, _down.transform.position);
        if (_left) Gizmos.DrawLine(transform.position, _left.transform.position);
        if (_right) Gizmos.DrawLine(transform.position, _right.transform.position);
    }

    /// <summary>
    /// This method is used to get and return a Node in a certain direction.
    /// </summary>
    /// <param name="direction">The direction to check in</param>
    /// <returns>Returns the Node in the specific direction or null if none is found</returns>
    public Node GetNodeInDirection(Vector2 direction)
    {
        if (direction.y > 0) return _up;
        if (direction.y < 0) return _down;
        if (direction.x < 0) return _left;
        if (direction.x > 0) return _right;
        else return null;
    }

    /// <summary>
    /// This method gets a random Node that is connected to this Node.
    /// Mainly used for Enemy movement which is random.
    /// </summary>
    /// <returns>A Randomly selected Node</returns>
    public Node GetRandomConnectedNode()
    {
        if (_neighbours.Count > 0)
            return _neighbours[Random.Range(0, _neighbours.Count)];
        else
            return null;
    }

    public List<Node> GetNeighbours()
    {
        return _neighbours;
    }

    public int PriorityQueueIndex
    {
        get
        {
            return priorityQueueIndex;
        }
        set
        {
            priorityQueueIndex = value;
        }
    }

    public float GetDistance(Node other)
    {
        return Vector2.Distance(transform.position, other.transform.position);
    }

    public int CompareTo(Node nodeToCompareTo)
    {
        int compare = fCost.CompareTo(nodeToCompareTo.fCost);

        if(compare == 0)
            compare = hCost.CompareTo(nodeToCompareTo.hCost);

        return -compare;
    }
}
