using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPatrol : MonoBehaviour {




    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    PlatformerController platformerController;
    Bounds bounds;
    public LayerMask groundMask;
    public LayerMask playerMask;

    //movement variables

    Vector2 velocity;
    public float speed = 1;
    bool shouldMove = true;
    public bool goingUp = true;
    public int patrolFrequency;

    public float gravity = -20;

    public int damage = 2;
    
    public int health = 2;


    // Use this for initialization
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        platformerController = GetComponent<PlatformerController>();
        bounds = GetComponent<Collider2D>().bounds;

    }

    // Update is called once per frame
    void Update()
    {

        if (platformerController.collisionInfo.above || platformerController.collisionInfo.below)
        {
            velocity.y = 0;
        }

        if (shouldMove)
        {
            bounds = GetComponent<Collider2D>().bounds;
            //if we are currently moving up
            if (goingUp)
            {

                if (platformerController.collisionInfo.above == false)
                {

                }
                else
                {
                    SetDirection(false);
                    Debug.Log(string.Format("Flip | Speed: {0}", speed));
                    
                }
            }
            //if we are currently moving down
            else
            {
                if (platformerController.collisionInfo.below == false)
                {

                }
                else
                {
                    SetDirection(true);
                    Debug.Log(string.Format("Flip | Speed: {0}", speed));
                    
                }
            }

            Vector2 moveInput = new Vector2(0, speed);
            velocity.y = moveInput.y;
        }
        
        platformerController.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement.damageOnCooldown == false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-damage);
                playerMovement.Knockback(transform.position, 8);                
            }
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            collision.gameObject.GetComponent<Projectile>();
            ModifyHealth(collision.gameObject.GetComponent<Projectile>().damage * -1);
            Destroy(collision.gameObject);
        }
    }


    public void SetDirection(bool up)
    {
        goingUp = !goingUp;
        if (up) speed = Mathf.Abs(speed);
        else if (!up) speed = Mathf.Abs(speed) * -1;
    }

    

    public void ModifyHealth(int healthChange)
    {
        Debug.Log(string.Format("Health change: {0} | Health: {1}  healthChange, health,", healthChange, health));
        //if player can be damaged
        if (healthChange < 0)
        {
            health += healthChange;
            if (health <= 0)
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
