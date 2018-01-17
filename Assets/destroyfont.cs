using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyfont : MonoBehaviour {

    //띄운 파티클 나중에 없앰
    // controll particle

    public ParticleSystem particle;

    public void pariclestop() {

        particle = gameObject.GetComponent<ParticleSystem>();
        particle.enableEmission = false;
    }

    public void particlestart() {

        particle = gameObject.GetComponent<ParticleSystem>();
        particle.enableEmission = true;

    }
}
