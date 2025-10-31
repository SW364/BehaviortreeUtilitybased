using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "FollowTargetTransform",
    story: "[Self] follows [Target] until within [StoppingDistance]",
    category: "Navigation",
    id: "follow_target_transform_001")]
public partial class MoveToTargetAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;     // ? Transform
    [SerializeReference] public BlackboardVariable<float> StoppingDistance;

    NavMeshAgent agent;

    protected override Status OnStart()
    {
        agent = Self?.Value != null ? Self.Value.GetComponent<NavMeshAgent>() : null;
        return agent != null ? Status.Running : Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (agent == null || Target?.Value == null) return Status.Failure;

        float dist = Vector3.Distance(agent.transform.position, Target.Value.position);
        if (dist <= StoppingDistance.Value)
        {
            agent.ResetPath();
            return Status.Success;
        }

        agent.SetDestination(Target.Value.position); // ? siempre la posición actual
        return Status.Running;
    }
}
