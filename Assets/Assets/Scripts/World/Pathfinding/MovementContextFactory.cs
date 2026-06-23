public static class MovementContextFactory
{
    public static MovementContext FromUnit(HexUnit unit)
    {
        return new MovementContext
        {
            Unit = unit,

            IgnoreOccupants =
                unit.MovementProperties.IgnoreOccupants,

            CanShareTile =
                unit.MovementProperties.CanShareTile,

            IgnoreHeight =
                unit.MovementProperties.IgnoreHeight
        };
    }
}