using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitInteractionController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private HexPathRules pathRules;

    public HexPathRules PathRules => pathRules;

    private InputManager inputManager;
    private HexGridManager gridManager;
    private UnitManager unitManager;
    private HexSelectionManager cellSelection;
    private UnitSelectionManager unitSelection;
    private MovementRangeVisualizer rangeVisualizer;
    private PathPreviewSystem pathPreview;
    private UnitMovementController movementController;
    private GameStateMachine stateMachine;

    private MovementRangeCalculator rangeCalculator;
    private HexPathfinder pathfinder;
    private HexRaycaster raycaster;

    private HashSet<HexCell> reachableCells = new();

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void Initialize(
        InputManager inputManager,
        HexGridManager gridManager,
        HexSelectionManager cellSelection,
        UnitSelectionManager unitSelection,
        MovementRangeVisualizer rangeVisualizer,
        PathPreviewSystem pathPreview,
        UnitMovementController movementController,
        UnitManager unitManager,
        GameStateMachine stateMachine)
    {
        this.inputManager = inputManager;
        this.gridManager = gridManager;
        this.cellSelection = cellSelection;
        this.unitSelection = unitSelection;
        this.rangeVisualizer = rangeVisualizer;
        this.pathPreview = pathPreview;
        this.movementController = movementController;
        this.unitManager = unitManager;
        this.stateMachine = stateMachine;

        pathfinder = new HexPathfinder(gridManager.Grid, pathRules);
        rangeCalculator = new MovementRangeCalculator(gridManager.Grid, pathRules);
        raycaster = new HexRaycaster(targetCamera);

        inputManager.MouseSelectAction.ActionStarted += OnLeftClick;

        cellSelection.CellHovered += OnCellHovered;

        unitSelection.UnitSelected += OnUnitSelected;
        unitSelection.UnitDeselected += OnUnitDeselected;

        movementController.MovementStarted += OnMovementStarted;
        movementController.MovementFinished += OnMovementFinished;

        stateMachine.StateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        if (inputManager != null)
            inputManager.MouseSelectAction.ActionStarted -= OnLeftClick;

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

        if (stateMachine != null)
            stateMachine.StateChanged -= OnStateChanged;
    }

    // ---------------- INPUT ----------------

    private void OnLeftClick(float _)
    {
        Vector2 mouse = inputManager.MousePositionAction.GetCurrentValue();

        HexUnitView unitView = raycaster.RaycastUnit(mouse);

        if (unitView != null)
        {
            HexUnit unit = unitView.Unit;

            if (unit.Owner != unitManager.LocalPlayer)
                return;

            if (unitSelection.SelectedUnit != unit)
                return;

            return;
        }

        HexCellView cellView = raycaster.RaycastCell(mouse);

        if (cellView == null)
            return;

        HexUnit selected = unitSelection.SelectedUnit;

        if (selected == null || selected.IsMoving)
            return;

        if (!reachableCells.Contains(cellView.Cell))
            return;

        var path = pathfinder.FindPath(selected.CurrentCell, cellView.Cell);

        if (path == null)
            return;

        movementController.MoveUnit(selected, path);
    }

    // ---------------- SELECTION ----------------

    private void OnCellHovered(HexCell hoveredCell)
    {
        pathPreview.Clear();

        HexUnit selectedUnit = unitSelection.SelectedUnit;

        if (selectedUnit == null)
            return;

        if (selectedUnit.IsMoving)
            return;

        if (hoveredCell == null)
            return;

        List<HexCell> path =
            pathfinder.FindPath(
                selectedUnit.CurrentCell,
                hoveredCell);

        if (path == null)
            return;

        bool valid = reachableCells.Contains(hoveredCell);

        pathPreview.ShowPath(path, valid);
    }

    private void OnUnitSelected(HexUnit unit)
    {
        if (unit == null)
            return;

        unit.Stats.MovementPointsChanged += OnMovementChanged;

        reachableCells = rangeCalculator.GetReachableCells(unit);
        rangeVisualizer.ShowRange(reachableCells);
    }

    private void OnUnitDeselected(HexUnit unit)
    {
        if (unit != null)
            unit.Stats.MovementPointsChanged -= OnMovementChanged;

        pathPreview.Clear();
        rangeVisualizer.Clear();
        reachableCells.Clear();
    }

    private void OnMovementChanged()
    {
        HexUnit unit = unitSelection.SelectedUnit;

        if (unit == null)
            return;

        reachableCells = rangeCalculator.GetReachableCells(unit);
        rangeVisualizer.ShowRange(reachableCells);
    }

    // ---------------- MOVEMENT ----------------

    private void OnMovementStarted(HexUnit unit)
    {
        pathPreview.Clear();
        rangeVisualizer.Clear();
        reachableCells.Clear();
    }

    private void OnMovementFinished(HexUnit unit)
    {
        if (unitSelection.SelectedUnit != unit)
            return;

        reachableCells = rangeCalculator.GetReachableCells(unit);
        rangeVisualizer.ShowRange(reachableCells);
    }

    // ---------------- STATE MACHINE ----------------

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.TeamTurn)
        {
            RefreshSelection();
        }

        if (state == GameState.UnitSelected)
        {
            RefreshSelection();
        }

        if (state == GameState.UnitMoving)
        {
            rangeVisualizer.Clear();
        }
    }

    public void RefreshSelection()
    {
        OnMovementChanged();
    }
}