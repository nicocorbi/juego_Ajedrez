using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicaInicio : MonoBehaviour
{
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private Vector3 posicionInicial = new Vector3(2.5f, 10f, -10f);
    [SerializeField] private Vector3 posicionFinal = new Vector3(2.5f, 8f, 2.5f);
    [SerializeField] private Quaternion rotacionInicial = Quaternion.Euler(60f, 0f, 0f);
    [SerializeField] private Quaternion rotacionFinal = Quaternion.Euler(45f, 0f, 0f);
    [SerializeField] private float duracionCinematica = 2.5f;
    [SerializeField] private TMP_Text contadorTexto;
    [SerializeField] private MainManagerPruebas mainManager;
    [SerializeField] AnimationCurve curva;

    private void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        StartCoroutine(CinematicaYContador());
    }

    private IEnumerator CinematicaYContador()
    {
        mainManager.enabled = false;

        float tiempoRestante = 3f; // Duración del contador
        float t = 0f;
        camaraPrincipal.transform.position = posicionInicial;
        camaraPrincipal.transform.rotation = rotacionInicial;
        contadorTexto.enabled = true;

        while (tiempoRestante > 0f)
        {
            // Movimiento de cámara interpolado
            float lerpT = Mathf.Clamp01(t / duracionCinematica);
            camaraPrincipal.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, lerpT);
            camaraPrincipal.transform.rotation = Quaternion.Lerp(rotacionInicial, rotacionFinal, lerpT);

            // Actualiza el contador (redondeado hacia arriba)
            contadorTexto.text = Mathf.CeilToInt(tiempoRestante).ToString();

            t += Time.deltaTime;
            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        contadorTexto.enabled = false;
        mainManager.enabled = true;
    }
}