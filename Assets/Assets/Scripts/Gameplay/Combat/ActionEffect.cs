using UnityEngine;

public abstract class ActionEffect : ScriptableObject
{
    public abstract void Apply(ActionContext context);
}