using System.Collections.Generic;
using UnityEngine;

public class UnitInteractionController : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private HexPathRules pathRules;

    private InputManager inputManager;

    private HexGridManager gridManager;

    private HexSelectionManager cellSelection;

    private UnitSelectionManager unitSelection;

    private MovementRangeVisualizer rangeVisualizer;

    private PathPreviewSystem pathPreview;

    private UnitMovementController movementController;

    private MovementRangeCalculator rangeCalculator;

    private HexPathfinder pathfinder;

    private HexRaycaster raycaster;

    private HashSet<HexCell> reachableCells =
        new();

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    public void Initialize(InputManager inputManager, HexGridManager gridManager, HexSelectionManager cellSelection, UnitSelectionManager unitSelection, 
                           MovementRangeVisualizer rangeVisualizer, PathPreviewSystem pathPreview, UnitMovementController movementController)
    {
        this.inputManager = inputManager;
        this.gridManager = gridManager;
        this.cellSelection = cellSelection;
        this.unitSelection = unitSelection;
        this.rangeVisualizer = rangeVisualizer;
        this.pathPreview = pathPreview;
        this.movementController = movementController;

        pathfinder =
            new HexPathfinder(
                gridManager.Grid,
                pathRules);

        rangeCalculator =
            new MovementRangeCalculator(
                gridManager.Grid,
                pathRules);

        raycaster =
            new HexRaycaster(
                targetCamera);
    }

    private void OnEnable()
    {
        if (inputManager != null)
            inputManager.MouseLeftClickAction.ActionStarted += OnLeftClick;

        if (cellSelection != null)
            cellSelection.CellHovered += OnCellHovered;

        if (unitSelection != null)
            unitSelection.UnitSelected += OnUnitSelected;
    }

    private void OnDisable()
    {
        if (inputManager != null)
            inputManager.MouseLeftClickAction.ActionStarted -= OnLeftClick;

        if (cellSelection != null)
            cellSelection.CellHovered -= OnCellHovered;

        if (unitSelection != null)
            unitSelection.UnitSelected -= OnUnitSelected;
    }

    private void OnLeftClick(float _)
    {
        Vector2 mousePosition =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        HexUnitView unitView = raycaster.RaycastUnit(mousePosition);

        if (unitView != null)
        {
            unitSelection.SelectUnit(unitView.Unit);

            return;
        }

        HexCellView cellView =
            raycaster.RaycastCell(
                mousePosition);

        if (cellView == null)
        {
            return;
        }

        HexUnit selectedUnit =
            unitSelection.SelectedUnit;

        if (selectedUnit == null)
        {
            return;
        }

        if (!reachableCells.Contains(
                cellView.Cell))
        {
            return;
        }

        movementController.MoveUnit(
            selectedUnit,
            cellView.Cell);

        reachableCells =
            rangeCalculator
                .GetReachableCells(
                    selectedUnit);

        rangeVisualizer.ShowRange(
            reachableCells);

        pathPreview.Clear();
    }

    private void OnUnitSelected(
        HexUnit unit)
    {
        Debug.Log("Unit Selected");
        pathPreview.Clear();

        rangeVisualizer.Clear();

        reachableCells.Clear();

        if (unit == null)
        {
            return;
        }

        reachableCells =
            rangeCalculator
                .GetReachableCells(
                    unit);

        rangeVisualizer.ShowRange(
            reachableCells);
    }

    private void OnCellHovered(
        HexCell hoveredCell)
    {
        pathPreview.Clear();

        HexUnit selectedUnit =
            unitSelection.SelectedUnit;

        if (selectedUnit == null)
        {
            return;
        }

        if (hoveredCell == null)
        {
            return;
        }

        List<HexCell> path =
            pathfinder.FindPath(
                selectedUnit.CurrentCell,
                hoveredCell);

        if (path == null)
        {
            return;
        }

        bool valid =
            reachableCells.Contains(
                hoveredCell);

        pathPreview.ShowPath(
            path,
            valid);
    }
}