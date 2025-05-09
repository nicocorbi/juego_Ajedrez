using UnityEngine;
using UnityEngine.UI;

public class barraTiempo : MonoBehaviour
{
    [SerializeField] public Image icon; 
    [SerializeField] private TiempoManager tiempoManager; 
    [SerializeField] Color barraTurno1 = Color.red; 
    [SerializeField] Color barraTurno2 = Color.blue; 

    private bool esTurno1 = true; 

    void Start()
    {
              
        tiempoManager.OnCambioTurno += CambiarTurno;
        icon.color = barraTurno1;
    }

    void Update()
    {      
            icon.fillAmount = tiempoManager.TiempoRestante();       
    }

    private void CambiarTurno()
    {       
        esTurno1 = !esTurno1;
        icon.color = esTurno1 ? barraTurno1 : barraTurno2;
    }

    void OnDestroy()
    {     
       tiempoManager.OnCambioTurno -= CambiarTurno;
    }
}