using System.Collections.Generic;
using UnityEngine;

public class ActionRangeVisualizer : MonoBehaviour
{
    private HexGridManager gridManager;

    private readonly List<HexCellView>
        rangeViews = new();

    private readonly List<HexCellView>
        validTargetViews = new();

    private readonly List<HexCellView>
        invalidTargetViews = new();

    public void Initialize(
        HexGridManager gridManager)
    {
        this.gridManager =
            gridManager;
    }

    public void ShowRange(
        IEnumerable<HexCell> cells)
    {
        foreach (HexCell cell in cells)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
                continue;

            view.SetActionRange(true);

            rangeViews.Add(view);
        }
    }

    public void ShowValidTargets(
        IEnumerable<HexCell> cells)
    {
        foreach (HexCell cell in cells)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
                continue;

            view.SetValidActionTarget(true);

            validTargetViews.Add(view);
        }
    }

    public void ShowInvalidTargets(
        IEnumerable<HexCell> cells)
    {
        foreach (HexCell cell in cells)
        {
            HexCellView view =
                gridManager.GetView(cell);

            if (view == null)
                continue;

            view.SetInvalidActionTarget(true);

            invalidTargetViews.Add(view);
        }
    }

    public void Clear()
    {
        foreach (HexCellView view in rangeViews)
        {
            view.SetActionRange(false);
        }

        foreach (HexCellView view in validTargetViews)
        {
            view.SetValidActionTarget(false);
        }

        foreach (HexCellView view in invalidTargetViews)
        {
            view.SetInvalidActionTarget(false);
        }

        rangeViews.Clear();
        validTargetViews.Clear();
        invalidTargetViews.Clear();
    }
}