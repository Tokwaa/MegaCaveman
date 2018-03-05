using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayerHealthTest {

    public Image tempImage;

    [Test]
    public void PlayerHealthTestSimplePasses()
    {
        // Use the Assert class to test conditions.

    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator PlayerHealthTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        PlayerMovement playerMovement = new PlayerMovement();
        playerMovement.healthContainers = new List<Image>();
        playerMovement.maxHealth = 10;

        //fill health containers
        for (int i = 0; i < playerMovement.maxHealth; i++)
        {
            playerMovement.healthContainers.Add(tempImage);
        }
        // yield to skip a frame
        yield return null;
        playerMovement.UpdateHealth();

        int enabledObjects = 0;
        foreach (Image image in playerMovement.healthContainers)
        {
            if(image.enabled)
            {
            enabledObjects++;
            }
        }
        Assert.AreEqual(enabledObjects, playerMovement.health);

    }
}
