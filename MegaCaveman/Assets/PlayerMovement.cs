using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (PlatformerController))]

public class PlayerMovement : MonoBehaviour {

    public float jumpHeight=2;
    public float jumpApexTime=1;

    public float jumpVelocity = 15;

    public float walkSpeed;
    public float gravity = -5;
    public float allowedJumps = 1;

   
    public bool facingRight = true;
    Vector3 velocity;
    PlatformerController platformerController;
    
    //projectile stats
    public float fireRate;
    public bool canFire = true;
    public GameObject projectile_GO;
    public Transform spawnPos;

    public GameObject shootPos;

    //sounds

    AudioSource audioSource;

    public AudioClip DeathTempAudioClip;
    public AudioClip JumpTempAudioClip;
    public AudioClip ShootTempAudioClip;

    // Use this for initialization
    void Start () {
        platformerController = GetComponent<PlatformerController>();
        audioSource = GetComponent<AudioSource>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(jumpApexTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpApexTime;
        Debug.Log(string.Format("Gravity: {0} Jump Velocity: {1}", gravity, jumpVelocity));
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(platformerController.collisionInfo.above || platformerController.collisionInfo.below)
        {
            velocity.y = 0;
        }

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }



        if (Input.GetButtonDown("Jump")&&platformerController.collisionInfo.below)
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

        platformerController.Move(velocity * Time.deltaTime);

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spike"))
        {
            StartCoroutine("Die");
        }
    }

    
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;

        localScale.x *= -1;
        transform.localScale = localScale;
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
    IEnumerator Die()
    {

        audioSource.PlayOneShot(DeathTempAudioClip);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);

    }

}
