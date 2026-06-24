using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager
{
    private readonly UnitManager unitManager;
    private readonly HexPathfinder pathfinder;
    private readonly HexPathRules pathRules;
    private readonly UnitMovementController movementController;
    private readonly ActionExecutor actionExecutor;

    private readonly Dictionary<HexUnit, UnitAIController> controllers = new();

    public AIManager(
        UnitManager unitManager,
        HexPathfinder pathfinder,
        HexPathRules pathRules,
        UnitMovementController movementController,
        ActionExecutor actionExecutor)
    {
        this.unitManager = unitManager;
        this.pathfinder = pathfinder;
        this.pathRules = pathRules;
        this.movementController = movementController;
        this.actionExecutor = actionExecutor;
    }

    public IEnumerator ExecuteTeamTurn(Team team)
    {
        var units = unitManager.GetUnits(team)
            .Where(u => !u.TurnEnded)
            .ToList();

        foreach (var unit in units)
        {
            if (unit.IsMoving)
                yield return new WaitUntil(() => !unit.IsMoving);

            var controller = CreateOrGetAI(unit);
            yield return controller.ExecuteTurn();

            unit.EndTurn(); // 👈 IMPORTANTE
        }
    }

    private UnitAIController CreateOrGetAI(HexUnit unit)
    {
        if (controllers.TryGetValue(unit, out var controller))
            return controller;

        BTNode tree = AITreeFactory.Create(
            unit,
            unitManager,
            pathfinder,
            pathRules,
            movementController,
            actionExecutor);

        var brain = new UnitAIBrain(tree);

        controller = new UnitAIController(unit, brain);

        controllers[unit] = controller;

        return controller;
    }

    private void OnUnitSpawned(HexUnit unit)
    {
        if (unit.Owner.IsLocalPlayer)
            return;

        BTNode tree = AITreeFactory.Create(
            unit,
            unitManager,
            pathfinder,
            pathRules,
            movementController,
            actionExecutor);

        var brain = new UnitAIBrain(tree);

        controllers[unit] = new UnitAIController(unit, brain);
    }

    public void RegisterUnit(HexUnit unit)
    {
        if (controllers.ContainsKey(unit))
            return;

        var tree = AITreeFactory.Create(
            unit,
            unitManager,
            pathfinder,
            pathRules,
            movementController,
            actionExecutor);

        var brain = new UnitAIBrain(tree);

        controllers[unit] = new UnitAIController(unit, brain);
    }
}