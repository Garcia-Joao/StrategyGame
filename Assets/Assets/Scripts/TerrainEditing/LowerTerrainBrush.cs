using System.Collections.Generic;

public class LowerTerrainBrush : ITerrainBrush
{
    private readonly float amount;

    public LowerTerrainBrush(float amount = 1f)
    {
        this.amount = amount;
    }

    public IEnumerable<BrushCell> GetAffectedCells(
        HexGrid grid,
        HexCell centerCell)
    {
        yield return new BrushCell(
            centerCell,
            1f);
    }

    public void Apply(
        HexGrid grid,
        HexCell centerCell)
    {
        centerCell.SetHeight(
            centerCell.Height - amount);
    }
}