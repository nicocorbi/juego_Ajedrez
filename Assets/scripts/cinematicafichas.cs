using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicaFichas : MonoBehaviour
{
    [Header("Prefabs de las fichas")]
    [SerializeField] private GameObject fichaPrefabBlanca;
    [SerializeField] private GameObject fichaPrefabNegra;

    [Header("Posiciones de la animación BLANCAS")]
    [SerializeField] private Transform pos1Blancas;
    [SerializeField] private Transform pos2Blancas;
    [SerializeField] private Transform pos3Blancas;

    [Header("Posiciones de la animación NEGRAS")]
    [SerializeField] private Transform pos1Negras;
    [SerializeField] private Transform pos2Negras;
    [SerializeField] private Transform pos3Negras;

    [Header("Tiempos")]
    [SerializeField] private float duracionPaso = 1.0f;
    [SerializeField] private float desfase = 0.5f;

    [Header("Número de fichas")]
    [SerializeField] private int cantidadFichasBlancas = 3;
    [SerializeField] private int cantidadFichasNegras = 3;

    [Header("Separación entre fichas")]
    [SerializeField] private float separacionBlancas = 0.5f;
    [SerializeField] private float separacionNegras = 0.5f;

    [Header("Referencias externas")]
    public MainManager mainManager; 

    private BoardSquare[,] Board;
    private List<Vector2Int> posicionesDestinoBlancas = new List<Vector2Int>();
    private List<Vector2Int> posicionesDestinoNegras = new List<Vector2Int>();

    private List<GameObject> fichasBlancas = new List<GameObject>();
    private List<GameObject> fichasNegras = new List<GameObject>();
    private List<Vector3> offsetsBlancas = new List<Vector3>();
    private List<Vector3> offsetsNegras = new List<Vector3>();

    private void Start()
    {
        if (mainManager != null)
        {
            Board = mainManager.Board;
            posicionesDestinoBlancas = mainManager.ObtenerPosicionesInicialesFichasJugador1();
            posicionesDestinoNegras = mainManager.ObtenerPosicionesInicialesFichasJugador2();
        }
        InstanciarFichas();
        StartCoroutine(AnimarFichas());
    }

    private void InstanciarFichas()
    {
        fichasBlancas.Clear();
        offsetsBlancas.Clear();
        fichasNegras.Clear();
        offsetsNegras.Clear();

        // Blancas
        Vector3 direccionBlancas = (pos2Blancas.position - pos1Blancas.position).normalized;
        for (int i = 0; i < cantidadFichasBlancas; i++)
        {
            Vector3 offset = -direccionBlancas * separacionBlancas * i;
            offsetsBlancas.Add(offset);
            GameObject ficha = Instantiate(fichaPrefabBlanca, pos1Blancas.position + offset, Quaternion.identity);
            fichasBlancas.Add(ficha);
        }

        // Negras
        Vector3 direccionNegras = (pos2Negras.position - pos1Negras.position).normalized;
        for (int i = 0; i < cantidadFichasNegras; i++)
        {
            Vector3 offset = -direccionNegras * separacionNegras * i;
            offsetsNegras.Add(offset);
            GameObject ficha = Instantiate(fichaPrefabNegra, pos1Negras.position + offset, Quaternion.identity);
            fichasNegras.Add(ficha);
        }
    }

    private IEnumerator AnimarFichas()
    {
        // Lanzar ambas animaciones en paralelo
        List<Coroutine> coroutines = new List<Coroutine>();

        for (int i = 0; i < fichasBlancas.Count; i++)
        {
            coroutines.Add(StartCoroutine(MoverFicha(
                fichasBlancas[i], offsetsBlancas[i], i,
                pos1Blancas.position, pos2Blancas.position, pos3Blancas.position, posicionesDestinoBlancas
            )));
            yield return new WaitForSeconds(desfase);
        }

        for (int i = 0; i < fichasNegras.Count; i++)
        {
            coroutines.Add(StartCoroutine(MoverFicha(
                fichasNegras[i], offsetsNegras[i], i,
                pos1Negras.position, pos2Negras.position, pos3Negras.position, posicionesDestinoNegras
            )));
            yield return new WaitForSeconds(desfase);
        }

        // Esperar a que terminen todas las animaciones
        foreach (var c in coroutines)
            yield return c;

        // Destruir las fichas de la cinemática
        foreach (var ficha in fichasBlancas)
            Destroy(ficha);
        foreach (var ficha in fichasNegras)
            Destroy(ficha);

      
        if (mainManager != null)
            mainManager.ColocarPiezasIniciales();
    }

    private IEnumerator MoverFicha(GameObject ficha, Vector3 offset, int indice,
        Vector3 pos1, Vector3 pos2, Vector3 pos3, List<Vector2Int> posicionesDestino)
    {
        // Movimiento de pos1 a pos2
        yield return StartCoroutine(MoverDeA_B(ficha, pos1 + offset, pos2 + offset));
        // Movimiento de pos2 a pos3
        yield return StartCoroutine(MoverDeA_B(ficha, pos2 + offset, pos3 + offset));
        // Movimiento de pos3 a la casilla real del tablero (interpolado)
        if (Board != null && posicionesDestino != null && indice < posicionesDestino.Count)
        {
            Vector2Int posDestino = posicionesDestino[indice];
            Vector3 destinoTablero = Board[posDestino.x, posDestino.y].visual.transform.position;
            yield return StartCoroutine(MoverDeA_B(ficha, pos3 + offset, destinoTablero));
        }
    }

    private IEnumerator MoverDeA_B(GameObject ficha, Vector3 origen, Vector3 destino)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duracionPaso;
            ficha.transform.position = Vector3.Lerp(origen, destino, t);
            yield return null;
        }
        ficha.transform.position = destino;
    }
}










