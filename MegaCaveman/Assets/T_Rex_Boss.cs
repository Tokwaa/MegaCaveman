using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Rex_Boss : MonoBehaviour {

    // 
    //public int state = 0;
    public enum State { Attacks1, Attacks2, Attacks3, Dying}

    public State state;

    public Transform[] meleePositions;
    public Transform[] firePositions;


	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        switch (state)
        {
            case State.Attacks1:

                break;

            case State.Attacks2:

                break;
            case State.Attacks3:

                break;

            case State.Dying:

                break;
        }
    }

    IEnumerator BossBattle()
    {
        yield return new WaitForSeconds(1);
    }

}
