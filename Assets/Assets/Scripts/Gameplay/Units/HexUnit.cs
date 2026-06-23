using System;

public class HexUnit : IHexOccupant
{

    public virtual bool BlocksMovement => true;

    public virtual bool BlocksVision => true;

    public virtual bool IsDestructible => true;

    public string Name { get; }

    public Team Team => Owner.Team;

    public Player Owner { get; }

    public UnitStats Stats { get; private set; }
    public ActionStats ActionStats{ get; private set; }

    public MovementProperties MovementProperties { get; private set; }

    public HexCell CurrentCell { get; private set; }

    public HexUnitView View { get; private set; }

    public bool TurnEnded { get; private set; }

    public bool IsMoving { get; private set; }

    public event Action TurnStateChanged;


    public HexUnit(
        string name,
        Player owner,
        UnitStats stats,
        MovementProperties movementProperties,
        ActionStats actionStats)
    {
        Name = name;
        Owner = owner;
        Stats = stats;
        MovementProperties = movementProperties;
        ActionStats = actionStats;

        owner?.AddUnit(this);
    }

    public void SetStats(UnitStats stats)
    {
        Stats = stats;
    }

    public void SetMovementProperties(MovementProperties movementProperties)
    {
        MovementProperties =
            movementProperties;
    }

    public void SetCell(HexCell cell)
    {
        if (CurrentCell != null)
        {
            CurrentCell.SetOccupyingUnit(null);
        }

        CurrentCell = cell;

        if (CurrentCell != null)
        {
            CurrentCell.SetOccupyingUnit(this);
        }
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
        if (TurnEnded)
        {
            return;
        }

        TurnEnded = true;

        TurnStateChanged?.Invoke();
    }

    public void ResetTurn()
    {
        TurnEnded = false;

        Stats.ResetMovement();
        ActionStats.Reset();

        TurnStateChanged?.Invoke();
    }
}