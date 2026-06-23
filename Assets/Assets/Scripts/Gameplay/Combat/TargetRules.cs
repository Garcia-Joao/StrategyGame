using System;

[Serializable]
public class TargetRules
{
    public TargetType AllowedTargets;

    public bool RequireOccupiedTile = true;

    public bool AllowEmptyTile;

    public bool AllowDeadUnits;
}