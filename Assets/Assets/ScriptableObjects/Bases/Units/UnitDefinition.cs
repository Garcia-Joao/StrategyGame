using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Units/Unit Definition")]
public class UnitDefinition : ScriptableObject
{
    public string UnitName;

    [Header("Movement")]
    public int MovementPoints;
    public float MoveSpeed;

    [Header("Attributes")]
    public int Strength;
    public int Dexterity;
    public int Reflexes;
    public int Vitality;
}