using UnityEngine;

public class MoverCursor : MonoBehaviour
{
    
    public GameObject[] casillas;
    public GameObject cursor; 
    public int filas = 8; 
    public int columnas = 8; 

    private int filaActual = 0; 
    private int columnaActual = 0; 

    void Start()
    {
        
        cursor.transform.position = casillas[filaActual * columnas + columnaActual].transform.position;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.DownArrow) && filaActual > 0)
        {
            filaActual--; 
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && filaActual < filas - 1)
        {
            filaActual++; 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && columnaActual > 0)
        {
            columnaActual--; 
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && columnaActual < columnas - 1)
        {
            columnaActual++; 
        }

        
        cursor.transform.position = casillas[filaActual * columnas + columnaActual].transform.position;
    }
}

