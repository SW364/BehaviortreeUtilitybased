using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "ActivateNearbyFollowers",
    story: "[Self] activates NPCs with tag [NPCTag] within [ActivationRadius] to follow [Player]",
    category: "Interaction",
    id: "activate_followers_001")]
public partial class ActivateNearbyFollowersAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<Transform> Player; // ? Transform
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<string> NPCTag;
    [SerializeReference] public BlackboardVariable<float> ActivationRadius;

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Player?.Value == null) return Status.Failure;

        var npcs = GameObject.FindGameObjectsWithTag(NPCTag.Value);
        foreach (var npc in npcs)
        {
            if (Vector3.Distance(npc.transform.position, Self.Value.transform.position) <= ActivationRadius.Value)
            {
                var flag = npc.GetComponent<FollowFlag>();
                if (flag != null) flag.Activate(Player.Value); // ? pasa Transform directo
            }
        }
        return Status.Success;
    }
}
