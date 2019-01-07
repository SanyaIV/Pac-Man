using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PacMan : MonoBehaviour {

    [Header("Constants")]
    [HideInInspector] public const float CHECK_DISTANCE = 30f;
    [HideInInspector] public const float MIN_CHECK_DISTANCE = 0.5f;

    [Header("Settings")]
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _nodeLayer;
    
    [Header("Movement")]
    [SerializeField] private Node _destination;
    private Vector2 _point = Vector2.zero;
    private Vector2 _cachedDirection = Vector2.zero;

    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Helper Variables")]
    private RaycastHit2D _wallHit;
    private RaycastHit2D _hit;
    private Vector2 _direction;

    void Start () {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _point = transform.position;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            GetAndSetDestination(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            GetAndSetDestination(Vector2.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            GetAndSetDestination(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            GetAndSetDestination(Vector2.right);
    }

    void FixedUpdate()
    {
        if (_destination == null)
            return;

        if((Vector2)_destination.transform.position - (Vector2)transform.position != Vector2.zero)
            _direction = (Vector2)_destination.transform.position - (Vector2)transform.position;
        _animator.SetFloat("DirX", _direction.x);
        _animator.SetFloat("DirY", _direction.y);

        _point = Vector2.MoveTowards(transform.position, _destination.transform.position, _speed);
        _rb.MovePosition(_point);

        if(transform.position == _destination.transform.position)
        {
            if (_cachedDirection != Vector2.zero && _destination.GetNodeInDirection(_cachedDirection) != null)
                _destination = _destination.GetNodeInDirection(_cachedDirection);
            else
                _destination = _destination.GetNodeInDirection(_direction);

            if (_destination == null)
                _cachedDirection = Vector2.zero;
        }
    }

    void GetAndSetDestination(Vector2 direction)
    {
        _wallHit = Physics2D.Raycast(transform.position, direction, CHECK_DISTANCE, _wallLayer);
        _hit = Physics2D.Raycast(transform.position + (Vector3)direction.normalized * MIN_CHECK_DISTANCE, direction, Vector2.Distance(transform.position, _wallHit.point) - MIN_CHECK_DISTANCE, _nodeLayer);

        if (_hit.collider != null)
        {
            _destination = _hit.collider.GetComponent<Node>();
            _cachedDirection = Vector2.zero;
            return;
        }

        _cachedDirection = direction;
    }

    public Node GetDestination()
    {
        return _destination;
    }

    public void SetDestination(Node destination)
    {
        _destination = destination;
    }
}
