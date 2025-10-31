using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "DetectPlayerNearby",
    story: "Detect if [Player] is within [DetectionRadius] of [Self] and store [TargetPosition] & [HasTarget]",
    category: "Perception",
    id: "detect_player_nearby_001")]
public partial class DetectPlayerNearbyAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> DetectionRadius;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;
    [SerializeReference] public BlackboardVariable<bool> HasTarget;

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Player?.Value == null) return Status.Failure;

        float d = Vector3.Distance(Self.Value.transform.position, Player.Value.transform.position);
        bool detected = d <= DetectionRadius.Value;
        HasTarget.Value = detected;
        if (detected) TargetPosition.Value = Player.Value.transform.position;

        return Status.Success;
    }
}
