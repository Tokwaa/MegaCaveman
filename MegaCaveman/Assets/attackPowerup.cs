using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackPowerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Attack power before"+ PlayerStatController.AttackPower);
        PlayerStatController.AttackPower += 1;
        print("Attack power after" + PlayerStatController.AttackPower);
        Destroy(gameObject);
    }
}
