using UnityEngine;

public class FollowFlag : MonoBehaviour
{
    public bool ShouldFollow = false;
    public Transform Target;

    public void Activate(Transform player)
    {
        ShouldFollow = true;
        Target = player;
    }

    public void Deactivate()
    {
        ShouldFollow = false;
        Target = null;
    }
}
