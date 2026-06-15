using System;
using UnityEngine;

public class HexSelectionManager : MonoBehaviour
{
    public HexCell HoveredCell { get; private set; }
    public HexCell SelectedCell { get; private set; }

    public event Action<HexCell> CellHovered;
    public event Action<HexCell> CellSelected;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    public void SetHoveredCell(HexCell cell)
    {
        if (HoveredCell == cell)
        {
            return;
        }

        HoveredCell = cell;
        CellHovered?.Invoke(cell);
    }

    public void SelectCell(HexCell cell)
    {
        if (SelectedCell == cell)
        {
            return;
        }

        SelectedCell = cell;
        CellSelected?.Invoke(cell);
    }

    public void ClearSelection()
    {
        SelectedCell = null;
        CellSelected?.Invoke(null);
    }

    public void ClearHover()
    {
        HoveredCell = null;
        CellHovered?.Invoke(null);
    }
}