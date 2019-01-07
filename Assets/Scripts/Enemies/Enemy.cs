using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Destination")]
    [SerializeField] private Node _destination;

    [Header("Movement")]
    [SerializeField] private float _speed = 0.2f;
    private Vector2 _direction = Vector2.zero;
    private Vector2 _point = Vector2.zero;

    [Header("Components")]
    private Animator _animator;
    private Rigidbody2D _rb;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_destination == null)
            return;

        if ((Vector2)_destination.transform.position - (Vector2)transform.position != Vector2.zero)
            _direction = (Vector2)_destination.transform.position - (Vector2)transform.position;
        _animator.SetFloat("DirX", _direction.x);
        _animator.SetFloat("DirY", _direction.y);

        _point = Vector2.MoveTowards(transform.position, _destination.transform.position, _speed);
        _rb.MovePosition(_point);

        if (transform.position == _destination.transform.position)
        {
            _destination = _destination.GetRandomConnectedNode();
        }
    }

	void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("PacMan"))
            GameManager.KillPacMan();
    }
}
