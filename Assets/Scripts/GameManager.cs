using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("GameManager")]
    [HideInInspector] public static GameManager gameManager;

    [Header("Revive and Reset events")]
    private static UnityEvent _reviveEventManager = new UnityEvent();
    private static UnityEvent _resetEventManager = new UnityEvent();

    [Header("PacMan")]
    private static PacMan _pacMan;

    [Header("Points")]
    private static int _totalPoints = 0;
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

	public static void AddPoint()
    {
        _totalPoints++;
    }

    public static void CollectPoint()
    {
        _collectedPoints++;

        if(PointsLeft() <= 0)
            Win();
    }

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

    public static void Reset()
    {
        Time.timeScale = 1f;
        gameManager._gameOverUIObject.SetActive(false);
        _resetEventManager.Invoke();
        gameManager._extraLife1.enabled = true;
        gameManager._extraLife2.enabled = true;
        _collectedPoints = 0;
    }

    public static void GameOver()
    {
        Time.timeScale = 0f;
        gameManager._statusText.text = "GAME OVER";
        gameManager._gameOverUIObject.SetActive(true);
    }

    public static void Win()
    {
        Time.timeScale = 0f;
        gameManager._statusText.text = "YOU WIN";
        gameManager._gameOverUIObject.SetActive(true);
    }

    public void InstancedReset()
    {
        Reset();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static int PointsLeft()
    {
        return _totalPoints - _collectedPoints;
    }

    public static void AddReviveEvent(UnityAction call)
    {
        _reviveEventManager.AddListener(call);
    }

    public static void RemoveReviveEvent(UnityAction call)
    {
        _reviveEventManager.RemoveListener(call);
    }

    public static void AddResetEvent(UnityAction call)
    {
        _resetEventManager.AddListener(call);
    }

    public static void RemoveResetEvent(UnityAction call)
    {
        _resetEventManager.RemoveListener(call);
    }
}
