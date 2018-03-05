using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (PlatformerController))]

public class PlayerMovement : MonoBehaviour {

    PlatformerController platformerController;
    SpriteRenderer spriteRenderer;


    //movement variables

    public float jumpHeight=2;
    public float jumpApexTime=1;

    public float jumpVelocity = 15;

    public float walkSpeed;
    public float gravity = -5;
    public float allowedJumps = 1;

    public bool shouldMove = true;

    public bool facingRight = true;
    public Vector3 velocity;

    //Stats and acompanying UI

    public int score;
    public Text scoreTxt;
    public int maxHealth = 10;
    public int health=10;
    public List<Image> healthContainers = new List<Image>();
    public float damageCooldownTime=2;
    public float damageBlinkRate = 0.2f;
    public bool blinkOn=false;
    public bool damageOnCooldown=false;
    
    //projectile stats & variables
    public float fireRate;
    public bool canFire = true;
    public GameObject projectile_GO;
    public Transform spawnPos;

    public GameObject shootPos;

    //sounds & variables

    AudioSource audioSource;

    public AudioClip DeathTempAudioClip;
    public AudioClip JumpTempAudioClip;
    public AudioClip ShootTempAudioClip;

    // Use this for initialization
    void Start () {
        platformerController = GetComponent<PlatformerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        UpdateHealth();

        gravity = -(2 * jumpHeight) / Mathf.Pow(jumpApexTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpApexTime;
        //Debug.Log(string.Format("Gravity: {0} Jump Velocity: {1}", gravity, jumpVelocity));

        scoreTxt.text = string.Format("Eggs: {0}", score);

	}
	
	// Update is called once per frame
	void Update ()
    {

        if(platformerController.collisionInfo.above || platformerController.collisionInfo.below)
        {
            velocity.y = 0;
        }
        if (shouldMove)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //if we want to go down ignoreOneWayCollision else don't
            platformerController.ignoreOneWayCollision = moveInput.y < 0 ? true:false;
            if (moveInput.x < 0 && facingRight)
            {
                Flip();
            }
            if (moveInput.x > 0 && !facingRight)
            {
                Flip();
            }



            if (Input.GetButtonDown("Jump") && platformerController.collisionInfo.below)
            {
                velocity.y = jumpVelocity;
                audioSource.PlayOneShot(JumpTempAudioClip);

            }
            if (Input.GetButton("Fire1"))
            {
                if (canFire)
                {
                    StartCoroutine("FireProjectile");

                }
            }
        velocity.x = moveInput.x * walkSpeed;
        velocity.y += gravity * Time.deltaTime;
        }

        platformerController.Move(velocity * Time.deltaTime);
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        
    }


    public void Knockback(Vector2 forceApplyingPosition,float force)
    {
        Vector2 hitDir = ((Vector2)transform.position - forceApplyingPosition);
        hitDir.y = Mathf.Clamp(hitDir.y, -5, 1);
        hitDir.Normalize();
        velocity += (Vector3)hitDir * force;
    }

    public void ModifyScore(int scoreChange, int scoreMultiplier)
    {
        score += scoreChange;
        score *= scoreMultiplier;
        
        scoreTxt.text = string.Format("Eggs: {0}", score);
    }

    
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;

        localScale.x *= -1;
        transform.localScale = localScale;
    }
    public void Die()
    {
        
        
        
        shouldMove = false;  
        StartCoroutine("DieTimer");
    }

    IEnumerator FireProjectile()
    {
        canFire = false;
        audioSource.PlayOneShot(ShootTempAudioClip);
        GameObject projectileClone = Instantiate(projectile_GO, spawnPos.position, projectile_GO.transform.rotation);
        if(!facingRight)
        {
            projectileClone.GetComponent<Projectile>().dir = -1;
            projectileClone.GetComponent<Projectile>().Orient();
        }
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
    public IEnumerator DieTimer()
    {
        audioSource.PlayOneShot(DeathTempAudioClip);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void ModifyHealth(int healthChange)
    {
        Debug.Log(string.Format("Health change: {0} | Health: {1} | Can player be damaged? {2}", healthChange, health,damageOnCooldown?"no":"yes"));

        //if player can be damaged
        if (healthChange < 0)
        {
            if (damageOnCooldown == false)
            {
                damageOnCooldown = true;
                StartCoroutine("DamageTimer", healthChange);
            }
        }
        else
        {
            health += healthChange;
            UpdateHealth();
        }
        
    }

    public void UpdateHealth()
    {

        int index = 0;
        foreach (Image healthImage in healthContainers)
        {
            // if(i+1[1] >= 10)
            //enabled sprite
            if (index+1 <= health)
            {
                healthImage.enabled = true;
            }

            else
            {
                healthImage.enabled = false;
            }
            index++;
        }
    }
    IEnumerator DamageTimer(int healthChange)
    {

        health += healthChange;
        UpdateHealth();
        //StartCoroutine("DamageCooldownEffect");
        InvokeRepeating("DamageCooldownEffect", 0, damageBlinkRate);
        if (health<=0)
        {
            Die();
        }

        yield return new WaitForSeconds(damageCooldownTime);
        CancelInvoke("DamageCooldownEffect");
        Color color = spriteRenderer.color;

        color.a = 1;
        spriteRenderer.color = color;
        
        damageOnCooldown = false;

    }

    void DamageCooldownEffect()
    {
        
            print(damageBlinkRate);
            Color color = spriteRenderer.color;

            color.a = blinkOn?0:1;
            spriteRenderer.color = color;
            blinkOn = !blinkOn;
       
    }

    IEnumerator FreezePlayerInput(float freezeTime)
    {
        shouldMove = false;
        yield return new WaitForSeconds(freezeTime);
        shouldMove = true;       
        
    }

}
