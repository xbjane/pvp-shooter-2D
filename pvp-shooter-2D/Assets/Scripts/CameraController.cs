using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform playerTransf;
        private Vector3 velocity;
        private Vector3 targetPos;
        //void Start()
        //{
        //    velocity = Vector3.zero;
        //}

        //// Update is called once per frame
        //void Update()
        //{
        //    targetPos = playerTransf.position;
        //    targetPos.z = -10f;
        //    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, Time.deltaTime);
        //    //transform.LookAt(playerTransf);

        //    if (Input.touchCount > 0)
        //    {
        //        Touch touch = Input.GetTouch(0);

        //        // Update the Text on the screen depending on current position of the touch each frame
        //        Debug.Log("Touch Position : " + touch.position);
        //        // touch.azimuthAngle = 90;
        //    }
        //    else
        //    {
        //        Debug.Log("No touch contacts");
        //    }
        //}
    }
}

