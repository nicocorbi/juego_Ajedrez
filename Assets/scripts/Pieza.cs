using UnityEngine;

public class Pieza
{
    public GameObject pieza;
    public Vector2Int posicionActual;
    public bool esJugador1; 

    public Pieza(GameObject piezaVisual, Vector2Int posicionInicial, bool esJugador1)
    {
        pieza = piezaVisual;
        posicionActual = posicionInicial;
        this.esJugador1 = esJugador1;
    }

    public void Mover(Vector2Int nuevaPosicion, Vector3 nuevaPosicionVisual)
    {
        posicionActual = nuevaPosicion;
        pieza.transform.position = nuevaPosicionVisual;
    }
}
