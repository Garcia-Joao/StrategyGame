using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexUnit : IHexOccupant
{
    public virtual bool BlocksMovement => true;

    public virtual bool BlocksVision => true;

    public virtual bool IsDestructible => true;

    public string Name { get; }

    public Team Team => Owner.Team;

    public Player Owner { get; }

    public UnitStats Stats { get; private set; }
    public ActionStats ActionStats { get; private set; }

    public event Action<HexUnit> Died;

    public UnitResources Resources
    {
        get;
        private set;
    }

    public MovementProperties MovementProperties { get; private set; }

    public HexCell CurrentCell { get; private set; }

    public HexUnitView View { get; private set; }

    public bool TurnEnded { get; private set; }

    public bool IsMoving { get; private set; }
    private readonly List<UnitActionDefinition> actions;

    public IReadOnlyList<UnitActionDefinition> Actions =>
        actions;

    public event Action TurnStateChanged;

    public readonly Transform transform;


    public HexUnit(
        string name,
        Player owner,
        UnitStats stats,
        MovementProperties movementProperties,
        ActionStats actionStats,
        UnitResources resources,
        IEnumerable<UnitActionDefinition> actions,
        Transform transform)
    {
        Name = name;
        Owner = owner;

        Stats = stats;
        ActionStats = actionStats;
        Resources = resources;

        MovementProperties = movementProperties;

        this.transform = transform;

        Resources.Died += OnDied;

        this.actions = actions.ToList();

        owner?.AddUnit(this);
    }

    private void OnDied()
    {
        CurrentCell?.ClearOccupant();

        Died?.Invoke(this);
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

        Resources.Movement.Reset();

        ActionStats.Reset();

        TurnStateChanged?.Invoke();
    }
}