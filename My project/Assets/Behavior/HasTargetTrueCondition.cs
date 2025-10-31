using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties; // <-- añade esto

[Serializable, GeneratePropertyBag]
[Condition(
    name: "HasTargetTrue",
    story: "[HasTarget] is [Value]",
    category: "Conditions",
    id: "35c57f4bf2115577b1537ba2f831b936")]
public partial class HasTargetTrueCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> HasTarget;
    [SerializeReference] public BlackboardVariable<bool> Value;  // normalmente True

    public override bool IsTrue()
    {
        if (HasTarget == null) return false;

        // Si no asignas "Value" en el inspector, asumimos true
        bool expected = (Value != null) ? Value.Value : true;
        return HasTarget.Value == expected;
    }

    public override void OnStart() { }
    public override void OnEnd() { }
}
