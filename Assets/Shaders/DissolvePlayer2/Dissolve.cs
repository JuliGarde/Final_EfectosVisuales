using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public Material mat;
    public float speed = 1f;

    void Update()
    {
        float v = Mathf.PingPong(Time.time * speed, 1f);
        mat.SetFloat("_DissolveAmount", v);
    }
}
