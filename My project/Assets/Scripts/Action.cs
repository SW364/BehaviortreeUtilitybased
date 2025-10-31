// Action.cs
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string actionName;
    public abstract float CalculateUtility(Context context);
    public abstract void Execute(Context context);
}