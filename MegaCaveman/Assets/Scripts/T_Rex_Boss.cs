using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Rex_Boss : MonoBehaviour {

    //general state management   
    public enum State { Start, AttacksStage1, AttacksStage2, AttacksStage3, Dying }

    public State state;

    [SerializeField] bool canAttack = true;

    public float hitsTaken=0;
    public int maxHealth = 20;
    bool damageOnCooldown = false;
    public bool blinkOn = false;
    float damageCooldownTime = 2f;
    float damageBlinkRate=0.1f;

    //attack 1 variables
    public enum Attack1 { ShootingAngle1, ShootingAngle2, Stomp }

    public Attack1 attack1;

    public Transform[] firePositions;

    public GameObject fireBallGO;

    public Transform[] meleePositions;



    public SpriteRenderer[] spriteRenderers;




    // Use this for initialization
    void Start () {
        state = State.Start;
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
                //if we should move to next stage
                if(hitsTaken>=20)
                {
                    state = State.AttacksStage2;
                }
                //else attack
                else
                {
                    if(canAttack)
                    {
                        canAttack = false;
                        //do a random attack on a timer (shootingAngle1,shootingAngle2,stomp)
                        attack1 = (Attack1)Random.Range(0, 2);
                        attack1 = Attack1.ShootingAngle1;
                        switch(attack1)
                        {
                            case Attack1.ShootingAngle1:

                                canAttack = false;                                
                                StartCoroutine("ShootingAngle",0);

                                break;

                            case Attack1.ShootingAngle2:

                                canAttack = false;
                                StartCoroutine("ShootingAngle", 1);

                                break;

                            case Attack1.Stomp:

                                canAttack = false;
                                break;
                        }
                    }
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
    IEnumerator ShootingAngle(int variant)
    {
        print("shoot");

        float elapsedTime = 0;       
        float timeToComplete = 2;

        //store starting variables
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        while (elapsedTime < timeToComplete)
        {
            print(elapsedTime/timeToComplete);

            //roughly lerp to correct position
            transform.position = Vector3.Lerp(startPosition, firePositions[variant].position, (elapsedTime / timeToComplete));

            transform.rotation = Quaternion.Lerp(startRotation, firePositions[variant].rotation, (elapsedTime / timeToComplete));

            

           
            
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
                                   
        }
        
            //hard set position
            transform.position = firePositions[variant].position;
            //hard set rotation
            transform.rotation =firePositions[variant].rotation;
            print(firePositions[variant].childCount);
        for (int i = 0; i < firePositions[variant].childCount; i++)
        {
            Debug.Log("Fireball " + (i+1)+" fired!");
            Transform tempTransform = firePositions[variant].GetChild(i);
            Instantiate(fireBallGO, tempTransform.position,tempTransform.rotation);
        }


        yield return new WaitForSeconds(timeToComplete+5);
        canAttack = true;
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
