using System.Collections.Generic;
using UnityEngine;

public class PathPreviewSystem : MonoBehaviour
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

    public void ShowPath(
        List<HexCell> path,
        bool valid)
    {
        Clear();

        foreach (HexCell cell in path)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
            {
                continue;
            }

            if (valid)
            {
                view.SetPath(true);
            }
            else
            {
                view.SetInvalidPath(true);
            }

            activeViews.Add(view);
        }
    }

    public void Clear()
    {
        foreach (HexCellView view
                 in activeViews)
        {
            view.SetPath(false);

            view.SetInvalidPath(false);
        }

        activeViews.Clear();
    }
}