using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class looktomove1 : MonoBehaviour {
    //자동이동
    //auto moving , look to move

    public Camera Camera;
    int speed = 2;
    int sspeed = 0;
    public GameObject ground;


    // Use this for initialization

    void Start()
    {
       
    }
       
    // Update is called once per frame

    void Update()
    {
        moveObject();


    }


void moveObject()

    {
        Camera.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //Camera.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
    }

    public void stop() {

        //제스쳐로 이동멈춤을 위한 스탑함수
        // stop function 

        if (speed > 1)
        {
            speed = 0;
        }
    }
    public void moveagain() {
        

        // 제스쳐로 이동을 위한 이동함수
        // move fuction
        speed = 2;
        Camera.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //Camera.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
    }

}
