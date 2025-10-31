using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "WaitForFollowFlag",
    story: "Wait until [Self] ShouldFollow, then set [Target] and [HasTarget]",
    category: "Flow",
    id: "wait_for_followflag_001")]
public partial class WaitForFollowFlagAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target; // ? Transform
    [SerializeReference] public BlackboardVariable<bool> HasTarget;

    protected override Status OnUpdate()
    {
        var go = Self?.Value;
        if (go == null) return Status.Failure;

        var flag = go.GetComponent<FollowFlag>();
        if (flag != null && flag.ShouldFollow && flag.Target != null)
        {
            Target.Value = flag.Target; // ? guarda el Transform en el Blackboard
            HasTarget.Value = true;
            return Status.Success;
        }
        return Status.Running;
    }
}
