// WaitForActivationFlagAction.cs
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "WaitForActivationFlag",
    story: "Wait until [Self] ActivationFlag.ShouldActivate is true (then reset)",
    category: "Flow",
    id: "wait_for_activation_flag_001")]
public partial class WaitForActivationFlagAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    ActivationFlag flag;

    protected override Status OnStart()
    {
        flag = Self?.Value != null ? Self.Value.GetComponent<ActivationFlag>() : null;
        return flag != null ? Status.Running : Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (flag == null) return Status.Failure;
        if (!flag.ShouldActivate) return Status.Running;

        // consumir el trigger
        flag.ShouldActivate = false;
        return Status.Success;
    }
}
