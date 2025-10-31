// Assets/Scripts/IInteractable.cs
using UnityEngine;

namespace Game.Interactions
{
    // Interfaz única en TODO el proyecto
    public interface IInteractable
    {
        // Usa un tipo genérico/flexible para no romper si cambias tu controller
        void Interact(Transform interactor);
    }
}
