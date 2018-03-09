using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPatrolPattern : MonoBehaviour {
    public int frequency;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<UpDownPatrol>())
        {
            UpDownPatrol upDownPatrol = collision.GetComponent<UpDownPatrol>();
            if (frequency == upDownPatrol.patrolFrequency)
            {
                upDownPatrol.SetDirection(!upDownPatrol.goingUp);

                
            }
        }
    }
}
