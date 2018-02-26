using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    float knockbackForce=5;
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

            if(playerMovement.damageOnCooldown==false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-1);
                playerMovement.Knockback(transform.position, knockbackForce);
                playerMovement.velocity.y += 10;
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collidedObject.GetComponent<PlayerMovement>();

            if (playerMovement.damageOnCooldown == false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-1);
                playerMovement.Knockback(transform.position, knockbackForce);
                playerMovement.velocity.y += 10;
            }

        }
    }
}
