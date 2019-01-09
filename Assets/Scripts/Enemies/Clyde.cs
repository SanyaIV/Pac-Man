using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Enemy {

    public const int MAX_NODES = 82;
    private List<Node> _path;
    private int _currentIndex = 0;

    public override void SetDestination()
    {
        if (_pacMan.GetDestination() != null && (_path == null || _currentIndex >= _path.Count))
        {
            GetPath();
            _currentIndex = 0;
        }

        _destination = _path[_currentIndex++];
    }

    private void GetPath()
    {
        _path = Astar.FindPath(_destination, _pacMan.GetDestination(), MAX_NODES);
    }
}
