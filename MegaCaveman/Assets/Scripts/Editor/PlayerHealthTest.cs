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
	public IEnumerator PlayerHealthTestWithEnumeratorPasses() {
        tempImage = new GameObject("tempImage").AddComponent<Image>();
        GameObject tempGameObject = new GameObject();
        PlayerMovement playerMovement =  tempGameObject.AddComponent<PlayerMovement>();
        
       
        playerMovement.healthContainers = new List<Image>();
        playerMovement.maxHealth = 10;
        
        //fill health containers
        for (int i = 0; i < playerMovement.maxHealth; i++)
        {
            playerMovement.healthContainers.Add(tempImage);
            
        }
        playerMovement.UpdateHealth();        
        // yield to skip a frame
        yield return null;

        int enabledObjects = 0;
        foreach(Image image in playerMovement.healthContainers)
        {
            enabledObjects ++;
        }
        Assert.AreEqual(enabledObjects, playerMovement.health);        
    }
    [UnityTest]
    public IEnumerator PlayerHealthModificationTestWithEnumeratorPasses()
    {
        tempImage = new GameObject("tempImage").AddComponent<Image>();

        GameObject tempGameObject = new GameObject();
        PlayerMovement playerMovement = tempGameObject.AddComponent<PlayerMovement>();

        playerMovement.healthContainers = new List<Image>();
        playerMovement.maxHealth = 10;

        //fill health containers
        for (int i = 0; i < playerMovement.maxHealth; i++)
        {
            playerMovement.healthContainers.Add(tempImage);

        }
        playerMovement.damageOnCooldown = false;
        
        playerMovement.ModifyHealth(-1);
        // yield to skip a frame
        yield return null;
        playerMovement.UpdateHealth();
        // yield to skip a frame
        yield return null;

        int enabledObjects = 0;
        foreach (Image image in playerMovement.healthContainers)
        {
            enabledObjects++;
        }
        Assert.AreEqual(enabledObjects, playerMovement.health);
    }
}
