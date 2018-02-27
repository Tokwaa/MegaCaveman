using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour {

    public Collider2D Area;

    float minX, minY;
    float maxX, maxY;
    Vector2 bounds_Y;

    public Transform target;

    // Use this for initialization
    void Start () {
        UpdateBounds(Area);
	}
	
	

    void UpdateBounds(Collider2D Area)
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;



        minX = Area.bounds.min.x + cameraWidth;
        maxX = Area.bounds.max.x - cameraWidth;

        minY = Area.bounds.min.y +cameraHeight;
        maxY = Area.bounds.max.y - cameraHeight;

        //bounds_Max = Area.bounds.max;
        //bounds_Min = Area.bounds.min;
    }

    private void Update()
    {


        float targetPosX = Mathf.Clamp(target.transform.position.x, minX, maxX); ;
        //targetPosX = Mathf.Clamp(targetPosX, minX, maxX);

        //float targetPosY = target.position.y;
        float targetPosY = Mathf.Clamp(target.position.y, minY, maxY);

        Camera.main.transform.position = new Vector3(targetPosX, targetPosY, Camera.main.transform.position.z);

    }

}
