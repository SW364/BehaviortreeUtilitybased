// InteractableCube.cs
using UnityEngine;
using Game.Interactions;  // <-- usa la misma interfaz

public class InteractableCube : MonoBehaviour, IInteractable
{
    ActivationFlag flag;

    void Awake() => flag = GetComponent<ActivationFlag>();

    public void Interact(Transform interactor)
    {
        if (flag != null) flag.TriggerOnce();  // 👈 dispara al graph del cubo
    }
}