using System.Collections.Generic;
using System.Linq;

public static class AITreeFactory
{
    public static BTNode Create(
        HexUnit unit,
        UnitManager unitManager,
        HexPathfinder pathfinder,
        HexPathRules pathRules,
        UnitMovementController movementController,
        ActionExecutor actionExecutor)
    {
        return new Selector(new List<BTNode>
        {
            new ActionNode(() =>
            {
if (unit == null || unit.CurrentCell == null)
    return BTState.Failure;

                var target = FindClosestEnemy(unit, unitManager);
                if (target == null)
                    return BTState.Failure;

                var attack = unit.Actions?
    .FirstOrDefault(a =>
        a.ActionTypes != null &&
        a.ActionTypes.Contains(ActionType.Attack));

                if (attack == null)
                    return BTState.Failure;

                int dist = unit.CurrentCell.DistanceTo(target.CurrentCell);

                if (dist < attack.Range.MinRange || dist > attack.Range.MaxRange)
                    return BTState.Failure;

                actionExecutor.TryExecute(attack, unit, target.CurrentCell);

                return BTState.Success;
            }),

            new ActionNode(() =>
            {
                var target = FindClosestEnemy(unit, unitManager);
                if (target == null)
                    return BTState.Failure;

                var path = pathfinder.FindPath(
                    unit.CurrentCell,
                    target.CurrentCell,
                    MovementContextFactory.FromUnit(unit));

                if (path == null || path.Count < 2)
                    return BTState.Failure;

                movementController.MoveUnit(unit, path);

                return BTState.Success;
            })
        });
    }

    private static HexUnit FindClosestEnemy(HexUnit unit, UnitManager unitManager)
    {
        return unitManager.Units
            .Where(u => u.Team != unit.Team)
            .OrderBy(u => unit.CurrentCell.DistanceTo(u.CurrentCell))
            .FirstOrDefault();
    }
}