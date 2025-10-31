// AttackAction.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackAction", menuName = "UtilityAI/Actions/Attack Action")]
public class AttackAction : Action
{
    public List<Consideration> considerations;

    public override float CalculateUtility(Context context)
    {
        float totalScore = 1f;
        foreach (var consideration in considerations)
        {
            totalScore *= consideration.Evaluate(context);
        }
        return totalScore;
    }

    public override void Execute(Context context)
    {
        Debug.Log("Ejecutando Acción: Atacar");
        GameObject target = GetClosestEnemy(context);
        if (target != null)
        {
            context.navMeshAgent.SetDestination(target.transform.position);
        }
    }

    private GameObject GetClosestEnemy(Context context)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;
        foreach (var enemy in context.nearbyEnemies)
        {
            float distance = Vector3.Distance(context.agent.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }
}