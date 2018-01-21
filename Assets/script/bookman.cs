using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bookman : MonoBehaviour {

    //패널 띄우고 끄는 함수
    // show panel / hide panel

    public GameObject Pannel;
    public GameObject book;
        
    public void hidepan()
    {
        
            Pannel.gameObject.SetActive(false);
        
    }
    public void openpan()
    {
        Pannel.gameObject.SetActive(true);
    }
}
