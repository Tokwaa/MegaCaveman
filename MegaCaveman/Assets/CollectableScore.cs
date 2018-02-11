using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScore : MonoBehaviour {

    public int scoreMultiplier;
    public int scoreIncrease;
    int score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {

            collidedObject.GetComponent<PlayerMovement>().ModifyScore(scoreIncrease, scoreMultiplier);
            Destroy(gameObject);
        }
    }

    
    
}
