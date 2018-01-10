using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bookman : MonoBehaviour {

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
