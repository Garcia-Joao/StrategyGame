using System.Collections.Generic;

public class HexGrid
{
    private readonly Dictionary<HexCoord, HexCell> cells = new();

    public int CellCount => cells.Count;

    public void AddCell(HexCell cell)
    {
        cells.Add(cell.Coordinate, cell);
    }

    public bool ContainsCell(HexCoord coordinate)
    {
        return cells.ContainsKey(coordinate);
    }

    public HexCell GetCell(HexCoord coordinate)
    {
        return cells[coordinate];
    }

    public bool TryGetCell(
        HexCoord coordinate,
        out HexCell cell)
    {
        return cells.TryGetValue(
            coordinate,
            out cell);
    }

    public IEnumerable<HexCell> GetAllCells()
    {
        return cells.Values;
    }

    public List<HexCell> GetNeighbors(HexCoord coordinate)
    {
        List<HexCell> neighbors = new();

        foreach (HexCoord direction in HexDirections.AllDirections)
        {
            HexCoord neighborCoordinate =
                coordinate + direction;

            if (TryGetCell(
                    neighborCoordinate,
                    out HexCell neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    public void RemoveCell(HexCoord coordinate)
    {
        cells.Remove(coordinate);
    }

    public void Clear()
    {
        cells.Clear();
    }
}