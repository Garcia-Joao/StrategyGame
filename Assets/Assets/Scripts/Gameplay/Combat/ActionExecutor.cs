using System.Collections.Generic;
using System.Linq;

public class ActionExecutor
{
    private readonly HexGrid grid;

    public ActionExecutor(
        HexGrid grid)
    {
        this.grid = grid;
    }

    public bool TryExecute(
        UnitActionDefinition action,
        HexUnit source,
        HexCell target)
    {
        if (action == null)
            return false;

        if (source == null)
            return false;

        if (target == null)
            return false;

        if (!CanPayCost(action, source))
            return false;

        if (!ValidateRange(action, source, target))
            return false;

        if (!ValidateTarget(action, source, target))
            return false;

        ActionContext context =
            BuildContext(
                action,
                source,
                target);

        ConsumeCost(
            action,
            source);

        foreach (ActionEffect effect in action.Effects)
        {
            effect.Apply(context);
        }

        return true;
    }

    private bool CanPayCost(
        UnitActionDefinition action,
        HexUnit source)
    {
        ActionCost cost =
            action.Cost;

        if (!source.ActionStats.CanPay(
                cost.ActionPoints,
                cost.QuickActionPoints))
        {
            return false;
        }

        if (source.Resources.Mana.Current < cost.Mana)
        {
            return false;
        }

        if (source.Resources.Health.Current <= cost.Health)
        {
            return false;
        }

        if (source.Resources.Movement.Current <
            cost.MovementPoints)
        {
            return false;
        }

        return true;
    }

    private void ConsumeCost(
        UnitActionDefinition action,
        HexUnit source)
    {
        ActionCost cost =
            action.Cost;

        source.ActionStats.Consume(
            cost.ActionPoints,
            cost.QuickActionPoints);

        source.Resources.ConsumeMana(
            cost.Mana);

        source.Resources.ConsumeHealth(
            cost.Health);

        source.Resources.Movement.Consume(
    cost.MovementPoints);
    }

    private bool ValidateRange(
        UnitActionDefinition action,
        HexUnit source,
        HexCell target)
    {
        int distance =
            source.CurrentCell.DistanceTo(
                target);

        return distance >= action.Range.MinRange &&
               distance <= action.Range.MaxRange;
    }

    private bool ValidateTarget(
        UnitActionDefinition action,
        HexUnit source,
        HexCell target)
    {
        TargetRules rules =
            action.TargetRules;

        HexUnit targetUnit =
            target.OccupyingUnit as HexUnit;

        if (rules.RequireOccupiedTile &&
            targetUnit == null)
        {
            return false;
        }

        if (!rules.AllowEmptyTile &&
            targetUnit == null)
        {
            return false;
        }

        if (targetUnit == null)
        {
            return true;
        }

        bool isEnemy =
            targetUnit.Team != source.Team;

        bool isAlly =
            targetUnit.Team == source.Team;

        switch (rules.AllowedTargets)
        {
            case TargetType.Neutral:
                return true;

            case TargetType.Enemy:
                return isEnemy;

            case TargetType.Ally:
                return isAlly;

            case TargetType.Self:
                return targetUnit == source;

            default:
                return false;
        }
    }

    private ActionContext BuildContext(
        UnitActionDefinition action,
        HexUnit source,
        HexCell target)
    {
        List<HexCell> affectedCells =
            action.Area
                .GetAffectedCells(
                    grid,
                    target)
                .ToList();

        List<HexUnit> affectedUnits =
            affectedCells
                .Select(x => x.OccupyingUnit)
                .OfType<HexUnit>()
                .ToList();

        return new ActionContext
        {
            Action = action,
            Source = source,
            TargetCell = target,
            AffectedCells = affectedCells,
            AffectedUnits = affectedUnits
        };
    }
}

public static class ActionUsageUtility
{
    public static bool CanUseAgain(
        HexUnit unit,
        UnitActionDefinition action)
    {
        ActionCost cost =
            action.Cost;

        bool hasActions =
            unit.ActionStats.CanPay(
                cost.ActionPoints,
                cost.QuickActionPoints);

        bool hasMovement =
    unit.Resources.Movement.Current >=
    cost.MovementPoints;

        return hasActions &&
               hasMovement;
    }
}