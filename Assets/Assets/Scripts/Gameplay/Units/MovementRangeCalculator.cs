using System.Collections.Generic;

public class MovementRangeCalculator
{
    private readonly HexGrid grid;

    private readonly HexPathRules rules;

    public MovementRangeCalculator(
        HexGrid grid,
        HexPathRules rules)
    {
        this.grid = grid;
        this.rules = rules;
    }

    public HashSet<HexCell> GetReachableCells(
        HexUnit unit)
    {
        HashSet<HexCell> visited =
            new();

        Queue<(HexCell Cell, int Cost)>
            frontier =
                new();

        frontier.Enqueue(
            (unit.CurrentCell, 0));

        visited.Add(
            unit.CurrentCell);

        while (frontier.Count > 0)
        {
            (HexCell current, int cost) =
                frontier.Dequeue();

            foreach (HexCell neighbor in grid.GetNeighbors(current.Coordinate))
            {
                if (!rules.CanMove(
                        current,
                        neighbor))
                {
                    continue;
                }

                int nextCost =
                    cost +
                    rules.GetMoveCost(
                        current,
                        neighbor);

                if (nextCost >
                    unit.Stats.CurrentMovementPoints)
                {
                    continue;
                }

                if (visited.Add(
                        neighbor))
                {
                    frontier.Enqueue(
                        (neighbor,
                         nextCost));
                }
            }
        }

        visited.Remove(
            unit.CurrentCell);

        return visited;
    }
}