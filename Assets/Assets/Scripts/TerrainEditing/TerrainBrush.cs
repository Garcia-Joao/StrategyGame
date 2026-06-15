using System.Collections.Generic;

public interface ITerrainBrush
{
    IEnumerable<BrushCell> GetAffectedCells(
        HexGrid grid,
        HexCell centerCell);

    void Apply(
        HexGrid grid,
        HexCell centerCell);
}

public readonly struct BrushCell
{
    public HexCell Cell { get; }

    public float Intensity { get; }

    public BrushCell(
        HexCell cell,
        float intensity)
    {
        Cell = cell;
        Intensity = intensity;
    }
}