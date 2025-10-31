using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;

public class GroupAwareness : MonoBehaviour
{
    public static HashSet<GroupAwareness> ActiveFollowers = new HashSet<GroupAwareness>();

    public bool IsAlerted = false;
    public bool IsLeader = false;
    public Transform AlertSource;
    public Transform Leader;

    [Header("Configuración de comportamiento")]
    public float ShareRadius = 12f;       // Radio para reclutar aliados
    public float EngageRadius = 8f;       // Radio para atacar
    public float LowHealthThreshold = 0.3f; // <30% vida → huir
    public float BaseStrength = 30f;      // Fuerza base del NPC individual (0–100)

    private NavMeshAgent agent;
    private FollowFlag follow;
    private Stats stats;
    private Transform player;

    void OnEnable() => ActiveFollowers.Add(this);
    void OnDisable() => ActiveFollowers.Remove(this);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        follow = GetComponent<FollowFlag>();
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!IsAlerted || player == null || follow == null) return;

        float healthPercent = stats.currentHealth / stats.maxHealth;
        float playerStrength = PlayerStatus.Instance != null ? PlayerStatus.Instance.Strength : 50f;

        // Calculamos poder del grupo (cada aliado aporta un poco)
        int nearbyAllies = GetNearbyAlliesCount();
        float groupPower = BaseStrength + (nearbyAllies * 10f); // cada aliado agrega 10 puntos de poder
        groupPower *= healthPercent; // si está herido, su poder efectivo baja

        bool shouldRetreat = healthPercent < LowHealthThreshold || groupPower < playerStrength * 0.8f;
        bool shouldRecruit = groupPower < playerStrength && nearbyAllies < 4;
        bool shouldAttack = groupPower >= playerStrength * 0.9f;

        if (shouldRetreat)
        {
            MoveAwayFromPlayer();
            RecruitAllies();
            return;
        }

        if (shouldRecruit)
        {
            RecruitAllies();
            return;
        }

        if (shouldAttack)
        {
            AttackPlayer();
            return;
        }

        // Si no cumple ninguna, se queda con el grupo
        if (Leader != null && Vector3.Distance(transform.position, player.position) > EngageRadius)
        {
            agent.SetDestination(Leader.position);
        }
    }

    private int GetNearbyAlliesCount()
    {
        int count = 0;
        foreach (var ally in ActiveFollowers)
        {
            if (ally == this) continue;
            if (Vector3.Distance(transform.position, ally.transform.position) <= ShareRadius)
                count++;
        }
        return count;
    }

    public void AlertGroup(Transform player)
    {
        if (player == null) return;

        IsAlerted = true;
        IsLeader = true;
        AlertSource = player;

        if (follow != null)
        {
            follow.ShouldFollow = true;
            follow.Target = player;
        }

        RecruitAllies();
        Debug.Log($"🔥 {name} se volvió líder. Reclutando aliados…");
    }

    private void RecruitAllies()
    {
        foreach (var ally in ActiveFollowers)
        {
            if (ally == this || ally.IsAlerted) continue;
            float dist = Vector3.Distance(transform.position, ally.transform.position);
            if (dist <= ShareRadius)
                ally.ReceiveAlert(AlertSource);
        }
    }

    public void ReceiveAlert(Transform player)
    {
        if (IsAlerted || player == null) return;

        IsAlerted = true;
        IsLeader = false;
        AlertSource = player;

        if (follow != null)
        {
            follow.ShouldFollow = true;
            follow.Target = AlertSource;
        }

        var agent = GetComponent<BehaviorGraphAgent>();
        if (agent != null)
            agent.Restart();

        Debug.Log($"🟢 {name} se unió al grupo de {AlertSource.name}");
    }

    public void MoveAwayFromPlayer()
    {
        if (agent == null || player == null) return;

        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 fleePos = transform.position + dir * 6f;

        if (NavMesh.SamplePosition(fleePos, out NavMeshHit hit, 6f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"🏃‍♂️ {name} huye del jugador más fuerte ({PlayerStatus.Instance.Strength}).");
        }
    }

    private void AttackPlayer()
    {
        if (agent == null || player == null) return;

        follow.Target = player;
        follow.ShouldFollow = true;
        agent.SetDestination(player.position);

        var awareness = GetComponent<GroupAwareness>();
        if (awareness != null)
            awareness.AlertGroup(this.transform);
    }
}
