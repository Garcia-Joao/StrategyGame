public class HexUnit
{
    public string Name { get; }

    public int MovementPoints { get; }

    public HexCell CurrentCell
    {
        get;
        private set;
    }

    public HexUnitView View
    {
        get;
        private set;
    }
    public bool IsMoving
    {
        get;
        private set;
    }

    public HexUnit(
        string name,
        int movementPoints)
    {
        Name = name;
        MovementPoints = movementPoints;
    }

    public void SetCell(
        HexCell cell)
    {
        CurrentCell = cell;
    }

    public void SetView(
        HexUnitView view)
    {
        View = view;
    }

    public void SetMoving(bool value)
    {
        IsMoving = value;
    }
}