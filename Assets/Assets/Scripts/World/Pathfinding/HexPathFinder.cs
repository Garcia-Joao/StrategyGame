using System.Collections.Generic;
using System.Linq;

public class HexPathfinder
{
    private readonly HexGrid grid;
    private readonly HexPathRules rules;

    public HexPathfinder(
        HexGrid grid,
        HexPathRules rules)
    {
        this.grid = grid;
        this.rules = rules;
    }

    public List<HexCell> FindPath(
        HexCell start,
        HexCell goal)
    {
        Dictionary<HexCell, HexPathNode> nodes = new();

        List<HexPathNode> openSet = new();

        HashSet<HexPathNode> closedSet = new();

        HexPathNode startNode =
            GetOrCreateNode(start, nodes);

        startNode.GCost = 0;

        startNode.HCost =
            CalculateHeuristic(
                start,
                goal);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            HexPathNode current =
                GetLowestCostNode(openSet);

            if (current.Cell == goal)
            {
                return ReconstructPath(current);
            }

            openSet.Remove(current);

            closedSet.Add(current);

            foreach (HexCell neighborCell
                     in grid.GetNeighbors(
                         current.Cell.Coordinate))
            {
                if (!rules.CanMove(
                        current.Cell,
                        neighborCell))
                {
                    continue;
                }

                HexPathNode neighbor =
                    GetOrCreateNode(
                        neighborCell,
                        nodes);

                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                int tentativeGCost =
                    current.GCost +
                    rules.GetMoveCost(
                        current.Cell,
                        neighborCell);

                if (tentativeGCost >=
                    neighbor.GCost)
                {
                    continue;
                }

                neighbor.Parent = current;

                neighbor.GCost = tentativeGCost;

                neighbor.HCost =
                    CalculateHeuristic(
                        neighborCell,
                        goal);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    private HexPathNode GetOrCreateNode(
        HexCell cell,
        Dictionary<HexCell, HexPathNode> nodes)
    {
        if (!nodes.TryGetValue(
                cell,
                out HexPathNode node))
        {
            node = new HexPathNode(cell);

            nodes.Add(cell, node);
        }

        return node;
    }

    private int CalculateHeuristic(
        HexCell from,
        HexCell to)
    {
        return from.Coordinate.DistanceTo(
            to.Coordinate);
    }

    private HexPathNode GetLowestCostNode(
        List<HexPathNode> openSet)
    {
        HexPathNode bestNode =
            openSet[0];

        for (int i = 1;
             i < openSet.Count;
             i++)
        {
            HexPathNode node =
                openSet[i];

            if (node.FCost <
                bestNode.FCost)
            {
                bestNode = node;
            }
            else if (
                node.FCost ==
                bestNode.FCost &&
                node.HCost <
                bestNode.HCost)
            {
                bestNode = node;
            }
        }

        return bestNode;
    }

    private List<HexCell> ReconstructPath(
        HexPathNode endNode)
    {
        List<HexCell> path = new();

        HexPathNode current =
            endNode;

        while (current != null)
        {
            path.Add(current.Cell);

            current =
                current.Parent;
        }

        path.Reverse();

        return path;
    }
}