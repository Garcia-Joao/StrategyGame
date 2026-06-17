using System;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public bool HasSelection => SelectedUnit != null;

    public HexUnit SelectedUnit
    {
        get;
        private set;
    }

    public event Action<HexUnit> UnitSelected;
    public event Action<HexUnit> UnitDeselected;

    private UnitManager unitManager;
    private MovementRangeVisualizer movementRangeVisualizer;

    public void Initialize(UnitManager unitManager, MovementRangeVisualizer movementRangeVisualizer)
    {
        this.unitManager = unitManager;
        this.movementRangeVisualizer = movementRangeVisualizer;
    }


    public void SelectUnit(HexUnit unit)
    {
        if (unit == null)
            return;

        if (SelectedUnit == unit)
            return;

        if (SelectedUnit != null)
        {
            UnitDeselected?.Invoke(SelectedUnit);
        }

        SelectedUnit = unit;

        UnitSelected?.Invoke(unit);
    }

    public void ClearSelection()
    {
        if (SelectedUnit == null)
            return;

        HexUnit previous = SelectedUnit;

        SelectedUnit = null;

        UnitDeselected?.Invoke(previous);
    }
}