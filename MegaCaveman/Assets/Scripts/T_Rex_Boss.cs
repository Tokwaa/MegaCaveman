using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Rex_Boss : MonoBehaviour {

    // 
    //public int state = 0;
    public enum State { Start,AttacksStage1, AttacksStage2, AttacksStage3, Dying}

    public State state;

    public Transform[] meleePositions;
    public Transform[] firePositions;

    public SpriteRenderer[] spriteRenderers;

    public float hitsTaken=0;
    public int maxHealth = 20;
    bool damageOnCooldown = false;
    public bool blinkOn = false;
    float damageCooldownTime = 4f;
    float damageBlinkRate=0.4f;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        switch (state)
        {
            case State.Start:
                //play starting animation

                //when done start attack stage 1
                state = State.AttacksStage1;
                break;

            case State.AttacksStage1:
                //do a random attack on a timer (shootingAngle1,shootingAngle2,stomp)

                if(hitsTaken<=20)
                {
                    state = State.AttacksStage2;
                }

                break;

            case State.AttacksStage2:
                //do a random attack on a timer
                break;
            case State.AttacksStage3:
                //do the mega attack 
                break;

            case State.Dying:

                break;
        }
    }

    IEnumerator BossBattle()
    {
        yield return new WaitForSeconds(1);
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement.damageOnCooldown == false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-1);
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement.damageOnCooldown == false)
            {
                playerMovement.StartCoroutine("FreezePlayerInput", 0.2f);
                playerMovement.ModifyHealth(-1);
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


    public void ModifyHealth(int healthChange)
    {
        Debug.Log(string.Format("Health change: {0} | Health: {1} | Can player be damaged? {2}", healthChange, hitsTaken, damageOnCooldown ? "yes" : "no"));

        //if player can be damaged
        if (healthChange < 0)
        {
            if (damageOnCooldown == false)
            {
                damageOnCooldown = true;
                StartCoroutine("DamageTimer", healthChange);
            }
        }
        

    }

    
    IEnumerator DamageTimer(int healthChange)
    {

        hitsTaken += healthChange;        
        //StartCoroutine("DamageCooldownEffect");
        InvokeRepeating("DamageCooldownEffect", 0, damageBlinkRate);
        if (hitsTaken >= 0)
        {
            state = State.Dying;
        }

        yield return new WaitForSeconds(damageCooldownTime);
        CancelInvoke("DamageCooldownEffect");
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
        Color color = spriteRenderer.color;

        color.a = 1;
        spriteRenderer.color = color;

        }

        damageOnCooldown = false;

    }

    void DamageCooldownEffect()
    {

        print(damageBlinkRate);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;

            color.a = blinkOn ? 0 : 1;
            spriteRenderer.color = color;
            blinkOn = !blinkOn;
        }

    }


}
