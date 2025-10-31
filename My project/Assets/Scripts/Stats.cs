using UnityEngine;

public class Stats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Llamado cuando el NPC recibe da�o
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // Mostrar vida actual en la consola
        Debug.Log($"{gameObject.name} tiene {currentHealth} de vida.");

        // Verifica si la salud est� por debajo del umbral y si deber�a huir
        CheckHealthAndFlee();
    }

    // Verifica si la vida es baja y si deber�a huir
    private void CheckHealthAndFlee()
    {
        var groupAwareness = GetComponent<GroupAwareness>();
        if (groupAwareness != null && currentHealth / maxHealth < groupAwareness.LowHealthThreshold)
        {
            // Si la salud es baja, este NPC deber�a huir
            groupAwareness.MoveAwayFromPlayer();
        }
    }
}
