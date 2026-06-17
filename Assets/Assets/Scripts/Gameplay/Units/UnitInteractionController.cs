using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitInteractionController : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private HexPathRules pathRules;
    public HexPathRules PathRules
    {
        get => pathRules;
    }

    [SerializeField]
    private float dragThreshold = 5f;

    private InputManager inputManager;
    private HexGridManager gridManager;
    private UnitManager unitManager;
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

    private Vector2 rightClickStartPosition;
    private bool isDraggingRightClick;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    public void Initialize(
        InputManager inputManager,
        HexGridManager gridManager,
        HexSelectionManager cellSelection,
        UnitSelectionManager unitSelection,
        MovementRangeVisualizer rangeVisualizer,
        PathPreviewSystem pathPreview,
        UnitMovementController movementController,
        UnitManager unitManager)
    {
        this.inputManager = inputManager;
        this.gridManager = gridManager;
        this.cellSelection = cellSelection;
        this.unitSelection = unitSelection;
        this.rangeVisualizer = rangeVisualizer;
        this.pathPreview = pathPreview;
        this.movementController = movementController;
        this.unitManager = unitManager;

        Debug.Assert(gridManager != null, "GridManager NULL");
        Debug.Assert(gridManager.Grid != null, "Grid NULL");
        Debug.Assert(pathRules != null, "PathRules NULL");

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

        inputManager.MouseSelectAction.ActionStarted += OnLeftClick;

        inputManager.MouseCancelAction.ActionStarted += OnRightMouseStarted;
        inputManager.MouseCancelAction.ActionCanceled += OnRightMouseReleased;

        cellSelection.CellHovered += OnCellHovered;

        unitSelection.UnitSelected += OnUnitSelected;
        unitSelection.UnitDeselected += OnUnitDeselected;

        movementController.MovementStarted += OnMovementStarted;
        movementController.MovementFinished += OnMovementFinished;
    }

    private void Update()
    {
        if (!Mouse.current.rightButton.isPressed)
        {
            return;
        }

        if (isDraggingRightClick)
        {
            return;
        }

        Vector2 currentPosition =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        float distance =
            Vector2.Distance(
                currentPosition,
                rightClickStartPosition);

        if (distance > dragThreshold)
        {
            isDraggingRightClick = true;
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.MouseSelectAction.ActionStarted -= OnLeftClick;

            inputManager.MouseCancelAction.ActionStarted -= OnRightMouseStarted;
            inputManager.MouseCancelAction.ActionCanceled -= OnRightMouseReleased;
        }

        if (cellSelection != null)
        {
            cellSelection.CellHovered -= OnCellHovered;
        }

        if (unitSelection != null)
        {
            unitSelection.UnitSelected -= OnUnitSelected;
            unitSelection.UnitDeselected -= OnUnitDeselected;
        }

        if (movementController != null)
        {
            movementController.MovementStarted -= OnMovementStarted;
            movementController.MovementFinished -= OnMovementFinished;
        }
    }

    private void OnRightMouseStarted(
        float _)
    {
        rightClickStartPosition =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        isDraggingRightClick = false;
    }

    private void OnRightMouseReleased(float _)
    {

    }

    private void OnLeftClick(float _)
    {
        Vector2 mousePosition =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        HexUnitView unitView =
            raycaster.RaycastUnit(
                mousePosition);

        if (unitView != null)
        {
            HexUnit unit =
                unitView.Unit;

            if (unit.Owner != unitManager.LocalPlayer)
            {
                return;
            }

            unitSelection.SelectUnit(
                unit);

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

        if (selectedUnit.IsMoving)
        {
            return;
        }

        if (!reachableCells.Contains(
                cellView.Cell))
        {
            return;
        }

        List<HexCell> path =
            pathfinder.FindPath(
                selectedUnit.CurrentCell,
                cellView.Cell);

        if (path == null)
        {
            return;
        }

        movementController.MoveUnit(
            selectedUnit,
            path);
    }

    private void OnUnitSelected(
        HexUnit unit)
    {
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

    private void OnUnitDeselected(
        HexUnit unit)
    {
        pathPreview.Clear();

        rangeVisualizer.Clear();

        reachableCells.Clear();
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

        if (selectedUnit.IsMoving)
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

    private void OnMovementStarted(
        HexUnit unit)
    {
        pathPreview.Clear();

        rangeVisualizer.Clear();

        reachableCells.Clear();
    }

    private void OnMovementFinished(
        HexUnit unit)
    {
        if (unitSelection.SelectedUnit != unit)
        {
            return;
        }

        reachableCells =
            rangeCalculator
                .GetReachableCells(
                    unit);

        rangeVisualizer.ShowRange(
            reachableCells);

        pathPreview.Clear();
    }
}