using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Units/Unit Definition")]
public class UnitDefinition : ScriptableObject
{
    [Header("Identity")]
    public string UnitName;

    [Header("Visual")]
    public GameObject Prefab;

    [Header("Actions")]
    public int ActionPoints;
    public int QuickActionPoints;

    public List<UnitActionDefinition> Actions;

    [Header("Stats")]
    public int StartHealth;
    public int StartMana;

    [Header("Movement")]
    public int MovementPoints;
    public float MoveSpeed;

    [Header("Attributes")]
    public int Strength;
    public int Dexterity;
    public int Reflexes;
    public int Vitality;
}