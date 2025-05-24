using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicaInicio : MonoBehaviour
{
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private TMP_Text contadorTexto;
    [SerializeField] private MainManager mainManager;
    [SerializeField] private Vector3 posicionInicial = Vector3.zero;
    [SerializeField] private Vector3 posicionFinal = Vector3.zero;
    [SerializeField] private Vector3 rotacionInicialEuler = Vector3.zero;
    [SerializeField] private Vector3 rotacionFinalEuler = Vector3.zero;
    [SerializeField] private float duracionCinematica = 2.5f;
    [SerializeField] private AnimationCurve curva = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        StartCoroutine(CinematicaYContador());
    }

    private IEnumerator CinematicaYContador()
    {
        // Opcional: detener partículas de buff en todas las piezas al iniciar la cinemática
        if (mainManager != null)
        {
            var piezas = mainManager.GetPiezasEnJuego();
            foreach (var pieza in piezas)
            {
                if (pieza.buffParticles != null)
                    pieza.buffParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        mainManager.enabled = false;

        float tiempoContador = 3f;
        float t = 0f;
        Quaternion rotacionInicial = Quaternion.Euler(rotacionInicialEuler);
        Quaternion rotacionFinal = Quaternion.Euler(rotacionFinalEuler);

        camaraPrincipal.transform.position = posicionInicial;
        camaraPrincipal.transform.rotation = rotacionInicial;
        contadorTexto.enabled = true;

        while (tiempoContador > 0f)
        {
            float lerpT = Mathf.Clamp01(t / duracionCinematica);
            float curvaT = curva != null ? curva.Evaluate(lerpT) : lerpT;

            camaraPrincipal.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, curvaT);
            camaraPrincipal.transform.rotation = Quaternion.Lerp(rotacionInicial, rotacionFinal, curvaT);

            contadorTexto.text = Mathf.CeilToInt(tiempoContador).ToString();

            t += Time.deltaTime;
            tiempoContador -= Time.deltaTime;
            yield return null;
        }

        contadorTexto.enabled = false;
        mainManager.enabled = true;
        }
    }




