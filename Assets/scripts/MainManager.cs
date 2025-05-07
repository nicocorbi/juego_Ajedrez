using UnityEngine;

public class MainManagerPruebas : MonoBehaviour
{
    [SerializeField] GameObject squarePrefab;
    [SerializeField] int boardWidth = 5;
    [SerializeField] int boardHeight = 5;
    [SerializeField] Vector3 spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] Color color1 = Color.white;
    [SerializeField] Color color2 = Color.black;
    [SerializeField] GameObject cursorVisual;
    [SerializeField] Color cursorHighlightColor = Color.red;
    [SerializeField] Color cursorHighlightColorTurno2 = Color.blue; 
    [SerializeField] float turnoDuracion = 10f;

    private BoardSquare[,] Board;
    private Cursor cursor;
    private float tiempoTurnoActual = 0f; 
    private bool esTurnoJugador1 = true; 

    private void Awake()
    {
        // Inicializar el tablero
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

        // Inicializar el cursor
        cursor = new Cursor(Board, cursorVisual, cursorHighlightColor);
    }

    private void Update()
    {
        
        tiempoTurnoActual += Time.deltaTime;

        // Cambiar de turno si el tiempo excede la duración del turno
        if (tiempoTurnoActual >= turnoDuracion)
        {
            CambiarTurno();
            tiempoTurnoActual = 0f; 
        }

        // Movimiento del cursor
        int nuevaFila = cursor.GetFilaActual();
        int nuevaColumna = cursor.GetColumnaActual();

        if (Input.GetKeyDown(KeyCode.W)) nuevaColumna--;
        if (Input.GetKeyDown(KeyCode.S)) nuevaColumna++;
        if (Input.GetKeyDown(KeyCode.A)) nuevaFila++;
        if (Input.GetKeyDown(KeyCode.D)) nuevaFila--;

        cursor.Mover(nuevaFila, nuevaColumna);
    }

    private void CambiarTurno()
    {
        
        esTurnoJugador1 = !esTurnoJugador1;

        // Cambiar el color del cursor según el turno
        Color nuevoColor = esTurnoJugador1 ? cursorHighlightColor : cursorHighlightColorTurno2;
        cursor.CambiarColor(nuevoColor);
    }
}





