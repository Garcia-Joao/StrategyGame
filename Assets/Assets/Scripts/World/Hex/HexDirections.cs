using System.Collections.Generic;

public static class HexDirections
{
    public static readonly HexCoord East = new(1, -1, 0);
    public static readonly HexCoord NorthEast = new(1, 0, -1);
    public static readonly HexCoord NorthWest = new(0, 1, -1);
    public static readonly HexCoord West = new(-1, 1, 0);
    public static readonly HexCoord SouthWest = new(-1, 0, 1);
    public static readonly HexCoord SouthEast = new(0, -1, 1);

    public static readonly IReadOnlyList<HexCoord> AllDirections =
        new[]
        {
            East,
            NorthEast,
            NorthWest,
            West,
            SouthWest,
            SouthEast
        };
}