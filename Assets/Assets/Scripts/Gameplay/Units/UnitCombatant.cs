using UnityEngine;

public class UnitCombatant : MonoBehaviour
{
    public HexUnit Unit { get; private set; }

    public UnitStats Stats =>
        Unit.Stats;

    public UnitResources Resources =>
        Unit.Resources;

    public ActionStats ActionStats =>
        Unit.ActionStats;

    public void Initialize(
        HexUnit unit)
    {
        Unit = unit;
    }
}