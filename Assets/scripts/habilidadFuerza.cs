using UnityEngine;
using System.Collections;

public class habilidadFuerza : MonoBehaviour
{
    public static float multiplicadorDanio = 3f;
    public static float duracionBuff = 10f;

    // Llama a este método para aplicar el buff a una pieza
    public static void AplicarBuffFuerza(Pieza pieza, MonoBehaviour invocador)
    {
        if (pieza != null && invocador != null)
            invocador.StartCoroutine(BuffCoroutine(pieza));
    }

    private static IEnumerator BuffCoroutine(Pieza pieza)
    {
        Debug.Log($"[habilidadFuerza] Buff de daño x{multiplicadorDanio} ACTIVADO en pieza {pieza.pieza.name}");
        pieza.multiplicadorDanio = multiplicadorDanio;

        // Activa las partículas si existen
        if (pieza.buffParticles != null)
        {
            pieza.buffParticles.Play();
        }
        else
        {
            Debug.LogWarning($"[habilidadFuerza] No se encontró el sistema de partículas en la pieza: {pieza.pieza.name}");
        }

        yield return new WaitForSeconds(duracionBuff);

        pieza.multiplicadorDanio = 1f;

        // Desactiva las partículas si existen
        if (pieza.buffParticles != null)
        {
            pieza.buffParticles.Stop();
        }

        Debug.Log($"[habilidadFuerza] Buff de daño FINALIZADO en pieza {pieza.pieza.name}");
    }

}







