using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject piezaPrefab;
    [SerializeField] private GameObject piezaPrefab2;
    [SerializeField] private GameObject cofrePrefab;
    [SerializeField] private GameObject buffParticlesPrefab1; // Prefab partículas para piezas tipo 1
    [SerializeField] private GameObject buffParticlesPrefab2; // Prefab partículas para piezas tipo 2
    [SerializeField] private int boardWidth = 5;
    [SerializeField] private int boardHeight = 5;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.black;
    [SerializeField] private GameObject cursorVisual;
    [SerializeField] private Color cursorColor = Color.red;
    [SerializeField] private Color cursorColorTurno2 = Color.blue;
    [SerializeField] private Color cursorColorError = Color.magenta;
    [SerializeField] public TiempoManager tiempoManager;
    [SerializeField] private int vidaInicial = 3;
    [SerializeField] private float escalaInicial = 1.5f;
    [SerializeField] private int vidaCofre = 2;
    [SerializeField] private float escalaCofre = 1.2f;
    [SerializeField] private TextMeshProUGUI ataquesRestantesText;

    public BoardSquare[,] Board;
    private Cursor cursor;
    private List<Pieza> piezasEnJuego = new List<Pieza>();
    private List<Cofre> cofresEnJuego = new List<Cofre>();
    private Pieza piezaSeleccionada;
    private Vector2Int? posicionOriginalSeleccionada = null;
    private bool esTurnoJugador1 = true;
    private bool cofreInstanciado = false;
    private int ataquesRestantesEnTurno = 2;

    private void Awake()
    {
        Board = new BoardSquare[boardWidth, boardHeight];
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector3 position = spawnPosition + new Vector3(i, 0, j);
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity);
                Color color = (i + j) % 2 == 0 ? color1 : color2;
                square.GetComponent<Renderer>().material.color = color;
                Board[i, j] = new BoardSquare(new Vector2Int(i, j), square, color);
            }
        }
        cursor = new Cursor(Board, cursorVisual, cursorColor);
        tiempoManager.enabled = true;
        tiempoManager.OnCambioTurno += CambiarTurno;
        tiempoManager.CambiarColorBarra(esTurnoJugador1, cursorColor, cursorColorTurno2);

        ataquesRestantesEnTurno = 2;
        ActualizarTextoAtaquesRestantes();

        // Instanciar el cofre solo una vez al iniciar la partida
        if (!cofreInstanciado && cofrePrefab != null)
        {
            SpawnearCofreEnCentro(cofrePrefab);
            cofreInstanciado = true;
        }
    }

    private void Update()
    {
        cursor.Update();

        int nuevaFila = cursor.GetFilaActual();
        int nuevaColumna = cursor.GetColumnaActual();
        bool cursorMovido = false;

        if (GetKeyDownTurno(KeyCode.W, KeyCode.UpArrow)) { nuevaColumna--; cursorMovido = true; }
        if (GetKeyDownTurno(KeyCode.S, KeyCode.DownArrow)) { nuevaColumna++; cursorMovido = true; }
        if (GetKeyDownTurno(KeyCode.A, KeyCode.LeftArrow)) { nuevaFila++; cursorMovido = true; }
        if (GetKeyDownTurno(KeyCode.D, KeyCode.RightArrow)) { nuevaFila--; cursorMovido = true; }

        cursor.Mover(nuevaFila, nuevaColumna);

        if (cursorMovido)
            cursor.CambiarColor(esTurnoJugador1 ? cursorColor : cursorColorTurno2);

        if (piezaSeleccionada != null)
        {
            Vector2Int posCursor = new Vector2Int(cursor.GetFilaActual(), cursor.GetColumnaActual());
            piezaSeleccionada.pieza.transform.position = Board[posCursor.x, posCursor.y].visual.transform.position;
        }

        // Movimiento
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int posicionCursor = new Vector2Int(cursor.GetFilaActual(), cursor.GetColumnaActual());
            if (piezaSeleccionada == null)
            {
                SeleccionarPieza(posicionCursor);
            }
            else
            {
                int dx = Mathf.Abs(posicionCursor.x - piezaSeleccionada.posicionActual.x);
                int dy = Mathf.Abs(posicionCursor.y - piezaSeleccionada.posicionActual.y);
                bool esAdyacente = (dx <= 1 && dy <= 1) && (dx + dy > 0);

                if (esAdyacente)
                {
                    Color colorDestino = Board[posicionCursor.x, posicionCursor.y].GetLogicalColor();
                    if (ColoresIguales(colorDestino, piezaSeleccionada.colorInicial))
                    {
                        // No permite mover si hay una pieza rival o aliada en la casilla destino
                        bool hayPieza = false;
                        foreach (var pieza in piezasEnJuego)
                        {
                            if (pieza.posicionActual == posicionCursor)
                            {
                                hayPieza = true;
                                break;
                            }
                        }
                        // No permite mover si hay un cofre en la casilla destino
                        foreach (var cofre in cofresEnJuego)
                        {
                            if (cofre.posicionActual == posicionCursor)
                            {
                                hayPieza = true;
                                break;
                            }
                        }
                        if (!hayPieza)
                        {
                            piezaSeleccionada.posicionActual = posicionCursor;

                            // Ataca automáticamente si hay enemigo o cofre adyacente
                            AtacarAdyacente(piezaSeleccionada);

                            piezaSeleccionada = null;
                            posicionOriginalSeleccionada = null;
                            cursor.CambiarColor(esTurnoJugador1 ? cursorColor : cursorColorTurno2);
                        }
                        else
                        {
                            cursor.CambiarColorTemporal(cursorColorError, 1.0f);
                        }
                    }
                    else
                    {
                        cursor.CambiarColorTemporal(cursorColorError, 1.0f);
                    }
                }
                else
                {
                    cursor.CambiarColorTemporal(cursorColorError, 1.0f);
                }
            }
        }

        // Cambiar de turno solo cuando el jugador pulse Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CambiarTurno();
        }

        piezasEnJuego.RemoveAll(p => p.VidaActual <= 0);
        cofresEnJuego.RemoveAll(c => c.VidaActual <= 0);
    }

    private void AtacarAdyacente(Pieza atacante)
    {
        if (ataquesRestantesEnTurno <= 0)
            return;

        Vector2Int[] direcciones = {
            new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
            new Vector2Int(0, -1),                    new Vector2Int(0, 1),
            new Vector2Int(1, -1),  new Vector2Int(1, 0),  new Vector2Int(1, 1)
        };

        foreach (var dir in direcciones)
        {
            Vector2Int posObjetivo = atacante.posicionActual + dir;
            if (posObjetivo.x >= 0 && posObjetivo.x < boardWidth && posObjetivo.y >= 0 && posObjetivo.y < boardHeight)
            {
                // Ataca piezas rivales
                foreach (var pieza in piezasEnJuego)
                {
                    if (pieza.posicionActual == posObjetivo && pieza.esJugador1 != atacante.esJugador1)
                    {
                        int danioFinal = atacante.CalcularDanio(1);
                        pieza.RecibirDanio(danioFinal);
                        ataquesRestantesEnTurno--;
                        ActualizarTextoAtaquesRestantes();
                        return;
                    }
                }
                // Ataca cofres
                foreach (var cofre in cofresEnJuego)
                {
                    if (cofre.posicionActual == posObjetivo)
                    {
                        cofre.RecibirDanio(1);
                        ataquesRestantesEnTurno--;
                        ActualizarTextoAtaquesRestantes();
                        return;
                    }
                }
            }
        }
    }

    private void ActualizarTextoAtaquesRestantes()
    {
        if (ataquesRestantesText != null)
            ataquesRestantesText.text = $"Ataques restantes: {ataquesRestantesEnTurno}";
    }

    private bool GetKeyDownTurno(KeyCode keyJugador1, KeyCode keyJugador2)
    {
        return esTurnoJugador1 ? Input.GetKeyDown(keyJugador1) : Input.GetKeyDown(keyJugador2);
    }

    private void SeleccionarPieza(Vector2Int posicion)
    {
        foreach (Pieza pieza in piezasEnJuego)
        {
            if (pieza.posicionActual == posicion)
            {
                if ((esTurnoJugador1 && pieza.esJugador1) || (!esTurnoJugador1 && !pieza.esJugador1))
                {
                    piezaSeleccionada = pieza;
                    posicionOriginalSeleccionada = pieza.posicionActual;
                }
                else
                {
                    cursor.CambiarColorTemporal(cursorColorError, 1.0f);
                }
                return;
            }
        }
    }

    private void CambiarTurno()
    {
        esTurnoJugador1 = !esTurnoJugador1;
        ataquesRestantesEnTurno = 2;
        ActualizarTextoAtaquesRestantes();
        cursor.CambiarColor(esTurnoJugador1 ? cursorColor : cursorColorTurno2);

        if (piezaSeleccionada != null && posicionOriginalSeleccionada.HasValue)
        {
            piezaSeleccionada.posicionActual = posicionOriginalSeleccionada.Value;
            piezaSeleccionada.pieza.transform.position = Board[posicionOriginalSeleccionada.Value.x, posicionOriginalSeleccionada.Value.y].visual.transform.position;
            piezaSeleccionada = null;
            posicionOriginalSeleccionada = null;
        }

        if (tiempoManager != null)
        {
            tiempoManager.ReiniciarTiempo();
            tiempoManager.CambiarColorBarra(esTurnoJugador1, cursorColor, cursorColorTurno2);
        }
    }

    private void OnDestroy()
    {
        if (tiempoManager != null)
            tiempoManager.OnCambioTurno -= CambiarTurno;
    }

    public void ColocarPiezasIniciales()
    {
        for (int col = 1; col <= 3; col++)
        {
            var colorCasilla = Board[col, 0].GetLogicalColor();
            GameObject piezaGO = Instantiate(piezaPrefab, Board[col, 0].visual.transform.position, Quaternion.identity);

            ParticleSystem buffParticles = null;
            if (buffParticlesPrefab1 != null)
            {
                GameObject particlesGO = Instantiate(buffParticlesPrefab1, piezaGO.transform);
                particlesGO.transform.localPosition = Vector3.zero;
                buffParticles = particlesGO.GetComponent<ParticleSystem>();
            }

            piezasEnJuego.Add(new Pieza(
                piezaGO,
                new Vector2Int(col, 0),
                false,
                colorCasilla,
                vidaInicial,
                escalaInicial,
                buffParticles
            ));
        }
        int lastRow = boardHeight - 1;
        for (int col = 1; col <= 3; col++)
        {
            var colorCasilla = Board[col, lastRow].GetLogicalColor();
            GameObject piezaGO = Instantiate(piezaPrefab2, Board[col, lastRow].visual.transform.position, Quaternion.identity);

            ParticleSystem buffParticles = null;
            if (buffParticlesPrefab2 != null)
            {
                GameObject particlesGO = Instantiate(buffParticlesPrefab2, piezaGO.transform);
                particlesGO.transform.localPosition = Vector3.zero;
                buffParticles = particlesGO.GetComponent<ParticleSystem>();
            }

            piezasEnJuego.Add(new Pieza(
                piezaGO,
                new Vector2Int(col, lastRow),
                true,
                colorCasilla,
                vidaInicial,
                escalaInicial,
                buffParticles
            ));
        }
    }

    public bool HayPiezaEnCasilla(Vector2Int casilla)
    {
        foreach (var pieza in piezasEnJuego)
        {
            if (pieza.posicionActual == casilla)
                return true;
        }
        return false;
    }
    public List<Pieza> GetPiezasEnJuego()
    {
        return piezasEnJuego;
    }

    public List<Cofre> GetCofresEnJuego()
    {
        return cofresEnJuego;
    }
    public Pieza GetPiezaEnCasilla(Vector2Int casilla)
    {
        foreach (var pieza in piezasEnJuego)
        {
            if (pieza.posicionActual == casilla)
                return pieza;
        }
        return null;
    }

    public List<Vector2Int> ObtenerPosicionesInicialesFichasJugador1()
    {
        var posiciones = new List<Vector2Int>();
        for (int col = 1; col <= 3; col++)
            posiciones.Add(new Vector2Int(col, 0));
        return posiciones;
    }

    public List<Vector2Int> ObtenerPosicionesInicialesFichasJugador2()
    {
        var posiciones = new List<Vector2Int>();
        int lastRow = boardHeight - 1;
        for (int col = 1; col <= 3; col++)
            posiciones.Add(new Vector2Int(col, lastRow));
        return posiciones;
    }

    public List<Vector2Int> ObtenerPosicionesInicialesCofre()
    {
        var posiciones = new List<Vector2Int>();
        int centroX = boardWidth / 2;
        int centroY = boardHeight / 2;
        posiciones.Add(new Vector2Int(centroX, centroY));
        return posiciones;
    }

    public void SpawnearCofreEnCentro(GameObject prefabCofre)
    {
        var posiciones = ObtenerPosicionesInicialesCofre();
        if (posiciones.Count > 0)
        {
            Vector2Int pos = posiciones[0];
            Vector3 spawnPos = Board[pos.x, pos.y].visual.transform.position;
            GameObject cofreGO = Instantiate(prefabCofre, spawnPos, Quaternion.identity);
            cofresEnJuego.Add(new Cofre(cofreGO, pos, Board[pos.x, pos.y].GetLogicalColor(), vidaCofre, escalaCofre));
        }
    }

    private bool ColoresIguales(Color a, Color b, float tolerancia = 0.01f)
    {
        return Vector3.Distance(new Vector3(a.r, a.g, a.b), new Vector3(b.r, b.g, b.b)) < tolerancia;
    }
}
