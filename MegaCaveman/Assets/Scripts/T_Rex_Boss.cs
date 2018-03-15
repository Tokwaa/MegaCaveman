using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Rex_Boss : MonoBehaviour {

    //general state management   
    public enum State { Start, AttacksStage1, AttacksStage2, AttacksStage3, Dying }

    public State state;

    [SerializeField] bool canAttack = true;
    [SerializeField] bool stateRunning = false;
    public float hitsTaken = 0;
    public int maxHealth = 20;
    bool damageOnCooldown = false;
    public bool blinkOn = false;
    float damageCooldownTime = 2f;
    float damageBlinkRate = 0.1f;

    //attack 1 variables
    public enum Attack1 { ShootingAngle1, ShootingAngle2, Stomp }
    public Attack1 attack1;

    public enum Attack3 { blast1, barrage1, barrage2 }
    public Attack3 attack3;

    public Transform[] firePositions;

    public GameObject fireBallGO;

    public Transform[] stompPositions;

    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;
    public GameObject megaAttackObject;
    public GameObject megaAttackObjectTarget;

    public Collider2D secondPhaseCameraBounds;

    public SpriteRenderer[] spriteRenderers;

    public GameObject GameWinScreen;


    // Use this for initialization
    void Start() {        

    }

    // Update is called once per frame
    void Update() {
        if (stateRunning == false)
        {
            StartCoroutine("BossStateMachine");
        }

    }

    IEnumerator BossStateMachine()
    {
        stateRunning = true;
        switch (state)
        {
            case State.Start:
                //play starting animation

                //when done start attack stage 1
                yield return new WaitForSeconds(2);
                state = State.AttacksStage1;
                break;

            case State.AttacksStage1:
                //if we should move to next stage
                if (hitsTaken >= 20)
                {

                    state = State.AttacksStage2;
                }
                //else attack
                else
                {
                    if (canAttack)
                    {
                        canAttack = false;
                        //do a random attack on a timer (shootingAngle1,shootingAngle2,stomp)
                        attack1 = (Attack1)Random.Range(0, 3);

                        switch (attack1)
                        {
                            case Attack1.ShootingAngle1:

                                canAttack = false;
                                yield return StartCoroutine(LerpObjectTo(transform, firePositions[0].position, firePositions[0].rotation, 2f, -1));
                                StartCoroutine(ShootingAngle(0));

                                break;

                            case Attack1.ShootingAngle2:

                                canAttack = false;
                                yield return StartCoroutine(LerpObjectTo(transform, firePositions[1].position, firePositions[1].rotation, 2f, -1));
                                StartCoroutine(ShootingAngle(1));

                                break;

                            case Attack1.Stomp:

                                canAttack = false;
                                StartCoroutine(Stomp());
                                break;
                        }
                    }
                }

                break;

            case State.AttacksStage2:

                if (canAttack == true)
                {
                    StartCoroutine(MegaAttack());
                }

                break;
            case State.AttacksStage3:
                //if we should move to next stage
                if (hitsTaken >= 30)
                {

                    state = State.Dying;
                    
                }
                //else attack
                else
                {
                    if (canAttack)
                    {
                        canAttack = false;
                        //do a random attack on a timer (shootingAngle1,shootingAngle2,stomp)
                        attack3 = (Attack3)Random.Range(0, 3);
                        
                        switch (attack3)
                        {
                            case Attack3.blast1:

                                canAttack = false;
                                StartCoroutine(MiniBlastCross());

                                break;                            

                            case Attack3.barrage1:

                                canAttack = false;

                                StartCoroutine(ScaleObjectTo(transform, firePositions[3].localScale, 0));
                                yield return StartCoroutine(LerpObjectTo(transform, firePositions[3].position, firePositions[3].rotation, 2f, -1));
                                StartCoroutine(ShootingAngle(3));

                                break;

                            case Attack3.barrage2:

                                canAttack = false;

                                StartCoroutine(ScaleObjectTo(transform, firePositions[3].localScale, 0));
                                yield return StartCoroutine(LerpObjectTo(transform, firePositions[4].position, firePositions[4].rotation, 2f, -1));
                                StartCoroutine(ShootingAngle(4));
                                break;
                        }
                    }
                }

                
                break;

            case State.Dying:

                yield return new WaitForSeconds(1.5f);
                GameWinScreen.SetActive(true);                
                Time.timeScale = 0;
                break;
        }
        yield return null;
        stateRunning = false;
    }


    IEnumerator ShootingAngle(int variant)
    {
        print("shoot");

        for (int i = 0; i < firePositions[variant].childCount; i++)
        {
            Debug.Log("Fireball " + (i + 1) + " fired!");
            Transform tempTransform = firePositions[variant].GetChild(i);
            Instantiate(fireBallGO, tempTransform.position, tempTransform.rotation);
        }


        yield return new WaitForSeconds(1);
        canAttack = true;
    }

    IEnumerator LerpObjectTo(Transform ObjectToMove, Vector3 endPosition, Quaternion endRotation, float timeToComplete, float timeToComeBack)
    {

        float elapsedTime = 0;

        //store starting variables
        Transform startingPositionRotation = ObjectToMove.transform;
        Vector3 startPosition = ObjectToMove.transform.position;
        Quaternion startRotation = ObjectToMove.transform.rotation;
        while (elapsedTime < timeToComplete)
        {
            //print(elapsedTime / timeToComplete);

            //roughly lerp to correct position
            ObjectToMove.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timeToComplete));

            ObjectToMove.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / timeToComplete));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //hard set position
        ObjectToMove.position = endPosition;
        //hard set rotation
        ObjectToMove.rotation = endRotation;
        

        
    }
    IEnumerator ScaleObjectTo(Transform ObjectToScale, Vector3 targetSize, float timeToComplete)
    {

        float elapsedTime = 0;

        //store starting variables
        
        Vector3 startSize = ObjectToScale.localScale;      
        while (elapsedTime < timeToComplete)
        {
            //print(elapsedTime / timeToComplete);

            //roughly scale to correct size
            ObjectToScale.transform.localScale = Vector3.Lerp(startSize, targetSize, (elapsedTime / timeToComplete));


            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //hard set size
        ObjectToScale.localScale = targetSize;
        



    }
    IEnumerator Stomp()
    {
        print("stomp");

        int stompAreaIndex = Random.Range(0, 3);



        //float elapsedTime = 0;
        //float timeToComplete = 1.5f;


        Transform stompTarget = stompPositions[stompAreaIndex];
        Transform stompHazard = stompTarget.GetChild(0);

        Vector3 startPosition = stompHazard.position;
        Quaternion startRotation = stompHazard.rotation;


        yield return LerpObjectTo(stompHazard, stompTarget.transform.position, stompTarget.transform.rotation, 1.5f, 1f);

        yield return LerpObjectTo(stompHazard, startPosition, startRotation, 0.5f, 1f);


        ////store starting variables
        //Vector3 startPosition = stompHazard.transform.position;
        //Quaternion startRotation = stompHazard.transform.rotation;

        //yield return LerpObjectTo(stompHazard.transform,stompTarget,1.5f,0.5f);


        ////move stomp down
        //while (elapsedTime < timeToComplete)
        //{
        //    print(elapsedTime / timeToComplete);

        //    //roughly lerp to correct position
        //    stompHazard.transform.position = Vector3.Lerp(startPosition, stompTarget.position, (elapsedTime / timeToComplete));

        //    elapsedTime += Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}

        ////hard set position
        //stompHazard.transform.position = stompTarget.position;

        //yield return new WaitForSeconds(1);


        // elapsedTime = 0;
        // timeToComplete = 2;
        ////bring stomp back up
        //while (elapsedTime < timeToComplete)
        //{
        //    print(elapsedTime / timeToComplete);

        //    //roughly lerp to correct position
        //    stompHazard.transform.position = Vector3.Lerp(stompTarget.position, startPosition, (elapsedTime / timeToComplete));

        //    elapsedTime += Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(1);


        canAttack = true;
    }

    IEnumerator MegaAttack()
    {
        canAttack = false;
        print("MegaAttack");
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(ScaleObjectTo(transform, firePositions[2].localScale,1.5f));

        yield return StartCoroutine(LerpObjectTo(transform, firePositions[2].position, firePositions[2].rotation, 1.5f, -1));


        megaAttackObject.SetActive(true);
        StartCoroutine(LerpObjectTo(megaAttackObject.transform, megaAttackObjectTarget.transform.position, megaAttackObjectTarget.transform.rotation, 0.5f, -1));
        StartCoroutine(ScaleObjectTo(megaAttackObject.transform, megaAttackObjectTarget.transform.localScale, 0.5f));
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject objectToEnable in ObjectsToEnable)
        {
            objectToEnable.SetActive(true);            
        }
        yield return new WaitForSeconds(2);
        //blink screen
        
        Camera.main.GetComponent<RoomCamera>().UpdateBounds(secondPhaseCameraBounds);

        foreach (GameObject objectToDisable in ObjectsToDisable)
        {
            objectToDisable.SetActive(false);
        }
        megaAttackObject.SetActive(false);
        yield return new WaitForSeconds(1.05f);
        yield return StartCoroutine(ScaleObjectTo(transform, firePositions[0].localScale, 0.25f));

        yield return StartCoroutine(LerpObjectTo(transform, firePositions[0].position, firePositions[0].rotation, 0.25f, -1));
        
        state = State.AttacksStage3;
        canAttack = true;
    }

    IEnumerator MiniBlastCross()
    {
        canAttack = false;
        print("MiniBlast");

        StartCoroutine(ScaleObjectTo(transform, firePositions[5].localScale,0));
        yield return StartCoroutine(LerpObjectTo(transform, firePositions[5].position, firePositions[5].rotation, 4, -1));
        


        Transform miniBlastTarget = firePositions[5].GetChild(0);
        GameObject miniBlast = miniBlastTarget.GetChild(0).gameObject;
        miniBlast.SetActive(true);
        StartCoroutine(LerpObjectTo(miniBlast.transform, miniBlastTarget.position, miniBlastTarget.rotation, 0.75f, -1));
        StartCoroutine(ScaleObjectTo(miniBlast.transform, miniBlastTarget.transform.localScale, 0.75f));
        yield return new WaitForSeconds(0.5f);
        
        miniBlast.SetActive(false);

        StartCoroutine(ScaleObjectTo(transform, firePositions[6].localScale, 0));
        yield return StartCoroutine(LerpObjectTo(transform, firePositions[6].position, firePositions[6].rotation, 4, -1));


        miniBlastTarget = firePositions[6].GetChild(0);
        miniBlast = miniBlastTarget.GetChild(0).gameObject;
        miniBlast.SetActive(true);
        StartCoroutine(LerpObjectTo(miniBlast.transform, miniBlastTarget.position, miniBlastTarget.rotation, 0.75f, -1));
        StartCoroutine(ScaleObjectTo(miniBlast.transform, miniBlastTarget.transform.localScale, 0.75f));
        yield return new WaitForSeconds(0.5f);

        miniBlast.SetActive(false);
        yield return new WaitForSeconds(1f);



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
                playerMovement.ModifyHealth(-4);
                playerMovement.Knockback(transform.position, 16);
                playerMovement.velocity.y += 2;
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
            ModifyHealth(collision.gameObject.GetComponent<Projectile>().damage);
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

        hitsTaken += 1;        
        //StartCoroutine("DamageCooldownEffect");
        InvokeRepeating("DamageCooldownEffect", 0, damageBlinkRate);
        if (hitsTaken >= maxHealth)
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
