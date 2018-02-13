using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialPatrol : MonoBehaviour {



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
    bool facingRight = true;
    public int patrolFrequency;

    public float gravity = -20;

    public int damage = 2;
    public bool canFire=true;
    public float fireRate;
    public GameObject dropProjectileGO;
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
            //if we are currently moving right
            if (facingRight)
            {

                if (platformerController.collisionInfo.right == false)
                {

                   
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
        if(canFire)
        {
            Debug.DrawRay(transform.position, Vector2.down*5, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5, playerMask);
            if(hit.collider.gameObject.CompareTag("Player"))
            {
            StartCoroutine("FireBelow");

            }
        }

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
                playerMovement.velocity.y += 10;
            }
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            collision.gameObject.GetComponent<Projectile>();
            ModifyHealth(collision.gameObject.GetComponent<Projectile>().damage * -1);
            Destroy(collision.gameObject);


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("AirPatrolPoint"))
        {
            if (collision.gameObject.GetComponent<PatrolPattern>().frequency == patrolFrequency)
            {
                SetDirection(!facingRight);
                Debug.Log(string.Format("Flip | Speed: {0}", speed));
                Flip();
            }
        }
    }

    void SetDirection(bool right)
    {
        if (right) speed = Mathf.Abs(speed);
        else if (!right) speed = Mathf.Abs(speed) * -1;
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
    IEnumerator FireBelow()
    {
        canFire = false;
        
        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }

}
