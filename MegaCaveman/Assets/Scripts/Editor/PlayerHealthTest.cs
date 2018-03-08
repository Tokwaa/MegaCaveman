using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealthTest {

    public Image tempImage;	

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator PlayerHealthTestWithEnumeratorPasses()
    {

        //create dummy objects and set to dummy values

        GameObject tempGameObject = new GameObject();
        PlayerMovement playerMovement =  tempGameObject.AddComponent<PlayerMovement>();                
       
        playerMovement.healthContainers = new List<Image>();
        //set max health of dummy player to 10
        playerMovement.maxHealth = 10;
        
        //generate dummy health objects for amount of max health
        for (int i = 0; i < playerMovement.maxHealth; i++)
        {
            tempImage = new GameObject("tempImage").AddComponent<Image>();
            playerMovement.healthContainers.Add(tempImage);
            
        }

        //call function that updates the health visually
        playerMovement.UpdateHealth();        
        // yield to skip a frame
        yield return null;


        //check that each dummy object is visually enabled when it should be
        int enabledObjects = 0;
        foreach(Image image in playerMovement.healthContainers)
        {
            if(image.enabled) enabledObjects++;

        }
        Assert.AreEqual(enabledObjects, playerMovement.health);        
    }
    [UnityTest]
    public IEnumerator PlayerHealthModificationTestWithEnumeratorPasses()
    {

        //create dummy objects and set to dummy values

        GameObject tempGameObject = new GameObject();
        PlayerMovement playerMovement = tempGameObject.AddComponent<PlayerMovement>();

        playerMovement.healthContainers = new List<Image>();
        //set max health of dummy player to 10
        playerMovement.maxHealth = 10;

        //generate dummy health objects for amount of max health
        for (int i = 0; i < playerMovement.maxHealth; i++)
        {
            tempImage = new GameObject("tempImage").AddComponent<Image>();
            playerMovement.healthContainers.Add(tempImage);

        }
        playerMovement.damageOnCooldown = false;

        playerMovement.spriteRenderer = tempGameObject.AddComponent<SpriteRenderer>();

        //update health with the knowledge that we have max health
        playerMovement.UpdateHealth();
        //
        playerMovement.health -= 1;
        // yield to skip a frame
        yield return null;
        //call function that updates the health visually
        //update health with the knowledge that we have max health minus 1
        playerMovement.UpdateHealth();
        // yield to skip a frame
        yield return null;

        //check that each dummy object is visually enabled when it should be
        int enabledObjects = 0;
        foreach (Image image in playerMovement.healthContainers)
        {
            if (image.enabled) enabledObjects++;
        }
        Assert.AreEqual(enabledObjects, playerMovement.maxHealth-=1);
    }
}
