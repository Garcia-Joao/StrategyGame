using System.Collections.Generic;
using UnityEngine;

public class PathPreviewSystem : MonoBehaviour
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