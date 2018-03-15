using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer spr;
    BoxCollider2D box2d;


    public float walkSpeed;
    public float runSpeed;

    public bool dirRight;
    public float apex;
    public bool isGrounded;

    //projectile stats
    float fireRate;
    public bool canFire=true;
    public GameObject projectile_GO;
    public Transform spawnPos;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        box2d = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetButtonDown("Jump")&&isGrounded)
        {
            Jump(1);
        }
        if(Input.GetButtonDown("Fire1"))
        {
            if (canFire)
            {
                StartCoroutine("FireProjectile");
            }
        }
        rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);
        transform.position += (Vector3)moveInput * walkSpeed * Time.deltaTime;

        //moving left & not facing left
        if (moveInput.x < 0 && !dirRight) FlipSprite();

        //moving left & not facing left
        if (moveInput.x > 0 && dirRight) FlipSprite();

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded=true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    void FlipSprite()
    {
        dirRight = !dirRight;
        Vector3 localScale = gameObject.transform.localScale;
        
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void Jump(float multiplier)
    {
        rb.AddForce(transform.up * apex*multiplier);
    }

    IEnumerator FireProjectile()
    {
        canFire = false;
        Projectile newProjectile = Instantiate(projectile_GO, spawnPos.position, projectile_GO.transform.rotation).GetComponent<Projectile>();
        
        yield return new WaitForSeconds(fireRate);
        canFire = true;
            
    }
}
