using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Enemy {

    public const int MAX_NODES = 82;

    [Header("Percentage Chance to Chase")]
    [SerializeField][Range(0,100)]private float _chance = 50;

    public override void SetDestination()
    {
        if (Random.Range(0, 100) < _chance && _pacMan.GetDestination() != null)
            _destination = Astar.FindPath(_destination, _pacMan.GetDestination(), MAX_NODES)[0];
        else
            SetRandomDestination();
    }
}
