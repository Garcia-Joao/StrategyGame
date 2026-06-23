using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Action", menuName = "Combat/Action")]
public class UnitActionDefinition : ScriptableObject
{
    public string ActionName;

    public ActionCost Cost;

    public TargetRules TargetRules;

    public RangeDefinition Range;

    public AreaDefinition Area;

    public List<ActionEffect> Effects;
}