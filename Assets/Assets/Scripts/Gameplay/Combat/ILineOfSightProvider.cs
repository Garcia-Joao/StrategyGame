public interface ILineOfSightProvider
{
    bool HasLineOfSight(
        HexCell from,
        HexCell to);
}