using System;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public HexUnit SelectedUnit
    {
        get;
        private set;
    }

    public event Action<HexUnit> UnitSelected;

    public void SelectUnit(HexUnit unit)
    {
        if (SelectedUnit == unit)
        {
            Debug.Log("Unit is Null");
            return;
        }

        Debug.Log("Unit is not Null");
        SelectedUnit = unit;

        UnitSelected?.Invoke(unit);
    }

    public void ClearSelection()
    {
        SelectedUnit = null;

        UnitSelected?.Invoke(null);
    }
}