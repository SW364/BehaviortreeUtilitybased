// Consideration.cs
using UnityEngine;

public abstract class Consideration : ScriptableObject
{
    public string considerationName;
    
    // Una curva de respuesta para ajustar la puntuación visualmente
    public AnimationCurve responseCurve;

    public abstract float Evaluate(Context context);
}