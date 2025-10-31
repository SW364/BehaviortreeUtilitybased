// HealthConsideration.cs
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConsideration", menuName = "UtilityAI/Considerations/Health Consideration")]
public class HealthConsideration : Consideration
{
    public override float Evaluate(Context context)
    {
        float healthPercentage = context.stats.currentHealth / context.stats.maxHealth;
        return responseCurve.Evaluate(healthPercentage);
    }
}