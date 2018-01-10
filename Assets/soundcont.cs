using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundcont : MonoBehaviour {

    public AudioSource ad;

    public void Stop() {

        ad.Stop();
        
    }

    public void Startagain() {
        ad.Play();
    }
    
}
