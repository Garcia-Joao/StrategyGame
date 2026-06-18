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

    public void ShowRange(IEnumerable<HexCell> cells)
    {
        //Debug.Log("ShowRange called");

        Clear();

        int count = 0;

        foreach (HexCell cell in cells)
        {
            count++;

            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
                continue;

            view.SetMoveRange(true);

            activeViews.Add(view);
        }

        //Debug.Log($"Rendered {count} cells");
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