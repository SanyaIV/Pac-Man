using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("GameManager")]
    [HideInInspector] public static GameManager gameManager;

    [Header("PacMan")]
    private static PacMan _pacMan;
    private static Vector2 _startPos;
    private static Node _startDestination;

    [Header("Points")]
    private static List<Point> _points = new List<Point>();
    private static int _collectedPoints = 0;

    [Header("Extra Life")]
    private static int _extraLives = 2;
    [SerializeField] private Image _extraLife1;
    [SerializeField] private Image _extraLife2;

    public void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(this);

        _pacMan = GameObject.Find("PacMan").GetComponent<PacMan>();
        _startPos = (Vector2)_pacMan.transform.position;
        _startDestination = _pacMan.GetDestination();
    }

	public static void AddPoint(Point point)
    {
        _points.Add(point);
    }

    public static void CollectPoint()
    {
        _collectedPoints++;
    }

    public static void KillPacMan()
    {
        if (_extraLives > 0)
            Revive();
        else
            _pacMan.gameObject.SetActive(false);
    }

    public static void Revive()
    {
        //Write logic to restart with one less extra life
        _extraLives--;
        //_pacMan.GetComponent<Rigidbody2D>().MovePosition(_startPos);
        _pacMan.transform.position = _startPos;
        _pacMan.SetDestination(_startDestination);

        if(_extraLives == 1)
        {
            GameManager.gameManager._extraLife1.enabled = true;
            GameManager.gameManager._extraLife2.enabled = false;
        }
        else if(_extraLives == 0)
        {
            GameManager.gameManager._extraLife1.enabled = false;
            GameManager.gameManager._extraLife2.enabled = false;
        }
    }

    public static void Restart()
    {
        foreach(Point point in _points)
            point.gameObject.SetActive(true);

        _collectedPoints = 0;
    }

    public static int PointsLeft()
    {
        return _points.Count - _collectedPoints;
    }
}
