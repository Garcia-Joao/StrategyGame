using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Classes/Class")]
public class ClassDefinition : ScriptableObject
{
    public string className;

    [Header("Movement")]
    public int movementPointsBonus;

    [Header("Attributes")]
    public int strengthBonus;
    public int dexterityBonus;
    public int reflexesBonus;
    public int vitalityBonus;
}