using UnityEngine;
using UnityEngine.UI;

public class TiempoManager : MonoBehaviour
{
    [SerializeField] private float duracionTurno = 10f;
    private float tiempoActual;
    [SerializeField] private Image barraTurno; // Asigna la barra de tiempo en el inspector

    public delegate void CambioTurnoHandler();
    public event CambioTurnoHandler OnCambioTurno;

    private void Start()
    {
        tiempoActual = duracionTurno;
    }

    private void Update()
    {
        tiempoActual -= Time.deltaTime;
        if (tiempoActual < 0f)
        {
            tiempoActual = 0f;
            if (OnCambioTurno != null)
                OnCambioTurno.Invoke();
            ReiniciarTiempo();
        }
        ActualizarBarra();
    }

    public float TiempoRestante()
    {
        return tiempoActual;
    }

    public void ReiniciarTiempo()
    {
        tiempoActual = duracionTurno;
    }

    // Cambia el color de la barra de turno según el jugador activo
    public void CambiarColorBarra(bool esTurnoJugador1, Color colorJugador1, Color colorJugador2)
    {
        if (barraTurno != null)
            barraTurno.color = esTurnoJugador1 ? colorJugador1 : colorJugador2;
    }

    private void ActualizarBarra()
    {
        if (barraTurno != null)
            barraTurno.fillAmount = tiempoActual / duracionTurno;
    }
}

