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

        if (pathCost > unit.Resources.Movement.Current)
            return;

        unit.Resources.Movement.Consume(pathCost);

        stateMachine.SetState(GameState.UnitMoving);

        StartCoroutine(MoveRoutine(unit, path));
    }

    private int CalculatePathCost(IReadOnlyList<HexCell> path)
    {
        int totalCost = 0;

        for (int i = 1; i < path.Count; i++)
            totalCost += pathRules.GetMoveCost(path[i - 1], path[i]);

        return totalCost;
    }

    private IEnumerator MoveRoutine(HexUnit unit, List<HexCell> path)
    {
        unit.SetMoving(true);
        MovementStarted?.Invoke(unit);

        Transform t = unit.View.transform;

        float moveSpeed = unit.Stats.MoveSpeed;

        int index = 1;

        Vector3 currentTarget =
            path[index].WorldPosition + Vector3.up * unitManager.UnitHeightOffset;

        while (index < path.Count)
        {
            Vector3 position = t.position;

            // direção dinâmica (sempre recalculada)
            Vector3 direction = (currentTarget - position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(direction, Vector3.up);

                t.rotation = Quaternion.Slerp(
                    t.rotation,
                    targetRotation,
                    Time.deltaTime * 12f
                );
            }

            // move continuamente
            t.position = Vector3.MoveTowards(
                t.position,
                currentTarget,
                moveSpeed * Time.deltaTime
            );

            // chegou no alvo atual
            if (Vector3.Distance(t.position, currentTarget) < 0.05f)
            {
                t.position = currentTarget;

                unit.SetCell(path[index]);

                index++;

                if (index < path.Count)
                {
                    currentTarget =
                        path[index].WorldPosition + Vector3.up * unitManager.UnitHeightOffset;
                }
            }

            yield return null;
        }

        unit.SetMoving(false);
        MovementFinished?.Invoke(unit);

        stateMachine.SetState(GameState.UnitSelected);
    }

    private IEnumerator MoveToCell(HexUnit unit, HexCell targetCell, Action onComplete)
    {
        // não usado mais (mantido caso você precise depois)
        yield break;
    }
}