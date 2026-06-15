using System;
using UnityEngine;

public readonly struct HexCoord : IEquatable<HexCoord>
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public static HexCoord Zero => new(0, 0, 0);

    public HexCoord(int x, int y, int z)
    {
        if (x + y + z != 0)
        {
            throw new ArgumentException(
                $"Invalid HexCoord ({x}, {y}, {z}). X + Y + Z must equal 0.");
        }

        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(HexCoord other)
    {
        return X == other.X &&
               Y == other.Y &&
               Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        return obj is HexCoord other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public static HexCoord operator +(HexCoord a, HexCoord b)
    {
        return new HexCoord(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);
    }

    public static HexCoord operator -(HexCoord a, HexCoord b)
    {
        return new HexCoord(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z);
    }

    public static bool operator ==(HexCoord left, HexCoord right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(HexCoord left, HexCoord right)
    {
        return !left.Equals(right);
    }



    public int DistanceTo(HexCoord other)
    {
        return (
            Mathf.Abs(X - other.X) +
            Mathf.Abs(Y - other.Y) +
            Mathf.Abs(Z - other.Z)
        ) / 2;
    }
}