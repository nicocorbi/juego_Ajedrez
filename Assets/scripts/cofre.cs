using UnityEngine;

public class Cofre : IAttackable
{
    public GameObject cofreGO;
    public Vector2Int posicionActual;
    public Color colorInicial;
    private VidaPieza vidaPieza;

    public Cofre(GameObject cofreVisual, Vector2Int posicionInicial, Color colorInicial, int vida, float escalaInicial)
    {
        this.cofreGO = cofreVisual;
        this.posicionActual = posicionInicial;
        this.colorInicial = colorInicial;
        vidaPieza = cofreVisual.GetComponent<VidaPieza>();
        if (vidaPieza != null)
            vidaPieza.Inicializar(vida, escalaInicial);
    }

    public void RecibirDanio(int cantidad)
    {
        if (vidaPieza != null)
            vidaPieza.RecibirDanio(cantidad);
    }

    public int VidaActual => vidaPieza != null ? vidaPieza.GetVidaActual() : 0;
}








