using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 8f;

    private UnitManager unitManager;

    public void Initialize(UnitManager unitManager)
    {
        this.unitManager = unitManager;
    }

    public void MoveUnit(
        HexUnit unit,
        List<HexCell> path)
    {
        if (unit.IsMoving)
        {
            return;
        }

        StartCoroutine(
            MoveRoutine(
                unit,
                path));
    }

    private IEnumerator MoveRoutine(
        HexUnit unit,
        List<HexCell> path)
    {
        unit.SetMoving(true);

        foreach (HexCell cell in path)
        {
            yield return MoveToCell(
                unit,
                cell);
        }

        unit.SetMoving(false);
    }

    private IEnumerator MoveToCell(
        HexUnit unit,
        HexCell targetCell)
    {
        Transform transform =
            unit.View.transform;

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