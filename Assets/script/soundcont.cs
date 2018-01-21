using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundcont : MonoBehaviour {

    //사운드컨트롤 분수 소리 조절용 함수
    //sound controll

    public AudioSource ad;

    public void Stop() {

        ad.Stop();
        
    }

    public void Startagain() {
        ad.Play();
    }
    
}
