using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This class is the Game Manager, it helps out in doing certain things and maintains the game state.
/// Most of the things in the class are static and the Game Manager utilizes a simpleton pattern.
/// </summary>
public class GameManager : MonoBehaviour {

    [Header("GameManager")]
    [HideInInspector] public static GameManager gameManager;

    [Header("Revive and Reset events")]
    private static UnityEvent _reviveEventManager = new UnityEvent();
    private static UnityEvent _resetEventManager = new UnityEvent();

    [Header("PacMan")]
    private static PacMan _pacMan;

    [Header("Points")]
    private static List<Point> _allPoints = new List<Point>();
    private static int _collectedPoints = 0;

    [Header("Extra Life UI")]
    [SerializeField] private Image _extraLife1;
    [SerializeField] private Image _extraLife2;

    [Header("GameOver or Win UI")]
    [SerializeField] private GameObject _gameOverUIObject;
    [SerializeField] private Text _statusText;

    public void Awake()
    {
        Time.timeScale = 1f;

        if (gameManager == null)
            gameManager = this;
        if(gameManager != this)
            Destroy(this);

        _pacMan = GameObject.Find("PacMan").GetComponent<PacMan>();
    }

    /// <summary>
    /// This method is called by the Point class and is used to increment the total amount of points in the level.
    /// </summary>
	public static void AddPoint(Point point)
    {
        _allPoints.Add(point);
    }

    /// <summary>
    /// This method is called when Pac-Man eats a Point.
    /// </summary>
    public static void CollectPoint()
    {
        _collectedPoints++;

        if(PointsLeft() <= 0)
            Win();
    }

    /// <summary>
    /// This method is called when Pac-Man has died but has an extra life.
    /// </summary>
    public static void Revive()
    {
        Time.timeScale = 1f;
        _reviveEventManager.Invoke();

        if(_pacMan.GetExtraLives() == 1)
        {
            gameManager._extraLife1.enabled = true;
            gameManager._extraLife2.enabled = false;
        }
        else if(_pacMan.GetExtraLives() == 0)
        {
            gameManager._extraLife1.enabled = false;
            gameManager._extraLife2.enabled = false;
        }
    }

    /// <summary>
    /// This method is used to reset / restart the game. 
    /// </summary>
    public static void Reset()
    {
        Time.timeScale = 1f;
        gameManager._gameOverUIObject.SetActive(false);
        _resetEventManager.Invoke();
        gameManager._extraLife1.enabled = true;
        gameManager._extraLife2.enabled = true;
        _collectedPoints = 0;
    }

    /// <summary>
    /// This method is called when Pac-Man dies and has no extra life.
    /// </summary>
    public static void GameOver()
    {
        Time.timeScale = 0f;
        gameManager._statusText.text = "GAME OVER";
        gameManager._gameOverUIObject.SetActive(true);
    }

    /// <summary>
    /// This method is called when Pac-Man has collected all Points.
    /// </summary>
    public static void Win()
    {
        Time.timeScale = 0f;
        gameManager._statusText.text = "YOU WIN";
        gameManager._gameOverUIObject.SetActive(true);
    }

    /// <summary>
    /// Instance method to call a static method, workaround for Unity's UnityEvent system which can't handle Static methods.
    /// Called by a UI-button.
    /// </summary>
    public void InstancedReset()
    {
        Reset();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HardMode()
    {
        foreach (Point point in _allPoints)
            point.HardMode();
    }

    public static int PointsLeft()
    {
        return _allPoints.Count - _collectedPoints;
    }

    /// <summary>
    /// This method is called whenever a class wants to install listener to the Revive Event.
    /// The Revive Event is the event that happens when Pac-Man is to be revived.
    /// In the case that Pac-Man is revived, the GameManager will invoke the UnitEvent with the listeners, causing the listeners to execute the methods.
    /// </summary>
    /// <param name="call">The UnityAction call to install as a listener</param>
    public static void AddReviveEvent(UnityAction call)
    {
        _reviveEventManager.AddListener(call);
    }

    public static void RemoveReviveEvent(UnityAction call)
    {
        _reviveEventManager.RemoveListener(call);
    }

    /// <summary>
    /// This method is called whenever a class wants to install listener to the Reset Event.
    /// The Reset Event is the event that happens when the player chooses to restart the game after either running out of lives or winning the game.
    /// When the Reset Event is called, the GameManager will invoke the UnitEvent with the listeners, causing the listeners to execute the methods.
    /// </summary>
    /// <param name="call">The UnityAction call to install as a listener</param>
    public static void AddResetEvent(UnityAction call)
    {
        _resetEventManager.AddListener(call);
    }

    public static void RemoveResetEvent(UnityAction call)
    {
        _resetEventManager.RemoveListener(call);
    }
}
