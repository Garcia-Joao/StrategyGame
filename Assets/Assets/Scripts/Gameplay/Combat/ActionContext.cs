using System.Collections.Generic;

public class ActionContext
{
    public UnitActionDefinition Action;

    public HexUnit Source;

    public HexCell TargetCell;

    public List<HexCell> AffectedCells;

    public List<HexUnit> AffectedUnits;
}