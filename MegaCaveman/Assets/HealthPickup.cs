﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

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
            PlayerMovement playerMovement = collidedObject.GetComponent<PlayerMovement>();
            if(playerMovement.health < playerMovement.maxHealth)
            {
            playerMovement.ModifyHealth(1);
            Destroy(gameObject);

            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collidedObject.GetComponent<PlayerMovement>();
            if (playerMovement.health < playerMovement.maxHealth)
            {
                playerMovement.ModifyHealth(1);
                Destroy(gameObject);

            }

        }
    }
}
