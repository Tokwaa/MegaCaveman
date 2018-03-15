using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public int damage;
    public Vector3 velocity=Vector3.zero;
    public float speed=5;
    public Rigidbody2D rb;
    public bool moveForwards=false;
    public bool destroyOnImpact = true;
    public bool effectedByGround=true;

    public float lifeTime=-1;
    // Use this for initialization

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (moveForwards == false) rb.velocity = velocity * speed;
        else rb.velocity = -transform.right*speed;
        if(lifeTime>0) Destroy(gameObject, lifeTime);        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateVelocity(Vector2 dir,float force)
    {
        rb.velocity = dir*force;
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
                playerMovement.Knockback(transform.position, 6);

            }
            if(destroyOnImpact==true) Destroy(gameObject);

        }
        if (effectedByGround && collision.gameObject.CompareTag("Ground"))
        {
            if (destroyOnImpact==true) Destroy(gameObject);
        }
    }
}
