using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    public float velocidad = 2f;
    public float altura = 1f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidad) * altura;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }
}
