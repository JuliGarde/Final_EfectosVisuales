using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaLuces : MonoBehaviour
{
    public Light flickerLight;                   
    public Renderer cubeRenderer;                
    public Color emissionColor = Color.white;    
    public float minEmissionIntensity = 0.2f;    
    public float maxEmissionIntensity = 1.5f;    
    public float minFlickerTime = 0.05f;         
    public float maxFlickerTime = 0.3f;          

    private Material cubeMaterial;
    private float timer;
    private bool isLightOn = true;

    void Start()
    {
        cubeMaterial = cubeRenderer.material;   
        SetRandomFlicker();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
           
            isLightOn = !isLightOn;

           
            flickerLight.enabled = isLightOn;
            float emissionIntensity = isLightOn ? Random.Range(minEmissionIntensity, maxEmissionIntensity) : 0f;
            cubeMaterial.SetColor("_EmissionColor", emissionColor * emissionIntensity);
            DynamicGI.SetEmissive(cubeRenderer, emissionColor * emissionIntensity);  

           
            SetRandomFlicker();
        }
    }

    private void SetRandomFlicker()
    {
        timer = Random.Range(minFlickerTime, maxFlickerTime);
    }
}
