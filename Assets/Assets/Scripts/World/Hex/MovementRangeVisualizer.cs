using System.Collections.Generic;
using UnityEngine;

public class MovementRangeVisualizer : MonoBehaviour
{
    private readonly List<HexCellView>
        activeViews = new();

    private HexGridManager gridManager;

    public void Initialize(HexGridManager gridManager)
    {
        this.gridManager = gridManager;

        Debug.Assert(
            gridManager != null,
            "GridManager NULL");
    }

    public void ShowRange(
        IEnumerable<HexCell> cells)
    {
        Clear();

        foreach (HexCell cell in cells)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
            {
                continue;
            }

            view.SetMoveRange(true);

            activeViews.Add(view);
        }
    }

    public void Clear()
    {
        foreach (HexCellView view
                 in activeViews)
        {
            view.SetMoveRange(false);
        }

        activeViews.Clear();
    }
}