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

    public Cursor(BoardSquare[,] tablero, GameObject cursorVisual, Color highlightColor)
    {
        this.tablero = tablero;
        this.filas = tablero.GetLength(0);
        this.columnas = tablero.GetLength(1);
        this.cursorVisual = cursorVisual;
        this.highlightColor = highlightColor;

        // Establecer la casilla inicial
        casillaAnterior = tablero[filaActual, columnaActual];
        casillaAnterior.SetColor(highlightColor);
        cursorVisual.transform.position = casillaAnterior.visual.transform.position;
    }

    public void Mover(int nuevaFila, int nuevaColumna)
    {
        
        if (nuevaFila < 0)
            nuevaFila = filas - 1; 
        else if (nuevaFila >= filas)
            nuevaFila = 0; 

        // Aplicar cambio de lado 
        if (nuevaColumna < 0)
            nuevaColumna = columnas - 1; 
        else if (nuevaColumna >= columnas)
            nuevaColumna = 0; 

        
        if (nuevaFila != filaActual || nuevaColumna != columnaActual)
        {
           
            casillaAnterior.ResetColor();

            // Actualizar posición
            filaActual = nuevaFila;
            columnaActual = nuevaColumna;
            casillaAnterior = tablero[filaActual, columnaActual];

            // Establecer el nuevo color y mover el cursor visual
            casillaAnterior.SetColor(highlightColor);
            cursorVisual.transform.position = casillaAnterior.visual.transform.position;
        }
    }
    public void CambiarColor(Color nuevoColor)
    {
        
        highlightColor = nuevoColor;
        casillaAnterior.SetColor(highlightColor);
    }

    public int GetFilaActual() => filaActual;
    public int GetColumnaActual() => columnaActual;
}




