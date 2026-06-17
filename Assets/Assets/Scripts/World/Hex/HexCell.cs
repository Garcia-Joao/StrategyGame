using UnityEngine;

public class HexCell
{
    public HexCoord Coordinate { get; }

    public float Height { get; private set; }

    public int MovementCost { get; private set; }

    public Vector3 WorldPosition =>
        HexMetrics.GetWorldPosition(Coordinate, Height);

    public HexUnit OccupyingUnit { get; private set; }

    public bool IsOccupied => OccupyingUnit != null;

    public HexCell(
        HexCoord coordinate,
        float height,
        int movementCost = 0)
    {
        Coordinate = coordinate;
        Height = height;
        MovementCost = movementCost;
    }

    public void SetHeight(float height)
    {
        Height = height;
    }

    public void SetMovementCost(int movementCost)
    {
        MovementCost = movementCost;
    }

    public override string ToString()
    {
        return $"Cell {Coordinate} | Height: {Height} | Cost: {MovementCost}";
    }

    public void SetOccupyingUnit(
    HexUnit unit)
    {
        OccupyingUnit = unit;
    }
}