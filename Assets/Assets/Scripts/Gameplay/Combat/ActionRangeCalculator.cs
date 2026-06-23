using System.Collections.Generic;
using UnityEngine;

public class ActionRangeCalculator
{
    private readonly HexGrid grid;

    public ActionRangeCalculator(
        HexGrid grid)
    {
        this.grid = grid;
    }

    public HashSet<HexCell> GetCellsInRange(
        HexCell origin,
        RangeDefinition range)
    {
        HashSet<HexCell> result =
            new();

        Queue<(HexCell Cell, int Distance)>
            frontier =
                new();

        HashSet<HexCell> visited =
            new();

        frontier.Enqueue(
            (origin, 0));

        visited.Add(origin);

        while (frontier.Count > 0)
        {
            var (cell, distance) =
                frontier.Dequeue();

            if (distance >= range.MinRange &&
                distance <= range.MaxRange)
            {
                result.Add(cell);
            }

            if (distance >= range.MaxRange)
            {
                continue;
            }

            foreach (HexCell neighbor
                     in grid.GetNeighbors(
                         cell.Coordinate))
            {
                if (!visited.Add(neighbor))
                {
                    continue;
                }

                frontier.Enqueue(
                    (neighbor,
                     distance + 1));
            }
        }

        return result;
    }

    public static class ActionTargetValidator
    {
        public static bool IsValidTarget(
            HexUnit source,
            HexCell targetCell,
            TargetRules rules)
        {
            HexUnit target =
                targetCell.OccupyingUnit as HexUnit;

            if (rules.RequireOccupiedTile &&
                target == null)
            {
                return false;
            }

            if (!rules.AllowEmptyTile &&
                target == null)
            {
                return false;
            }

            if (target == null)
            {
                return true;
            }

            bool isEnemy =
                target.Team != source.Team;

            bool isAlly =
                target.Team == source.Team;

            switch (rules.AllowedTargets)
            {
                case TargetType.Enemy:
                    return isEnemy;

                case TargetType.Ally:
                    return isAlly;

                case TargetType.Self:
                    return target == source;

                case TargetType.Neutral:
                    return true;
            }

            return false;
        }
    }
}