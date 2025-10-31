// ActivationFlag.cs
using UnityEngine;

public class ActivationFlag : MonoBehaviour
{
    public bool ShouldActivate = false;
    public void TriggerOnce() => ShouldActivate = true;  // lo pondremos a false desde el nodo
}
