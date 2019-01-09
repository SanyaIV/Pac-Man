using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents Pac-Man.
/// </summary>
[RequireComponent(typeof (Rigidbody2D))]
public class PacMan : MonoBehaviour {

    [Header("Constants")]
    [HideInInspector] public const float CHECK_DISTANCE = 30f;
    [HideInInspector] public const float MIN_CHECK_DISTANCE = 0.5f;

    [Header("Settings")]
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _nodeLayer;

    [Header("Life")]
    [SerializeField] private int _extraLives = 2; 
    
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
    private Vector2 _direction = Vector2.left;
    private Node _previousDestination;

    [Header("Reset Variables")]
    private Vector2 _startPos;
    private Vector2 _startDirection;
    private Node _startDestination;
    private int _startExtraLives;

    void Start () {
        _startPos = transform.position;
        _startDestination = _destination;
        _startExtraLives = _extraLives;
        _startDirection = _direction;

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _point = transform.position;

        GameManager.AddResetEvent(ResetEvent);
        GameManager.AddReviveEvent(ReviveEvent);
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

        _animator.SetFloat("DirX", _direction.x);
        _animator.SetFloat("DirY", _direction.y);

        _point = Vector2.MoveTowards(transform.position, _destination.transform.position, _speed);
        _rb.MovePosition(_point);

        if(transform.position == _destination.transform.position)
        {
            _previousDestination = _destination;
            if (_cachedDirection != Vector2.zero && _destination.GetNodeInDirection(_cachedDirection) != null)
            {
                _destination = _destination.GetNodeInDirection(_cachedDirection);
                _direction = _cachedDirection;
            }  
            else
            {
                _destination = _destination.GetNodeInDirection(_direction);
            }

            if (_destination == null)
            {
                _cachedDirection = Vector2.zero;
                _direction = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// This method tries to find a Node in a specific direction and then sets that node as the destination.
    /// If no Node is found then the direction will be cached to be checked later.
    /// </summary>
    /// <param name="direction">The direction to check in</param>
    void GetAndSetDestination(Vector2 direction)
    {
        _wallHit = Physics2D.Raycast(transform.position, direction, CHECK_DISTANCE, _wallLayer);
        _hit = Physics2D.Raycast(transform.position + (Vector3)direction.normalized * MIN_CHECK_DISTANCE, direction, Vector2.Distance(transform.position, _wallHit.point) - MIN_CHECK_DISTANCE, _nodeLayer);

        if (_hit.collider != null)
        {
            _destination = _hit.collider.GetComponent<Node>();
            _direction = direction;
            _cachedDirection = Vector2.zero;
            return;
        }

        _cachedDirection = direction;
    }

    public Node GetDestination()
    {
        if (_destination != null)
            return _destination;
        else
            return _previousDestination;
    }

    public void SetDestination(Node destination)
    {
        _destination = destination;
    }

    public int GetExtraLives()
    {
        return _extraLives;
    }

    /// <summary>
    /// This method is called whenever Pac-Man collides with an enemy that isn't vulnerable.
    /// This method is in charge of deciding whether to Revive Pac-Man or go into a Game Over state.
    /// </summary>
    public void Kill()
    {
        if (_extraLives > 0)
            GameManager.Revive();
        else
            GameManager.GameOver();
    }

    /// <summary>
    /// This method is called whenever the game is Reset.
    /// </summary>
    public void ResetEvent()
    {
        gameObject.SetActive(true);
        transform.position = _startPos;
        _destination = _startDestination;
        _extraLives = _startExtraLives;
        _direction = _startDirection;
    }

    /// <summary>
    /// This method is called whenever Pac-Man is revived.
    /// </summary>
    public void ReviveEvent()
    {
        transform.position = _startPos;
        _destination = _startDestination;
        _direction = _startDirection;
        _extraLives--;
    }
}
