using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

	void Start () {
        GameManager.AddPoint(this);
	}

    public void Enable()
    {
        gameObject.SetActive(true);
    }

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
}
