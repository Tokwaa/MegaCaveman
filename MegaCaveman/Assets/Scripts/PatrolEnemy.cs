﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour {


    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    PlatformerController platformerController;
    Bounds bounds;
    public LayerMask ground;
    
    //movement variables

    Vector2 velocity;
    public float speed=1;
    bool shouldMove=true;
    bool facingRight=true;

    public float gravity=-20;

    public int damage = 2;
    public int health=2;


	// Use this for initialization
	void Start () {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        platformerController = GetComponent<PlatformerController>();
        bounds = GetComponent<Collider2D>().bounds;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (platformerController.collisionInfo.above || platformerController.collisionInfo.below)
        {
            velocity.y = 0;
        }
        
        if (shouldMove)
        {
            bounds = GetComponent<Collider2D>().bounds;
            //if we are currently moving right
            if(facingRight)
            {

                if (platformerController.collisionInfo.right == false)
                {

                    //check if we will fall more than x blocks
                    Debug.DrawRay(new Vector2(bounds.max.x, bounds.min.y), Vector2.down * 1.1f, Color.red);
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, 1.1f, ground);

                    //hit ground
                    if (hit.collider != null)
                    {
                        //Debug.Log(string.Format("Hit Object: {0}", hit.collider.gameObject));

                        Debug.Log(string.Format("Hit distance: {0}", bounds.max.x, bounds.min.y));
                        if (hit.distance > 1)
                        {
                            SetDirection(false);
                            Debug.Log(string.Format("Flip | Speed: {0}", speed));
                            Flip();
                        }
                    }
                    else
                    {
                        SetDirection(false);
                        Debug.Log(string.Format("Flip | Speed: {0}", speed));
                        Flip();
                    }
                }
                else
                {
                    SetDirection(false);
                    Debug.Log(string.Format("Flip | Speed: {0}", speed));
                    Flip();
                }
            }

            else
            {
                if (platformerController.collisionInfo.left == false)
                {
                    //check if we will fall more than x blocks
                    Debug.DrawRay(new Vector2(bounds.min.x, bounds.min.y), Vector2.down * 1.1f, Color.red);
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, 1.1f, ground);
                    //hit ground
                    if (hit.collider != null)
                    {

                        //Debug.Log(string.Format("Hit Object: {0}", hit.collider.gameObject));
                        Debug.Log(string.Format("Hit distance: {0}", bounds.min.x, bounds.min.y));
                        if (hit.distance > 1)
                        {
                            SetDirection(true);
                            Debug.Log(string.Format("Flip | Speed: {0}", speed));
                            Flip();

                        }

                    }
                    else
                    {
                        SetDirection(true);
                        Debug.Log(string.Format("Flip | Speed: {0}", speed));
                        Flip();

                    }

                }
                else
                {
                    SetDirection(true);
                    Debug.Log(string.Format("Flip | Speed: {0}", speed));
                    Flip();
                }
            }

        Vector2 moveInput = new Vector2(speed, 0);
        velocity.x = moveInput.x;
        }
        velocity.y += gravity * Time.deltaTime;
        platformerController.Move(velocity * Time.deltaTime);        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement.damageOnCooldown == false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-damage);
                playerMovement.Knockback(transform.position, 8);
                playerMovement.velocity.y += 10;
            }
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            collision.gameObject.GetComponent<Projectile>();
            ModifyHealth(collision.gameObject.GetComponent<Projectile>().damage*-1);
            Destroy(collision.gameObject);


        }
    }

    void SetDirection(bool right)
    {
        if (right) speed = Mathf.Abs(speed);
        else if(!right) speed = Mathf.Abs(speed) * -1;
    }

    void Flip()
    {        
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;

        localScale.x *= -1;
        transform.localScale = localScale;

    }

    public void ModifyHealth(int healthChange)
    {
        Debug.Log(string.Format("Health change: {0} | Health: {1}  healthChange, health,",healthChange,health));

        //if player can be damaged
        if (healthChange < 0)
        {
            health += healthChange;
            if(health <= 0)
            {
                Die();
            }
        }       
    }
    public void Die()
    {
        shouldMove = false;
        Destroy(gameObject);
    }
}