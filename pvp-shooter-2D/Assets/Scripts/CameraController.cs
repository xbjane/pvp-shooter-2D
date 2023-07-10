using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransf;
    private Vector3 deltaPos;// Start is called before the first frame update
    void Start()
    {
        //playerPos
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaPos = transform.position - playerTransf.position;
        transform.position = Vector3.Lerp(playerTransf.position, deltaPos, Time.deltaTime);
        //if (Input.touchCount > 0)
        //{
        //    Input.
        //}
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    // Update the Text on the screen depending on current position of the touch each frame
        //    Debug.Log( "Touch Position : " + touch.position);
        //    touch.azimuthAngle = 90;
        //}
        //else
        //{
        //    Debug.Log("No touch contacts");
        //}
    }
}
