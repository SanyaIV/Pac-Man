using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : MonoBehaviour {

    [Header("Enemies")]
    [SerializeField] private Enemy _blinky;
    [SerializeField] private Enemy _inky;
    [SerializeField] private Enemy _pinky;
    [SerializeField] private Enemy _clyde;

    void Start () {
        GameManager.AddResetEvent(ResetEvent);
	}

    private void MakeGhostsVulnerable()
    {
        _blinky.MakeVulnerable();
        _inky.MakeVulnerable();
        _pinky.MakeVulnerable();
        _clyde.MakeVulnerable();
    }
	
	public void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("PacMan"))
        {
            MakeGhostsVulnerable();
            gameObject.SetActive(false);
        }
    }

    public void ResetEvent()
    {
        gameObject.SetActive(true);
    }
}
