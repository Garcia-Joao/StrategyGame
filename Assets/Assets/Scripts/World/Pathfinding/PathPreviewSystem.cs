using System.Collections.Generic;
using UnityEngine;

public class PathPreviewSystem : MonoBehaviour
{
    private readonly List<HexCellView>
        activeViews = new();

    private HexGridManager gridManager;

    private int lastPathLength;
    private int pathVersion;

    public void Initialize(
        HexGridManager gridManager)
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
        bool replayAnimation = Mathf.Abs(path.Count - lastPathLength) > 1;

        lastPathLength = path.Count;

        Clear();

        pathVersion++;

        const float stepDelay = 0.03f;

        for (int i = 0; i < path.Count; i++)
        {
            HexCellView view =
                gridManager.GetView(path[i]);

            if (view == null)
                continue;

            view.SetPathWaveIndex(i);

            if (replayAnimation)
            {
                view.SetPathAnimated(
                    valid,
                    i * stepDelay,
                    pathVersion);
            }
            else
            {
                if (valid)
                    view.SetPath(true);
                else
                    view.SetInvalidPath(true);
            }

            activeViews.Add(view);
        }
    }

    public void Clear()
    {
        foreach (HexCellView view in activeViews)
        {
            view.InvalidatePathAnimations();
        }

        activeViews.Clear();
    }
}