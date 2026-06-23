using System;

[Serializable]
public class MovementProperties
{
    public bool IgnoreOccupants;

    public bool CanShareTile;

    public bool IgnoreHeight;

    public MovementProperties Clone()
    {
        return new MovementProperties
        {
            IgnoreOccupants = IgnoreOccupants,
            CanShareTile = CanShareTile,
            IgnoreHeight = IgnoreHeight
        };
    }
}