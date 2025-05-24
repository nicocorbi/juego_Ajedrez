using UnityEngine;
using System.Collections;

public class GeneradorObjetoEnCofre : MonoBehaviour
{
    public string cofreTag = "Cofre";
    public GameObject prefabAInstanciar;
    public float rotacionY = 90f;
    public float alturaSpawn = 0f;

    private GameObject objetoInstanciado;
    private Vector2Int casillaCofre;
    private bool cofreDesaparecido = false;
    private MainManager mainManager;

    private void Start()
    {
        mainManager = FindObjectOfType<MainManager>();
        StartCoroutine(GenerarObjetoTrasRetraso());
    }

    private IEnumerator GenerarObjetoTrasRetraso()
    {
        yield return new WaitForSeconds(4f);

        GameObject cofreGO = GameObject.FindGameObjectWithTag(cofreTag);
        if (cofreGO != null && prefabAInstanciar != null && mainManager != null)
        {
            Vector3 posicionCofre = cofreGO.transform.position;
            posicionCofre.y += alturaSpawn;
            objetoInstanciado = Instantiate(prefabAInstanciar, posicionCofre, Quaternion.identity);

            // Buscar la posición lógica del cofre en el MainManager
            casillaCofre = Vector2Int.zero;
            foreach (var cofre in mainManager.GetCofresEnJuego())
            {
                if (cofre.cofreGO == cofreGO)
                {
                    casillaCofre = cofre.posicionActual;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (objetoInstanciado != null)
        {
            objetoInstanciado.transform.Rotate(0, rotacionY * Time.deltaTime, 0);

            if (!cofreDesaparecido && GameObject.FindGameObjectWithTag(cofreTag) == null)
            {
                cofreDesaparecido = true;
            }

            if (cofreDesaparecido && mainManager != null && mainManager.HayPiezaEnCasilla(casillaCofre))
            {
                // Obtener la pieza en la casilla
                Pieza pieza = mainManager.GetPiezaEnCasilla(casillaCofre);
                if (pieza != null)
                {
                    habilidadFuerza.AplicarBuffFuerza(pieza, this);
                }
                Destroy(objetoInstanciado);
            }
        }
    }
}










