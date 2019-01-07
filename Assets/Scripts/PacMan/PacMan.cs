using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PacMan : MonoBehaviour {

    [Header("Constants")]
    [HideInInspector] public const float CHECK_DISTANCE = 30f;

    [Header("Settings")]
    [SerializeField] private float _speed = 0.4f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _nodeLayer;

    [Header("Extra Life")]
    [SerializeField] private int _extraLives = 2;
    
    [Header("Movement")]
    [SerializeField] private Node _destination;
    private Vector2 _point = Vector2.zero;

    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _animator;
    private CircleCollider2D _coll;

    [Header("Helper Variables")]
    private RaycastHit2D _wallHit;
    private RaycastHit2D[] _hits;
    private Vector2 _direction;
    private Node _nextDestination;

    void Start () {
        _coll = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _point = transform.position;
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            GetAndSetDestination(Vector2.up);
        if (Input.GetKey(KeyCode.DownArrow))
            GetAndSetDestination(Vector2.down);
        if (Input.GetKey(KeyCode.LeftArrow))
            GetAndSetDestination(Vector2.left);
        if (Input.GetKey(KeyCode.RightArrow))
            GetAndSetDestination(Vector2.right);
    }

    void FixedUpdate()
    {
        if (_destination == null)
            return;

        _point = Vector2.MoveTowards(transform.position, _destination.transform.position, _speed);
        _rb.MovePosition(_point);

        _direction = (Vector2)_destination.transform.position - (Vector2)transform.position;
        _animator.SetFloat("DirX", _direction.x);
        _animator.SetFloat("DirY", _direction.y);
    }

    void GetAndSetDestination(Vector2 direction)
    {
        _wallHit = Physics2D.Raycast(transform.position, direction, CHECK_DISTANCE, _wallLayer);
        _hits = Physics2D.RaycastAll(transform.position, direction, Vector2.Distance(transform.position, _wallHit.point), _nodeLayer);

        foreach(RaycastHit2D hit in _hits)
            if (hit.collider != null && (_nextDestination == null || Vector2.Distance(transform.position, hit.transform.position) > Vector2.Distance(transform.position, _nextDestination.transform.position)) && Vector2.Distance(transform.position, hit.transform.position) > 0.5f)
                _nextDestination = hit.collider.GetComponent<Node>();
                

        if (_nextDestination != null)
        {
            _destination = _nextDestination;
            _nextDestination = null;
        }  
    }
}
