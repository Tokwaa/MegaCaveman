using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialPatrol : MonoBehaviour {



    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    PlatformerController platformerController;
    Transform player;

    Bounds bounds;
    public LayerMask groundMask;
    public LayerMask playerMask;

    //movement variables

    Vector2 velocity;
    public float speed = 1;
    bool shouldMove = true;
    public bool facingRight = true;
    public int patrolFrequency;

    public float gravity = -20;

    public int damage = 2;
    public bool canFire=true;
    public float fireRate=2;
    public GameObject dropProjectileGO;
    public int health = 2;


    // Use this for initialization
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        platformerController = GetComponent<PlatformerController>();
        bounds = GetComponent<Collider2D>().bounds;
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            Debug.DrawRay(transform.position, direction*8, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 8, playerMask);
            if (hit.collider != null&&direction.y<0)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    StartCoroutine("FireBelow",direction);

                }
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
    

    public void SetDirection(bool right)
    {
        if (right) speed = Mathf.Abs(speed);
        else if (!right) speed = Mathf.Abs(speed) * -1;
    }

    public void Flip()
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
    IEnumerator FireBelow(Vector2 direction)
    {
        canFire = false;

        EnemyProjectile enemyProjectile= Instantiate(dropProjectileGO, transform.position, dropProjectileGO.transform.rotation).GetComponent<EnemyProjectile>();
        enemyProjectile.UpdateVelocity(direction,8);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

}
