using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameManager gameManager;
    private static List<Point> _points = new List<Point>();
    private static int _collectedPoints = 0;

    public void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(this);
    }

	public static void AddPoint(Point point)
    {
        _points.Add(point);
    }

    public static void CollectPoint()
    {
        _collectedPoints++;
    }

    public static void Restart()
    {
        foreach(Point point in _points)
            point.gameObject.SetActive(true);
    }

    public static int PointsLeft()
    {
        return _points.Count - _collectedPoints;
    }
}
