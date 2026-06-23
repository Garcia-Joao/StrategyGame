using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static ActionRangeCalculator;

public class UnitInteractionController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private HexPathRules pathRules;

    [SerializeField] private UnitInfoView infoView;

    public HexPathRules PathRules => pathRules;

    private InputManager inputManager;
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
    private TurnManager turnManager;
    private ActionSelectionManager actionSelectionManager;
    private ActionRangeVisualizer actionRangeVisualizer;
    private ActionPreviewSystem actionPreviewSystem;
    private ActionRangeCalculator actionRangeCalculator;
    private ActionExecutor actionExecutor;

    private HashSet<HexCell> actionRangeCells = new();

    private HashSet<HexCell> validTargetCells = new();

    private HexGridManager gridManager;

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
        GameStateMachine stateMachine,
        TurnManager turnManager,
        ActionSelectionManager actionSelectionManager,
        ActionRangeVisualizer actionRangeVisualizer,
        ActionPreviewSystem actionPreviewSystem)
    {
        this.inputManager = inputManager;
        this.turnManager = turnManager;
        this.cellSelection = cellSelection;
        this.unitSelection = unitSelection;
        this.rangeVisualizer = rangeVisualizer;
        this.pathPreview = pathPreview;
        this.movementController = movementController;
        this.unitManager = unitManager;
        this.stateMachine = stateMachine;
        this.actionSelectionManager = actionSelectionManager;

        pathfinder = new HexPathfinder(gridManager.Grid, pathRules);
        rangeCalculator = new MovementRangeCalculator(gridManager.Grid, pathRules);
        raycaster = new HexRaycaster(targetCamera);

        actionSelectionManager.ModeChanged += OnActionModeChanged;

        inputManager.MouseSelectAction.ActionStarted += OnLeftClick;
        inputManager.MouseCancelAction.ActionStarted += OnRightClick;

        cellSelection.CellHovered += OnCellHovered;

        unitSelection.UnitSelected += OnUnitSelected;
        unitSelection.UnitDeselected += OnUnitDeselected;

        movementController.MovementStarted += OnMovementStarted;
        movementController.MovementFinished += OnMovementFinished;

        stateMachine.StateChanged += OnStateChanged;

        this.gridManager = gridManager;

        this.actionRangeVisualizer =
            actionRangeVisualizer;

        this.actionPreviewSystem =
            actionPreviewSystem;

        actionRangeCalculator = new ActionRangeCalculator(gridManager.Grid);
        actionExecutor = new ActionExecutor(gridManager.Grid);
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.MouseSelectAction.ActionStarted -= OnLeftClick;
            inputManager.MouseCancelAction.ActionStarted -= OnRightClick;
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

        if (stateMachine != null)
            stateMachine.StateChanged -= OnStateChanged;

        if (actionSelectionManager != null)
        {
            actionSelectionManager.ModeChanged -= OnActionModeChanged;
        }
    }

    // ---------------- INPUT ----------------

    private void OnLeftClick(float _)
    {

        Debug.Log(
    $"Mode: {actionSelectionManager.CurrentMode} | " +
    $"Action: {actionSelectionManager.SelectedAction?.ActionName}");
        Vector2 mouse = inputManager.MousePositionAction.GetCurrentValue();

        HexUnitView unitView = raycaster.RaycastUnit(mouse);

        if (unitView != null)
        {
            HexUnit unit = unitView.Unit;

            if (unit.TurnEnded)
                return;

            switch (turnManager.TurnMode)
            {
                case TurnMode.Chaos:

                    if (unit.Team != turnManager.CurrentTeam)
                        return;

                    unitSelection.SelectUnit(unit);

                    break;

                case TurnMode.Dex:

                    if (unit != turnManager.CurrentUnit)
                        return;

                    unitSelection.SelectUnit(unit);

                    break;
            }

            return;
        }

        HexCellView cellView = raycaster.RaycastCell(mouse);

        if (cellView == null)
            return;

        HexUnit selected = unitSelection.SelectedUnit;

        if (selected == null || selected.IsMoving)
            return;

        if (actionSelectionManager.IsAbilityMode)
        {
            HandleAbilityClick(
                selected,
                cellView.Cell);

            return;
        }

        if (!reachableCells.Contains(cellView.Cell))
            return;

        MovementContext context =
            MovementContextFactory
                .FromUnit(selected);

        var path =
            pathfinder.FindPath(
                selected.CurrentCell,
                cellView.Cell,
                context);

        if (path == null)
            return;

        movementController.MoveUnit(selected, path);
    }

    private void OnRightClick(float _)
    {
        Vector2 mouse =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        HexUnitView unitView =
            raycaster.RaycastUnit(mouse);

        if (unitView == null)
            return;

        infoView.Show(unitView.Unit);
    }

    // ---------------- SELECTION ----------------

    private void OnCellHovered(HexCell hoveredCell)
    {
        pathPreview.Clear();

        HexUnit selectedUnit = unitSelection.SelectedUnit;

        if (selectedUnit == null)
            return;

        if (actionSelectionManager.IsAbilityMode)
        {
            HandleAbilityHover(
                hoveredCell);

            return;
        }

        if (selectedUnit.IsMoving)
            return;

        if (hoveredCell == null)
            return;

        MovementContext context =
            MovementContextFactory
                .FromUnit(selectedUnit);

        List<HexCell> path =
            pathfinder.FindPath(
                selectedUnit.CurrentCell,
                hoveredCell,
                context);

        if (path == null)
            return;

        bool valid = reachableCells.Contains(hoveredCell);

        pathPreview.ShowPath(path, valid);
    }

    private void OnUnitSelected(HexUnit unit)
    {
        unit.Resources.Movement.Changed -=
    OnMovementChanged;

        unit.Resources.Movement.Changed +=
            OnMovementChanged;

        reachableCells =
            rangeCalculator.GetReachableCells(unit);

        rangeVisualizer.ShowRange(reachableCells);
    }


    private void OnUnitDeselected(
        HexUnit unit)
    {
        if (unit != null)
            unit.Resources.Movement.Changed -=
    OnMovementChanged;

        pathPreview.Clear();
        rangeVisualizer.Clear();

        actionPreviewSystem.Clear();
        actionRangeVisualizer.Clear();

        reachableCells.Clear();
        actionRangeCells.Clear();
    }

    private void OnMovementChanged()
    {
        if (actionSelectionManager.IsAbilityMode)
            return;

        HexUnit unit =
            unitSelection.SelectedUnit;

        if (unit == null)
            return;

        reachableCells =
            rangeCalculator.GetReachableCells(unit);

        rangeVisualizer.ShowRange(reachableCells);
    }

    // ---------------- MOVEMENT ----------------

    private void OnMovementStarted(HexUnit unit)
    {
        pathPreview.Clear();
        rangeVisualizer.Clear();
        reachableCells.Clear();
        actionPreviewSystem.Clear()
        ; actionRangeVisualizer.Clear();
        actionRangeCells.Clear();
    }

    private void OnMovementFinished(HexUnit unit)
    {
        if (unitSelection.SelectedUnit != unit)
            return;

        reachableCells = rangeCalculator.GetReachableCells(unit);
        rangeVisualizer.ShowRange(reachableCells);
    }

    private void OnActionModeChanged(
        PlayerActionMode mode)
    {
        if (mode == PlayerActionMode.Ability)
        {
            pathPreview.Clear();
            rangeVisualizer.Clear();

            actionRangeVisualizer.Clear();

            HexUnit selected =
                unitSelection.SelectedUnit;

            if (selected == null)
                return;

            UnitActionDefinition action =
                actionSelectionManager.SelectedAction;

            if (action == null)
                return;

            actionRangeCells =
                actionRangeCalculator.GetCellsInRange(
                    selected.CurrentCell,
                    action.Range);

            validTargetCells =
                actionRangeCells
                    .Where(cell =>
                        ActionTargetValidator.IsValidTarget(
                            selected,
                            cell,
                            action.TargetRules))
                    .ToHashSet();

            HashSet<HexCell> invalidTargetCells =
                actionRangeCells
                    .Except(validTargetCells)
                    .ToHashSet();

            actionRangeVisualizer.ShowRange(
                actionRangeCells);

            actionRangeVisualizer.ShowValidTargets(
                validTargetCells);

            actionRangeVisualizer.ShowInvalidTargets(
                invalidTargetCells);
        }
        else
        {
            actionPreviewSystem.Clear();

            actionRangeVisualizer.Clear();

            actionRangeCells.Clear();
            validTargetCells.Clear();

            RefreshSelection();
        }
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

    private void RefreshSelection()
    {

        if (actionSelectionManager.IsAbilityMode)
            return;

        OnMovementChanged();
    }

    private void HandleAbilityClick(
        HexUnit source,
        HexCell targetCell)
    {
        UnitActionDefinition action =
            actionSelectionManager.SelectedAction;

        if (action == null)
            return;

        if (!actionRangeCells.Contains(targetCell))
            return;

        bool executed =
            actionExecutor.TryExecute(
                action,
                source,
                targetCell);

        if (!executed)
            return;

        bool canUseAgain =
            ActionUsageUtility.CanUseAgain(
                source,
                action);

        if (!canUseAgain)
        {
            actionSelectionManager.ClearAction();
            return;
        }

        // recalcula alcance caso AP/movimento tenham mudado
        actionRangeCells =
            actionRangeCalculator.GetCellsInRange(
                source.CurrentCell,
                action.Range);

        actionRangeVisualizer.ShowRange(
            actionRangeCells);
    }

    private void HandleAbilityHover(
        HexCell hoveredCell)
    {
        actionPreviewSystem.Clear();

        if (hoveredCell == null)
            return;

        UnitActionDefinition action =
            actionSelectionManager.SelectedAction;

        if (action == null)
            return;

        if (!actionRangeCells.Contains(
                hoveredCell))
        {
            return;
        }

        bool isValidTarget =
            validTargetCells.Contains(
                hoveredCell);

        IEnumerable<HexCell> affectedCells =
            action.Area.GetAffectedCells(
                gridManager.Grid,
                hoveredCell);

        actionPreviewSystem.ShowPreview(
            affectedCells,
            isValidTarget);
    }
}