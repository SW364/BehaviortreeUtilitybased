// Context.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// No se añade a ningún objeto, es solo una clase de datos
public class Context
{
    public GameObject agent;
    public NavMeshAgent navMeshAgent;
    public Stats stats;
    public List<GameObject> nearbyEnemies;
    public Transform safeZone;

    // Constructor para inicializar el contexto
    public Context(GameObject agent, NavMeshAgent navMeshAgent, Stats stats, List<GameObject> nearbyEnemies, Transform safeZone)
    {
        this.agent = agent;
        this.navMeshAgent = navMeshAgent;
        this.stats = stats;
        this.nearbyEnemies = nearbyEnemies;
        this.safeZone = safeZone;
    }
}
