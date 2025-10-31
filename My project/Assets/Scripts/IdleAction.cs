// IdleAction.cs
using UnityEngine;

[CreateAssetMenu(fileName = "IdleAction", menuName = "UtilityAI/Actions/Idle Action")]
public class IdleAction : Action
{
    [Range(0, 1)]
    public float utilityScore = 0.1f; // Puntuación constante y baja

    public override float CalculateUtility(Context context)
    {
        return utilityScore;
    }

    public override void Execute(Context context)
    {
        Debug.Log("Ejecutando Acción: Inactivo");
        context.navMeshAgent.SetDestination(context.agent.transform.position);
    }
}