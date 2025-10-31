using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "EvaluateGroupUtilityAction",
    story: "[Self] evaluates group power vs player strength to decide Attack, Flee, or Idle.",
    category: "AI",
    id: "evaluate_group_utility_action_002")]
public partial class EvaluateUtilityAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> MinAttackDistance;
    [SerializeReference] public BlackboardVariable<float> FleeThreshold;
    [SerializeReference] public BlackboardVariable<float> DetectionRadius;

    private NavMeshAgent agent;
    private Stats stats;
    private string currentState = "Idle";
    private float decisionCooldown = 1.5f;
    private float decisionTimer = 0f;

    protected override Status OnStart()
    {
        if (Self?.Value == null)
            return Status.Failure;

        agent = Self.Value.GetComponent<NavMeshAgent>();
        stats = Self.Value.GetComponent<Stats>();

        if (agent == null || stats == null)
            return Status.Failure;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Player?.Value == null)
            return Status.Failure;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer > 0f)
        {
            ExecuteCurrentState();
            return Status.Running;
        }

        Vector3 selfPos = Self.Value.transform.position;
        float healthPercent = stats.currentHealth / stats.maxHealth;

        Collider[] nearby = Physics.OverlapSphere(selfPos, DetectionRadius.Value);
        int nearbyAllies = 0;
        foreach (var col in nearby)
        {
            if (col.gameObject == Self.Value) continue;
            if (col.CompareTag("FollowerNPC")) nearbyAllies++;
        }

        float distance = Vector3.Distance(selfPos, Player.Value.transform.position);
        float playerStrength = PlayerStatus.Instance != null ? PlayerStatus.Instance.Strength : 50f;
        float groupPower = 30f + (nearbyAllies * 10f); // base strength + bonus por aliados
        groupPower *= healthPercent;

        // === UTILIDADES ===
        float attackUtility = Mathf.Clamp01((groupPower / Mathf.Max(1f, playerStrength)) * (1f - (distance / MinAttackDistance.Value)));
        float fleeUtility = Mathf.Clamp01((1f - healthPercent) + Mathf.Clamp01(playerStrength / Mathf.Max(1f, groupPower)));
        float idleUtility = 1f - Mathf.Max(attackUtility, fleeUtility);

        string bestAction = "Idle";
        float bestScore = idleUtility;

        if (attackUtility > bestScore) { bestAction = "Attack"; bestScore = attackUtility; }
        if (fleeUtility > bestScore) { bestAction = "Flee"; bestScore = fleeUtility; }

        if (bestAction != currentState)
        {
            currentState = bestAction;
            decisionTimer = decisionCooldown;
        }

        ExecuteCurrentState();

        Debug.Log($"{Self.Value.name} decide {currentState} | Power:{groupPower:F1} vs Player:{playerStrength:F1} | Allies:{nearbyAllies}");
        return Status.Running;
    }

    private void ExecuteCurrentState()
    {
        switch (currentState)
        {
            case "Attack": MoveTowardsPlayer(); break;
            case "Flee": FleeFromPlayer(); break;
            default: agent.ResetPath(); break;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (agent == null || Player?.Value == null) return;
        agent.SetDestination(Player.Value.transform.position);
        var awareness = Self.Value.GetComponent<GroupAwareness>();
        if (awareness != null) awareness.AlertGroup(Self.Value.transform);
    }

    private void FleeFromPlayer()
    {
        if (agent == null || Player?.Value == null) return;

        Vector3 dir = (Self.Value.transform.position - Player.Value.transform.position).normalized;
        Vector3 fleePos = Self.Value.transform.position + dir * 5f;
        if (NavMesh.SamplePosition(fleePos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}
