using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(
    name: "IsAlertedTrue",
    story: "[Self] is alerted [IsAlerted] == true",
    category: "Conditions",
    id: "is_alerted_true_condition_001")]
public partial class IsAlertedTrueCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> IsAlerted;

    public override bool IsTrue()
    {
        if (Self == null || Self.Value == null)
            return false;

        // lee la variable del Blackboard
        return IsAlerted != null && IsAlerted.Value;
    }

    public override void OnStart() { }
    public override void OnEnd() { }
}
