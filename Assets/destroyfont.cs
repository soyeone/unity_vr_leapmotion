using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyfont : MonoBehaviour {

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
