using UnityEngine;
using System;

public class TiempoManager : MonoBehaviour
{
    [SerializeField] private float duracionTurno = 10f; 
    private float tiempoActual;
    public event Action OnCambioTurno;

    void Start()
    {
        tiempoActual = duracionTurno;
    }

    void Update()
    {
        
        tiempoActual -= Time.deltaTime;
        if (tiempoActual <= 0f)
        {
            tiempoActual = duracionTurno;
            OnCambioTurno?.Invoke(); 
        }
    }
    public float TiempoRestante()
    {
        return Mathf.Clamp01(tiempoActual / duracionTurno);
    }
}
