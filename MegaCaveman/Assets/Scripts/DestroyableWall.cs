using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public int startingHealth=2;
    public int health = 2;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = startingHealth;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            health -= 1;



            float percentHealth = (float)health / (float)startingHealth;

            Color color = spriteRenderer.color;
            color.a = percentHealth;
            spriteRenderer.color = color;
            if (health <= 0)
            {               
                Destroy(gameObject);
            }
        }
    }

}
