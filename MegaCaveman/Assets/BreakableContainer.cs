using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableContainer : MonoBehaviour {

    
    float health = 1;
    public GameObject[] drops;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            health -= 1;
            if(health <=0)
            {
                Instantiate (drops[Random.Range(0, drops.Length)],transform.position,transform.rotation);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
