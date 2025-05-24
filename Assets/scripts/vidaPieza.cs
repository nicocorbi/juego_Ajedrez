using UnityEngine;
using System;

public class VidaPieza : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private float escalaInicial = 1.5f;
    public bool escalaFija = false;
    public float valorEscalaFija = 0.2f;

    private int vidaActual;
    private float escalaBase;

    public event Action OnMuerte;

    private void Awake()
    {
        vidaActual = vidaMaxima;
        escalaBase = escalaInicial;
        ActualizarEscala();
    }

    public void Inicializar(int vida, float escala)
    {
        vidaMaxima = vida;
        escalaInicial = escala;
        vidaActual = vidaMaxima;
        escalaBase = escalaInicial;
        ActualizarEscala();
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;
        ActualizarEscala();
        if (vidaActual <= 0)
        {
            if (OnMuerte != null)
                OnMuerte.Invoke();
            Destroy(gameObject);
        }
    }

    public int GetVidaActual() => vidaActual;
    public int GetVidaMaxima() => vidaMaxima;

    private void ActualizarEscala()
    {
        if (escalaFija)
        {
            transform.localScale = new Vector3(valorEscalaFija, valorEscalaFija, valorEscalaFija);
        }
        else
        {
            float escala = Mathf.Max(0.5f, escalaBase * ((float)vidaActual / vidaMaxima));
            transform.localScale = new Vector3(escala, escala, escala);
        }
    }
}



