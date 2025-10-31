// EnemyNearbyConsideration.cs
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyNearbyConsideration", menuName = "UtilityAI/Considerations/Enemy Nearby Consideration")]
public class EnemyNearbyConsideration : Consideration
{
    public override float Evaluate(Context context)
    {
        // Si hay enemigos, el valor es 1, si no, es 0. La curva decidirá la puntuación final.
        float value = (context.nearbyEnemies != null && context.nearbyEnemies.Count > 0) ? 1f : 0f;
        return responseCurve.Evaluate(value);
    }
}