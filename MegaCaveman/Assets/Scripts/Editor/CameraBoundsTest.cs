using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CameraBoundsTest {

	
	
	[UnityTest]
	public IEnumerator CameraBoundsTestWithEnumeratorPasses() {

        //create dummy objects and set to dummy values

        GameObject tempGameObject = new GameObject();
        RoomCamera roomCamera = tempGameObject.AddComponent<RoomCamera>();

        GameObject areaGameObject = new GameObject();
        BoxCollider2D boxCollider2D = areaGameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(Camera.main.orthographicSize, Camera.main.orthographicSize * Camera.main.aspect);
        roomCamera.Area = boxCollider2D;

        //call the function that assigns the camera constraints to appropriate values for the passed in area

        roomCamera.UpdateBounds(roomCamera.Area);
        yield return null;

        //ensure that (minX, minY, maxX, ,maxY) never exceed their bounds.

        Assert.GreaterOrEqual(roomCamera.minX, boxCollider2D.bounds.min.x + (Camera.main.orthographicSize * Camera.main.aspect));
        //minX = Area.bounds.min.x + cameraWidth;
        Assert.LessOrEqual(roomCamera.maxX, boxCollider2D.bounds.max.x - (Camera.main.orthographicSize * Camera.main.aspect));
        //maxX = Area.bounds.max.x - cameraWidth;
        Assert.GreaterOrEqual(roomCamera.minY, boxCollider2D.bounds.min.y + Camera.main.orthographicSize);
        //minY = Area.bounds.min.y + cameraHeight;
        Assert.LessOrEqual(roomCamera.maxY, boxCollider2D.bounds.max.y - Camera.main.orthographicSize);
        // maxY = Area.bounds.max.y - cameraHeight;

        
        

    }
}
