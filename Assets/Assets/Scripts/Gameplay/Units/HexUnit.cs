public class HexUnit
{
    public string Name { get; }

    public Team Team => Owner.Team;

    public Player Owner { get; }

    public UnitStats Stats { get; private set; }

    public HexCell CurrentCell { get; private set; }

    public HexUnitView View { get; private set; }

    public bool TurnEnded { get; private set; }

    public bool IsMoving { get; private set; }

    public HexUnit(
        string name,
        Player owner,
        UnitStats stats)
    {
        Name = name;
        Owner = owner;
        Stats = stats;

        owner?.AddUnit(this);
    }

    public void SetStats(UnitStats stats)
    {
        Stats = stats;
    }

    public void SetCell(HexCell cell)
    {
        if (CurrentCell != null)
            CurrentCell.SetOccupyingUnit(null);

        CurrentCell = cell;

        if (CurrentCell != null)
            CurrentCell.SetOccupyingUnit(this);
    }

    public void SetView(HexUnitView view)
    {
        View = view;
    }

    public void SetMoving(bool value)
    {
        IsMoving = value;
    }

    public void EndTurn()
    {
        TurnEnded = true;
    }

    public void ResetTurn()
    {
        TurnEnded = false;

        Stats.ResetMovement();
    }
}