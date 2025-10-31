using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;  // Para Singleton
    [Header("Stats del jugador")]
    public float Strength = 50f; // Valor de fuerza de 0 a 100
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        

        // Cuando se hace clic en un NPC, le baja la vida
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Rayo desde el mouse

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("FollowerNPC"))
                {
                    var npc = hit.collider.GetComponent<Stats>();
                    if (npc != null)
                    {
                        npc.TakeDamage(10);  // Reducir vida en 10
                        Debug.Log($"{npc.name} recibió 10 de daño.");
                    }
                }
            }
        }
    }

    public void UpdateStrength(float baseValue)
    {
        Strength = Mathf.Clamp(baseValue, 0, 100);
    }
}
