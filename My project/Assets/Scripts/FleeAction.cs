// FleeAction.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FleeAction", menuName = "UtilityAI/Actions/Flee Action")]
public class FleeAction : Action
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
        Debug.Log("Ejecutando Acción: Huir");
        if (context.safeZone != null)
        {
            context.navMeshAgent.SetDestination(context.safeZone.position);
        }
    }
}