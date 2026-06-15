using System.Collections.Generic;

public class FireballTerrainBrush : ITerrainBrush
{
    private readonly int radius;

    private readonly float depth;

    public FireballTerrainBrush(
        int radius = 3,
        float depth = 2f)
    {
        this.radius = radius;
        this.depth = depth;
    }

    public IEnumerable<BrushCell> GetAffectedCells(
        HexGrid grid,
        HexCell centerCell)
    {
        foreach (HexCell cell in grid.GetAllCells())
        {
            int distance =
                centerCell.Coordinate.DistanceTo(
                    cell.Coordinate);

            if (distance > radius)
            {
                continue;
            }

            float intensity =
                1f -
                (distance / (float)radius);

            yield return new BrushCell(
                cell,
                intensity);
        }
    }

    public void Apply(
        HexGrid grid,
        HexCell centerCell)
    {
        foreach (BrushCell affectedCell in
                 GetAffectedCells(
                     grid,
                     centerCell))
        {
            affectedCell.Cell.SetHeight(
                affectedCell.Cell.Height -
                depth * affectedCell.Intensity);
        }
    }
}