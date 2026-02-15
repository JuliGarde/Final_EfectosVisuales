using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeParticulas : MonoBehaviour
{
    
    public ParticleSystem particulas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            particulas.Clear();   
            particulas.Play();    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}


