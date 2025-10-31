// Brain.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(Stats), typeof(Sensor), typeof(NavMeshAgent))]
public class Brain : MonoBehaviour
{
    public List<Action> actions;
    private Stats _stats;
    private Sensor _sensor;
    private NavMeshAgent _navMeshAgent;
    private Context _context;
    private Transform _safeZone;

    void Awake()
    {
        _stats = GetComponent<Stats>();
        _sensor = GetComponent<Sensor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        // Encontrar la zona segura por su tag
        GameObject safeZoneObj = GameObject.FindGameObjectWithTag("SafeZone");
        if(safeZoneObj != null) _safeZone = safeZoneObj.transform;

        // Crear el contexto una vez
        _context = new Context(gameObject, _navMeshAgent, _stats, _sensor.detectedEnemies, _safeZone);
    }

    void Update()
    {
        // Actualizar la lista de enemigos en el contexto por si ha cambiado
        _context.nearbyEnemies = _sensor.detectedEnemies;

        ChooseBestAction();

        // Para probar, presiona 'D' para hacerle daño al NPC
        if (Input.GetKeyDown(KeyCode.D))
        {
            _stats.TakeDamage(20);
        }
    }

    void ChooseBestAction()
    {
        Action bestAction = null;
        float highestUtility = float.MinValue;

        foreach (var action in actions)
        {
            float utility = action.CalculateUtility(_context);
            if (utility > highestUtility)
            {
                highestUtility = utility;
                bestAction = action;
            }
        }

        if (bestAction != null)
        {
            bestAction.Execute(_context);
        }
    }
}