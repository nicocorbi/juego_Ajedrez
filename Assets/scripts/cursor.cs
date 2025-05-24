using UnityEngine;

public class Cursor
{
    private BoardSquare[,] tablero;
    private int filas;
    private int columnas;

    private int filaActual = 0;
    private int columnaActual = 0;

    private BoardSquare casillaAnterior;
    private GameObject cursorVisual;

    private Color highlightColor;
    private Color colorTurno;
    private float errorTimer = 0f;
    private float errorDuration = 0f;
    private Color errorColor;

    private bool sosteniendoPieza = false;

    public Cursor(BoardSquare[,] tablero, GameObject cursorVisual, Color highlightColor)
    {
        this.tablero = tablero;
        this.filas = tablero.GetLength(0);
        this.columnas = tablero.GetLength(1);
        this.cursorVisual = cursorVisual;
        this.highlightColor = highlightColor;
        this.colorTurno = highlightColor;

        casillaAnterior = tablero[filaActual, columnaActual];
        casillaAnterior.SetColorVisual(highlightColor);
        cursorVisual.transform.position = casillaAnterior.visual.transform.position;
    }

    public void SetSosteniendoPieza(bool valor)
    {
        sosteniendoPieza = valor;
    }

    public void Mover(int nuevaFila, int nuevaColumna)
    {
        
        nuevaFila = (nuevaFila + filas) % filas;
        nuevaColumna = (nuevaColumna + columnas) % columnas;

        if (nuevaFila != filaActual || nuevaColumna != columnaActual)
        {
            casillaAnterior.ResetColor();
            filaActual = nuevaFila;
            columnaActual = nuevaColumna;
            casillaAnterior = tablero[filaActual, columnaActual];
            casillaAnterior.SetColorVisual(highlightColor);
            cursorVisual.transform.position = casillaAnterior.visual.transform.position;
        }
    }

    public void CambiarColor(Color nuevoColor)
    {
        highlightColor = nuevoColor;
        colorTurno = nuevoColor;
        casillaAnterior.SetColorVisual(highlightColor);
        errorTimer = 0f;
    }

    public void CambiarColorTemporal(Color colorTemporal, float duracion)
    {
        errorColor = colorTemporal;
        errorDuration = duracion;
        errorTimer = duracion;
        casillaAnterior.SetColorVisual(errorColor);
    }

    public void Update()
    {
        if (errorTimer > 0f)
        {
            errorTimer -= Time.deltaTime;
            if (errorTimer <= 0f)
            {
                casillaAnterior.SetColorVisual(colorTurno);
                highlightColor = colorTurno;
            }
        }
    }

    public int GetFilaActual() => filaActual;
    public int GetColumnaActual() => columnaActual;
    public bool EstaSosteniendoPieza() => sosteniendoPieza;
}









