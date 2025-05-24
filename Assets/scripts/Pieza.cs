using UnityEngine;

public class Pieza : IAttackable
{
    public GameObject pieza;
    public Vector2Int posicionActual;
    public bool esJugador1;
    public Color colorInicial;
    private VidaPieza vidaPieza;
    public float multiplicadorDanio = 1f;
    public ParticleSystem buffParticles;
    // Cooldown de ataque
    private float cooldownAtaque = 2f;
    private float tiempoUltimoAtaque = -999f;

    public Pieza(
        GameObject piezaVisual,
        Vector2Int posicionInicial,
        bool esJugador1,
        Color colorInicial,
        int vida,
        float escalaInicial,
        ParticleSystem buffParticles = null // Nuevo parámetro opcional
    )
    {
        this.pieza = piezaVisual;
        this.posicionActual = posicionInicial;
        this.esJugador1 = esJugador1;
        this.colorInicial = colorInicial;
        vidaPieza = piezaVisual.GetComponent<VidaPieza>();
        if (vidaPieza != null)
            vidaPieza.Inicializar(vida, escalaInicial);

        this.buffParticles = buffParticles;
        if (this.buffParticles != null)
            this.buffParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void RecibirDanio(int cantidad)
    {
        if (vidaPieza != null)
            vidaPieza.RecibirDanio(cantidad);
    }

    public int VidaActual
    {
        get { return vidaPieza != null ? vidaPieza.GetVidaActual() : 0; }
    }

    public int CalcularDanio(int danioBase)
    {
        return Mathf.RoundToInt(danioBase * multiplicadorDanio);
    }

    public bool PuedeAtacar()
    {
        return Time.time - tiempoUltimoAtaque >= cooldownAtaque;
    }

    public void RegistrarAtaque()
    {
        tiempoUltimoAtaque = Time.time;
    }

    public void AjustarCooldown(int piezasRestantes)
    {
        // Ejemplo: reduce el cooldown si quedan pocas piezas
        cooldownAtaque = Mathf.Max(0.5f, 2f - (3 - piezasRestantes) * 0.3f);
    }
}










