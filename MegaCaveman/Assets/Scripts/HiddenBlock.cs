using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlock : MonoBehaviour {
    public List<SpriteRenderer> hiddenBlocks = new List<SpriteRenderer>();

	// Use this for initialization
	void Start () {
        hiddenBlocks.AddRange ( GetComponentsInChildren<SpriteRenderer>());
        hiddenBlocks.Add(GetComponent<SpriteRenderer>());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player"))
        {
            foreach (SpriteRenderer block in hiddenBlocks)
            {
                block.enabled = false;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (SpriteRenderer block in hiddenBlocks)
            {
                block.enabled = true;
            }
        }
    }
}
