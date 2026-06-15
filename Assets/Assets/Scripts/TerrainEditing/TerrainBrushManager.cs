using System.Collections.Generic;
using UnityEngine;

public class TerrainBrushManager : MonoBehaviour
{
    private InputManager inputManager;

    private HexGridManager gridManager;

    private HexSelectionManager selectionManager;

    private ITerrainBrush activeBrush;

    private readonly List<HexCellView>
        previewViews = new();

    public bool HasActiveBrush =>
        activeBrush != null;

    private void Start()
    {
        inputManager =
            ServiceLocator.Locate<InputManager>();

        gridManager =
            FindFirstObjectByType<HexGridManager>();

        selectionManager =
            FindFirstObjectByType<HexSelectionManager>();

        inputManager.Brush1Action.ActionStarted +=
            ToggleRaiseBrush;

        inputManager.Brush2Action.ActionStarted +=
            ToggleLowerBrush;

        inputManager.Brush3Action.ActionStarted +=
            ToggleFireballBrush;

        inputManager.MouseLeftClickAction.ActionStarted +=
            ApplyBrush;
    }

    private void Update()
    {
        UpdatePreview();
    }

    private void ToggleRaiseBrush(float _)
    {
        activeBrush =
            activeBrush is RaiseTerrainBrush
                ? null
                : new RaiseTerrainBrush();
    }

    private void ToggleLowerBrush(float _)
    {
        activeBrush =
            activeBrush is LowerTerrainBrush
                ? null
                : new LowerTerrainBrush();
    }

    private void ToggleFireballBrush(float _)
    {
        activeBrush =
            activeBrush is FireballTerrainBrush
                ? null
                : new FireballTerrainBrush();
    }

    private void UpdatePreview()
    {
        foreach (HexCellView view in previewViews)
        {
            view.SetBrushPreview(0f);
        }

        previewViews.Clear();

        if (activeBrush == null)
        {
            return;
        }

        HexCell hovered =
            selectionManager.HoveredCell;

        if (hovered == null)
        {
            return;
        }

        foreach (BrushCell affectedCell in
                 activeBrush.GetAffectedCells(
                     gridManager.Grid,
                     hovered))
        {
            HexCellView view =
                gridManager.GetView(
                    affectedCell.Cell);

            if (view == null)
            {
                continue;
            }

            view.SetBrushPreview(
                affectedCell.Intensity);

            previewViews.Add(view);
        }
    }

    private void ApplyBrush(float _)
    {
        if (activeBrush == null)
        {
            return;
        }

        HexCell hovered =
            selectionManager.HoveredCell;

        if (hovered == null)
        {
            return;
        }

        activeBrush.Apply(
            gridManager.Grid,
            hovered);

        foreach (BrushCell affectedCell in
                 activeBrush.GetAffectedCells(
                     gridManager.Grid,
                     hovered))
        {
            gridManager.RefreshCell(
                affectedCell.Cell);
        }
    }
}