using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //hold the specified positions of the specified objects
    private Vector3 InitialCameraPosition, InitilalMousePosition,CurrentMousePosition;

    //hold the max and min positions in space(should be opposite corners of a bounding Rectangular prism
    public Vector3 MaxPosition = new Vector3(200,200,200) , MinPosition = new Vector3(0, 1, 0);

    //Scalling for zoom speed
    public float zoomSpeed = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //when middle mouse is first pressed save the initial positions
        if (Input.GetMouseButtonDown(2))
        {
            InitialCameraPosition = gameObject.transform.position;
            //switch y and z since I am moving perpendicular to the camera
            InitilalMousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.z, Input.mousePosition.y);
            
        }
        //while the middle mouse is held down move the camera around
        if (Input.GetMouseButton(2))
        {
            //switch y and z since I am moving perpendicular to the camera
            CurrentMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.z, Input.mousePosition.y);
            //set the cameras position to its initial position plus the movement from the mouse
            gameObject.transform.position = InitialCameraPosition + (InitilalMousePosition - CurrentMousePosition);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            //move the camera based on the scroll
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 0);

        }
        //keep the camera within the max and min bounds
        if (gameObject.transform.position.x > MaxPosition.x)
        {
            gameObject.transform.position = new Vector3(MaxPosition.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.y > MaxPosition.y)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, MaxPosition.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.z > MaxPosition.z)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MaxPosition.z);
        }
        if (gameObject.transform.position.x < MinPosition.x)
        {
            gameObject.transform.position = new Vector3(MinPosition.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.y < MinPosition.y)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, MinPosition.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.z < MinPosition.z)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MinPosition.z);
        }
    }
}
