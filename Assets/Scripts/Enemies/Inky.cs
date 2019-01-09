using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Enemy {

    public const int MAX_NODES = 82;

    public override void SetDestination()
    {
        if(_pacMan.GetDestination() != null)
            _destination = Astar.FindPath(_destination, _pacMan.GetDestination(), MAX_NODES)[0];
    }
}
