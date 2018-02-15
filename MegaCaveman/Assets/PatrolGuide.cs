using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGuide : MonoBehaviour {

    public int frequency;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AerialPatrol>())
        {
            AerialPatrol aerialPatrol = collision.GetComponent<AerialPatrol>();
            if (frequency == aerialPatrol.patrolFrequency)
            {
                aerialPatrol.SetDirection(!aerialPatrol.facingRight);

                aerialPatrol.Flip();
            }
        }
    }
}
