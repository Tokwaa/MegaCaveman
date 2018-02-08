using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float damage;
    public Vector3 velocity;
    public Rigidbody2D rb;
    public int dir=1;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velocity.x = speed*dir;

        rb.velocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void Move()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void Orient()
    {
        Vector3 tempRot = transform.eulerAngles;
        tempRot.y = 180;
       
        transform.eulerAngles = tempRot;
    }
}
