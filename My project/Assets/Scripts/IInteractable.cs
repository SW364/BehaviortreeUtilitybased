// Assets/Scripts/IInteractable.cs
using UnityEngine;

namespace Game.Interactions
{
    // Interfaz �nica en TODO el proyecto
    public interface IInteractable
    {
        // Usa un tipo gen�rico/flexible para no romper si cambias tu controller
        void Interact(Transform interactor);
    }
}
