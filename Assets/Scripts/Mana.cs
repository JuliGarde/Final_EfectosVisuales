using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [Header("Material del líquido")]
    [SerializeField] Renderer renderLiquido;

    [Header("Movimiento automático")]
    [SerializeField] bool rotacionAutomatica = true;
    [SerializeField] Vector3 amplitudRotacion = new Vector3(2f, 0f, 2f);
    [SerializeField] float velocidadRotacion = 1f;

    [Header("Comportamiento del líquido")]
    [SerializeField] float fuerzaTambaleo = 0.03f;
    [SerializeField] float recuperacion = 1.5f;
    [SerializeField] float velocidadOnda = 2f;

    Vector3 rotAnterior;
    float tambaleoX;
    float tambaleoZ;
    float objetivoX;
    float objetivoZ;
    float tiempo;
    Quaternion rotInicial;

    void Start()
    {
        if (renderLiquido == null)
            renderLiquido = GetComponent<Renderer>();

        rotInicial = transform.localRotation;
        rotAnterior = transform.eulerAngles;
    }

    void Update()
    {
        float dt = Application.isPlaying ? Time.deltaTime : Time.unscaledDeltaTime;
        if (dt <= 0) return;

        tiempo += dt;

        // Rotación automática
        if (rotacionAutomatica)
        {
            Quaternion rot = Quaternion.Euler(
                Mathf.Sin(tiempo * velocidadRotacion) * amplitudRotacion.x,
                Mathf.Sin(tiempo * velocidadRotacion * 0.7f) * amplitudRotacion.y,
                Mathf.Sin(tiempo * velocidadRotacion * 1.3f) * amplitudRotacion.z
            );

            transform.localRotation = rotInicial * rot;
        }

        // Detectar cambio de rotación
        Vector3 rotActual = transform.eulerAngles;

        float deltaX = Mathf.DeltaAngle(rotAnterior.x, rotActual.x);
        float deltaZ = Mathf.DeltaAngle(rotAnterior.z, rotActual.z);

        // Inercia del líquido
        objetivoX += Mathf.Clamp(deltaZ * fuerzaTambaleo, -fuerzaTambaleo, fuerzaTambaleo);
        objetivoZ += Mathf.Clamp(deltaX * fuerzaTambaleo, -fuerzaTambaleo, fuerzaTambaleo);

        // Recuperación
        objetivoX = Mathf.Lerp(objetivoX, 0, dt * recuperacion);
        objetivoZ = Mathf.Lerp(objetivoZ, 0, dt * recuperacion);

        // Onda
        float onda = Mathf.Sin(tiempo * velocidadOnda);
        tambaleoX = objetivoX * onda;
        tambaleoZ = objetivoZ * onda;

        // 👉 Enviar a tu shader (Mana1 / Mana2)
        if (renderLiquido != null && renderLiquido.sharedMaterial != null)
        {
            renderLiquido.sharedMaterial.SetFloat("Mana1", tambaleoX);
            renderLiquido.sharedMaterial.SetFloat("Mana2", tambaleoZ);
        }

        rotAnterior = rotActual;
    }

    public void Sacudir(float intensidad = 1f)
    {
        objetivoX += Random.Range(-fuerzaTambaleo, fuerzaTambaleo) * intensidad;
        objetivoZ += Random.Range(-fuerzaTambaleo, fuerzaTambaleo) * intensidad;
    }
}

