using System.Collections.Generic;
using UnityEngine;

public class MovementRangeVisualizer : MonoBehaviour
{
    private readonly List<HexCellView>
        activeViews = new();

    private HexGridManager gridManager;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        gridManager =
            ServiceLocator
                .Locate<HexGridManager>();
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