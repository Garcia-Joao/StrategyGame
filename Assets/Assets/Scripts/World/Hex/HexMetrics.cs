using UnityEngine;

public static class HexMetrics
{
    public const float OuterRadius = 1f;

    public const float InnerRadius =
        OuterRadius * 0.866025404f;

    public const float HeightStep = 1f;

    public static Vector3 GetWorldPosition(HexCoord coord)
    {
        float x =
            (coord.X + coord.Z * 0.5f) *
            (InnerRadius * 2f);

        float z =
            coord.Z *
            (OuterRadius * 1.5f);

        return new Vector3(x, 0f, z);
    }

    public static Vector3 GetWorldPosition(
        HexCoord coord,
        float height)
    {
        Vector3 position =
            GetWorldPosition(coord);

        position.y =
            height * HeightStep;

        return position;
    }
}