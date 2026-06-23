using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    public event Action<HexUnit> MovementStarted;
    public event Action<HexUnit> MovementFinished;

    private UnitManager unitManager;
    private HexPathRules pathRules;
    private GameStateMachine stateMachine;

    public void Initialize(
        UnitManager unitManager,
        HexPathRules pathRules,
        GameStateMachine stateMachine)
    {
        this.unitManager = unitManager;
        this.pathRules = pathRules;
        this.stateMachine = stateMachine;
    }

    public void MoveUnit(HexUnit unit, List<HexCell> path)
    {
        if (unit == null || path == null || path.Count < 2)
            return;

        if (unit.IsMoving)
            return;

        int pathCost = CalculatePathCost(path);

        if (pathCost >
            unit.Resources.Movement.Current)
        {
            return;
        }

        unit.Resources.Movement.Consume(
            pathCost);

        stateMachine.SetState(GameState.UnitMoving);

        StartCoroutine(MoveRoutine(unit, path));
    }

    private int CalculatePathCost(IReadOnlyList<HexCell> path)
    {
        int totalCost = 0;

        for (int i = 1; i < path.Count; i++)
        {
            totalCost += pathRules.GetMoveCost(path[i - 1], path[i]);
        }

        return totalCost;
    }

    private IEnumerator MoveRoutine(HexUnit unit, List<HexCell> path)
    {
        unit.SetMoving(true);

        MovementStarted?.Invoke(unit);

        for (int i = 1; i < path.Count; i++)
        {
            yield return MoveToCell(unit, path[i]);
        }

        unit.SetMoving(false);

        MovementFinished?.Invoke(unit);

        stateMachine.SetState(GameState.UnitSelected);
    }

    private IEnumerator MoveToCell(HexUnit unit, HexCell targetCell)
    {
        Transform t = unit.View.transform;

        float moveSpeed = unit.Stats.MoveSpeed;

        Vector3 targetPosition =
            targetCell.WorldPosition + Vector3.up * unitManager.UnitHeightOffset;

        while (Vector3.Distance(t.position, targetPosition) > 0.02f)
        {
            t.position = Vector3.MoveTowards(
                t.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        t.position = targetPosition;
        unit.SetCell(targetCell);
    }
}