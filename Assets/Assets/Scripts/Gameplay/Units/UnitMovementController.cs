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

    public void Initialize(
        UnitManager unitManager,
        HexPathRules pathRules)
    {
        this.unitManager = unitManager;
        this.pathRules = pathRules;
    }

    public void MoveUnit(
        HexUnit unit,
        List<HexCell> path)
    {
        if (unit == null)
        {
            return;
        }

        if (path == null ||
            path.Count < 2)
        {
            return;
        }

        if (unit.IsMoving)
        {
            return;
        }

        int pathCost =
            CalculatePathCost(path);

        if (pathCost >
            unit.Stats.CurrentMovementPoints)
        {
            return;
        }

        unit.Stats.ConsumeMovement(
            pathCost);

        StartCoroutine(
            MoveRoutine(
                unit,
                path));
    }

    private int CalculatePathCost(
        IReadOnlyList<HexCell> path)
    {
        int totalCost = 0;

        for (int i = 1; i < path.Count; i++)
        {
            totalCost +=
                pathRules.GetMoveCost(
                    path[i - 1],
                    path[i]);
        }

        return totalCost;
    }

    private IEnumerator MoveRoutine(
        HexUnit unit,
        List<HexCell> path)
    {
        unit.SetMoving(true);

        MovementStarted?.Invoke(unit);

        for (int i = 1; i < path.Count; i++)
        {
            yield return MoveToCell(
                unit,
                path[i]);
        }

        unit.SetMoving(false);

        MovementFinished?.Invoke(unit);
    }

    private IEnumerator MoveToCell(
        HexUnit unit,
        HexCell targetCell)
    {
        Transform transform =
            unit.View.transform;

        float moveSpeed =
            unit.Stats.MoveSpeed;

        Vector3 targetPosition =
            targetCell.WorldPosition +
            Vector3.up * unitManager.UnitHeightOffset;

        while (
            Vector3.Distance(
                transform.position,
                targetPosition)
            > 0.02f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed *
                    Time.deltaTime);

            yield return null;
        }

        transform.position =
            targetPosition;

        unit.SetCell(
            targetCell);
    }
}