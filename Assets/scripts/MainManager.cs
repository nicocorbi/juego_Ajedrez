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
    [SerializeField] Color cursorColor = Color.red;
    [SerializeField] Color cursorColorTurno2 = Color.blue;
    [SerializeField] TiempoManager tiempoManager; 

    private BoardSquare[,] Board;
    private Cursor cursor;
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

        // Iniciar el cursor
        cursor = new Cursor(Board, cursorVisual, cursorColor);
        tiempoManager.OnCambioTurno += CambiarTurno;     
    }

    private void Update()
    {
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
        // Alternar el turno
        esTurnoJugador1 = !esTurnoJugador1;

        // Cambiar el color del cursor según el turno
        Color nuevoColor = esTurnoJugador1 ? cursorColor : cursorColorTurno2;
        cursor.CambiarColor(nuevoColor);
    }

    private void OnDestroy()
    {     
      tiempoManager.OnCambioTurno -= CambiarTurno;      
    }
}





