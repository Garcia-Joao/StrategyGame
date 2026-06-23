public interface IHexOccupant
{
    bool BlocksMovement { get; }

    bool BlocksVision { get; }

    bool IsDestructible { get; }
}