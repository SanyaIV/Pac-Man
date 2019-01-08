using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the Points that Pac-Man can eat.
/// </summary>
public class Point : MonoBehaviour {

	void Start () {
        GameManager.AddPoint(this);
        GameManager.AddResetEvent(ResetEvent);
	}

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// The point is disabled instead of deleted so it can just be re-enabled in the future when a Reset Event happens.
    /// </summary>
    private void Disable()
    {
        GameManager.CollectPoint();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("PacMan"))
            Disable();
    }

    public void HardMode()
    {
        if(GetComponent<SpriteRenderer>().color == Color.clear)
            GetComponent<SpriteRenderer>().color = Color.white;
        else
            GetComponent<SpriteRenderer>().color = Color.clear;
    }

    /// <summary>
    /// This method is called when a Reset Event happens.
    /// </summary>
    public void ResetEvent()
    {
        Enable();
    }
}
