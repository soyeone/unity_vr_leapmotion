using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Pathvr : MonoBehaviour {

    public float keySpeed = 10.0f;
    public float mouseSpeed = 1.25f;
    public GameObject eye;

    void Update()
    {
        

        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        Look(new Vector3(dx, dy, 0.0f) * mouseSpeed);
    }

    
    void Strafe(float dist)
    {
        transform.position += eye.transform.right * dist;
    }

    void Fly(float dist)
    {
        transform.position += eye.transform.forward * dist;
    }

    public void Look(Vector3 dist)
    {
        Vector3 angles = transform.eulerAngles;
        angles += new Vector3(-dist.y, dist.x, dist.z);
        transform.eulerAngles = new Vector3(ClampAngle(angles.x), angles.y, angles.z);
    }

    public void stop() {
        
    }

    public void moveagain() {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        Look(new Vector3(dx, dy, 0.0f) * mouseSpeed);
    }

    public float ClampAngle(float angle)
    {
        if (angle< 180f)
		{
            if (angle > 80f) angle = 80f;
        }
		else
		{
            if (angle < 280f) angle = 280f;
        }
        return angle;
    }
}
