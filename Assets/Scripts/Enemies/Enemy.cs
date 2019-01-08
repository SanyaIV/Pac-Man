using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Destination")]
    [SerializeField] private Node _destination;

    [Header("Ghost House")]
    [SerializeField] private float _timeInGhostHouse = 5f;
    [SerializeField] private EnemyNode _startNode;
    private EnemyNode _enemyNode;
    private bool _inGhostHouse = false;
    private float _ghostHouseCounter = 0f;
    private Coroutine _ghostHouseCoroutine;

    [Header("Movement")]
    [SerializeField] private float _speed = 0.2f;
    private Vector2 _direction = Vector2.zero;
    private Vector2 _point = Vector2.zero;

    [Header("Components")]
    private Animator _animator;
    private Rigidbody2D _rb;

    [Header("Vulnerability")]
    [SerializeField] private float _vulnerableTime;
    [SerializeField] private float _vulnerableMoveSpeed;
    private bool _vulnerable = false;
    private float _vulnerableCounter;
    private Coroutine _coroutine;

    [Header("Helper Variables")]
    private Node _startDestination;
    private Vector2 _startPos;

    void Awake()
    {
        _startDestination = _destination;
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        GameManager.AddResetEvent(ResetReviveEvent);
        GameManager.AddReviveEvent(ResetReviveEvent);
    }

    void Start()
    {
        StartGhostHouseRoutine();
    }

    void FixedUpdate()
    {
        if (_destination == null)
            return;

        Rotate(transform.position, _destination.transform.position);
        Move(_destination.transform.position, _vulnerable ? _vulnerableMoveSpeed : _speed);

        if (transform.position == _destination.transform.position)
            _destination = _destination.GetRandomConnectedNode();
    }

    private void Rotate(Vector2 from, Vector2 to)
    {
        _direction = to - from;
        _animator.SetFloat("DirX", _direction.x);
        _animator.SetFloat("DirY", _direction.y);
    }

    private void Move(Vector2 toPosition, float speed)
    {
        _point = Vector2.MoveTowards(transform.position, toPosition, speed);
        _rb.MovePosition(_point);
    }

    public void StartGhostHouseRoutine()
    {
        _ghostHouseCounter = 0f;

        if (!_inGhostHouse && _startNode != null)
            _ghostHouseCoroutine = StartCoroutine(GhostHouseRoutine());

    }

    public void MakeVulnerable()
    {
        _vulnerableCounter = 0f;

        if (!_vulnerable)
            _coroutine = StartCoroutine(Vulnerable());
    }

    private void Kill()
    {
        ResetReviveEvent();
    }

	void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("PacMan") && !_vulnerable)
            coll.GetComponent<PacMan>().Kill();
        else if (coll.gameObject.layer == LayerMask.NameToLayer("PacMan") && _vulnerable)
            Kill();
    }

    public void ResetReviveEvent()
    {
        transform.position = _startPos;
        _vulnerable = false;
        _animator.SetBool("Vulnerable", false);
        _destination = _startDestination;
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        if(_ghostHouseCoroutine != null)
        {
            StopCoroutine(_ghostHouseCoroutine);
            StartGhostHouseRoutine();
        }
    }

    private IEnumerator Vulnerable()
    {
        _vulnerable = true;
        _animator.SetBool("Vulnerable", true);

        while (_vulnerableCounter < _vulnerableTime)
        {
            _vulnerableCounter += Time.deltaTime;

            yield return null;
        }

        _vulnerable = false;
        _animator.SetBool("Vulnerable", false);
        yield break;
    }

    private IEnumerator GhostHouseRoutine()
    {
        _inGhostHouse = true;
        _enemyNode = _startNode;

        while(_ghostHouseCounter < _timeInGhostHouse)
        {
            _ghostHouseCounter += Time.fixedDeltaTime;

            Rotate(transform.position, _enemyNode.transform.position);
            Move(_enemyNode.transform.position, _speed);

            if (transform.position == _enemyNode.transform.position)
                _enemyNode = _enemyNode.GetNextNode();

            yield return new WaitForFixedUpdate();
        }

        bool exiting = true;

        while (exiting)
        {
            Rotate(transform.position, _enemyNode.transform.position);
            Move(_enemyNode.transform.position, _speed);

            if (transform.position == _enemyNode.transform.position && _enemyNode.GetExitNode() != null)
                _enemyNode = _enemyNode.GetExitNode();
            else if(transform.position == _enemyNode.transform.position && _enemyNode.GetNodeNode() != null)
            {
                _destination = _enemyNode.GetNodeNode();
                exiting = false;
            }

            yield return new WaitForFixedUpdate();
        }

        _inGhostHouse = false;
        yield break;
    }
}
