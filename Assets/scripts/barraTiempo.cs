using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class barraTiempo : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public float cooldown;
    private float cooldownTimer = 10;

    void Start()
    {
        
       
    }

    void Update()
    {
        
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            icon.fillAmount = 1 - (cooldown / cooldownTimer); // La barra se vacía con el tiempo
        }
        if (cooldownTimer <= 0)
        {
            icon.fillAmount = 1;
        }

        


    }
}
