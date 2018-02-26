﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&Input.GetAxis("Vertical")<0)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetAxis("Vertical") < 0)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}