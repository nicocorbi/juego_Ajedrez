using UnityEngine;
using System.Collections.Generic;

public class MainManagerPruebas : MonoBehaviour
{
    [SerializeField] GameObject squarePrefab;
    [SerializeField] GameObject piezaPrefab;
    [SerializeField] GameObject piezaPrefab2;

    [SerializeField] int boardWidth = 5;
    [SerializeField] int boardHeight = 5;
    [SerializeField] Vector3 spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] Color color1 = Color.white;
    [SerializeField] Color color2 = Color.black;
    [SerializeField] GameObject cursorVisual;
    [SerializeField] Color cursorColor = Color.red;
    [SerializeField] Color cursorColorTurno2 = Color.blue;
    [SerializeField] TiempoManager tiempoManager;

    private BoardSquare[,] Board;
    private Cursor cursor;
    private List<Pieza> piezasEnJuego = new List<Pieza>();
    private Pieza piezaSeleccionada;
    private bool esTurnoJugador1 = true;
    private void Awake()
    {
        // Iniciar el tablero
        Board = new BoardSquare[boardWidth, boardHeight];
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector3 position = spawnPosition + new Vector3(i, 0, j);
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity);
                Color color = (i + j) % 2 == 0 ? color1 : color2;
                square.GetComponent<Renderer>().material.color = color;

                Board[i, j] = new BoardSquare(new Vector2Int(i, j), square);
            }
        }

        
        cursor = new Cursor(Board, cursorVisual, cursorColor);

       
        ColocarPiezasIniciales();

        
        tiempoManager.OnCambioTurno += CambiarTurno;
    }

    private void Update()
    {
        // Movimiento del cursor
        int nuevaFila = cursor.GetFilaActual();
        int nuevaColumna = cursor.GetColumnaActual();

        if (esTurnoJugador1)
        {
            if (Input.GetKeyDown(KeyCode.W)) nuevaColumna--;
            if (Input.GetKeyDown(KeyCode.S)) nuevaColumna++;
            if (Input.GetKeyDown(KeyCode.A)) nuevaFila++;
            if (Input.GetKeyDown(KeyCode.D)) nuevaFila--;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) nuevaColumna--;
            if (Input.GetKeyDown(KeyCode.DownArrow)) nuevaColumna++;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) nuevaFila++;
            if (Input.GetKeyDown(KeyCode.RightArrow)) nuevaFila--;
        }

        cursor.Mover(nuevaFila, nuevaColumna);

        // movimiento pieza junto al cursor 
        if (piezaSeleccionada != null)
        {
            Vector2Int posCursor = new Vector2Int(cursor.GetFilaActual(), cursor.GetColumnaActual());
            BoardSquare casillaCursor = Board[posCursor.x, posCursor.y];
            piezaSeleccionada.pieza.transform.position = casillaCursor.visual.transform.position;
                      
        }

        // Seleccionar o soltar pieza
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int posicionCursor = new Vector2Int(cursor.GetFilaActual(), cursor.GetColumnaActual());
            if (piezaSeleccionada == null)
            {
                SeleccionarPieza(posicionCursor);
            }
            else
            {
                //  actualiza la posición lógica de la pieza
                piezaSeleccionada.posicionActual = posicionCursor;
                piezaSeleccionada = null;
            }
        }
    }

    private void SeleccionarPieza(Vector2Int posicion)
    {
        foreach (Pieza pieza in piezasEnJuego)
        {
            if (pieza.posicionActual == posicion)
            {
                // Solo permite seleccionar si la pieza pertenece al jugador activo
                if ((esTurnoJugador1 && pieza.esJugador1) || (!esTurnoJugador1 && !pieza.esJugador1))
                {
                    piezaSeleccionada = pieza;
                    Debug.Log($"Pieza seleccionada en posición: {posicion}");
                }
                else
                {
                    Debug.Log("No puedes mover la pieza del otro jugador.");
                }
                return;
            }
        }
        Debug.Log("No hay ninguna pieza en esta posición.");
    }



    private void CambiarTurno()
    {
        esTurnoJugador1 = !esTurnoJugador1;
        Color nuevoColor = esTurnoJugador1 ? cursorColor : cursorColorTurno2;
        cursor.CambiarColor(nuevoColor);
    }

    private void OnDestroy()
    {
        tiempoManager.OnCambioTurno -= CambiarTurno;
    }

    private void ColocarPiezasIniciales()
    {
        // Coloca 3 piezas en la fila superior 
        for (int col = 1; col <= 3; col++)
        {
            piezasEnJuego.Add(
                new Pieza(
                    Instantiate(piezaPrefab, Board[col, 0].visual.transform.position, Quaternion.identity),
                    new Vector2Int(col, 0),
                    false 
                )
            );
        }

        // Coloca 3 piezas en la fila inferior 
        int lastRow = boardHeight - 1;
        for (int col = 1; col <= 3; col++)
        {
            piezasEnJuego.Add(
                new Pieza(
                    Instantiate(piezaPrefab2, Board[col, lastRow].visual.transform.position, Quaternion.identity),
                    new Vector2Int(col, lastRow),
                    true 
                )
            );
        }
    }
}





