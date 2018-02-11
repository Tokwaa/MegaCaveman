using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if(collidedObject.CompareTag("Player"))
        {

            collidedObject.GetComponent<PlayerMovement>().velocity = -Vector3.up * 100;
            collidedObject.GetComponent<PlayerMovement>().Die();
        }
    }
}
