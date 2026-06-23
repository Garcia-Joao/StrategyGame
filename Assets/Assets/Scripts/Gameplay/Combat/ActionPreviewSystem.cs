using System.Collections.Generic;
using UnityEngine;

public class ActionPreviewSystem : MonoBehaviour
{
    private readonly List<HexCellView>
        activeViews = new();

    private HexGridManager gridManager;

    public void Initialize(
        HexGridManager gridManager)
    {
        this.gridManager =
            gridManager;
    }

    public void ShowPreview(
        IEnumerable<HexCell> affectedCells,
        bool isValid)
    {
        Clear();

        foreach (HexCell cell in affectedCells)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
                continue;

            view.SetActionPreview(
                true,
                isValid);

            activeViews.Add(view);
        }
    }

    public void Clear()
    {
        foreach (HexCellView view
                 in activeViews)
        {
            view.SetActionPreview(
                false,
                false);
        }

        activeViews.Clear();
    }
}