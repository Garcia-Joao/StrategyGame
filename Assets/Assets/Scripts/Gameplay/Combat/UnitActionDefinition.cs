using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Unit Action",
    menuName = "Combat/Action")]
public class UnitActionDefinition : ScriptableObject
{
    [Header("Identity")]
    public string ActionName;

    [Header("Cost")]
    public ActionCost Cost;

    [Header("Targeting")]
    public TargetRules TargetRules;

    public RangeDefinition Range;

    [Header("Area")]
    public AreaDefinition Area;

    [Header("Effects")]
    public List<ActionEffect> Effects;

    [Header("Types")]
    public List<ActionType> ActionTypes;
}

public enum ActionType
{
    Attack,
    Heal,
    Buff,
    Movement
}