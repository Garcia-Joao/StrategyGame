using System;
using UnityEngine;

public class ChaosTurnSystem : ITurnSystem
{
    public event Action<int> RoundStarted;

    public event Action<int> RoundFinished;

    public event Action WorldPhaseStarted;

    public event Action WorldPhaseFinished;

    public event Action<Team> TeamPhaseStarted;

    public event Action<Team> TeamPhaseFinished;

    public event Action<HexUnit> UnitTurnStarted;

    public event Action<HexUnit> UnitTurnFinished;

    public Team CurrentTeam
    {
        get;
        private set;
    }

    public int CurrentRound
    {
        get;
        private set;
    }

    public BattlePhase CurrentPhase
    {
        get;
        private set;
    }

    public HexUnit CurrentUnit
    {
        get;
        private set;
    }

    private readonly WorldTurnManager worldTurnManager;
    private readonly UnitManager unitManager;
    private readonly UnitSelectionManager selectionManager;
    private readonly CameraController cameraController;
    private readonly UnitInteractionController unitInteractionController;

    public ChaosTurnSystem(UnitManager unitManager, UnitSelectionManager selectionManager,
                           WorldTurnManager worldTurnManager, CameraController cameraController,
                           UnitInteractionController unitInteractionController)
    {
        this.unitManager = unitManager;
        this.selectionManager = selectionManager;
        this.worldTurnManager = worldTurnManager;
        this.unitInteractionController = unitInteractionController;
        this.cameraController = cameraController;

        Debug.Log($"Local Player: {unitManager.LocalPlayer?.Name ?? "NULL"}");
    }

    public void StartBattle()
    {
        CurrentRound = 1;

        StartRound();
    }

    private void StartRound()
    {
        CurrentPhase = BattlePhase.World;

        RoundStarted?.Invoke(CurrentRound);

        WorldPhaseStarted?.Invoke();
        WorldPhaseFinished?.Invoke();

        if (!TryStartTeamPhase(Team.Team1))
        {
            if (!TryStartTeamPhase(Team.Team2))
            {
                EndRound();
            }
        }
    }

    private void StartTeamPhase(Team team)
    {
        Debug.Log($"LocalPlayer = {unitManager.LocalPlayer?.Name ?? "NULL"}");

        CurrentPhase = BattlePhase.Team;
        CurrentTeam = team;

        foreach (HexUnit unit in unitManager.GetUnits(team))
        {
            unit.ResetTurn();
        }

        TeamPhaseStarted?.Invoke(team);

        CurrentUnit = null;

        SelectNextAvailableUnit();
    }

    public void EndCurrentTurn()
    {
        if (CurrentUnit == null)
            return;

        if (CurrentUnit.Owner != unitManager.LocalPlayer)
            return;

        if (CurrentUnit.TurnEnded)
            return;

        CurrentUnit.EndTurn();

        UnitTurnFinished?.Invoke(CurrentUnit);

        selectionManager.ClearSelection();

        CurrentUnit = null;

        SelectNextAvailableUnit();
    }


    private void SelectNextAvailableUnit()
    {
        foreach (HexUnit unit in unitManager.GetUnits(CurrentTeam))
        {
            if (unit.Owner != unitManager.LocalPlayer)
                continue;

            if (unit.TurnEnded)
                continue;

            CurrentUnit = unit;

            selectionManager.SelectUnit(unit);

            cameraController.FocusOn(unit);

            UnitTurnStarted?.Invoke(unit);

            return;
        }

        CurrentUnit = null;

        CheckPhaseCompletion();
    }


    private void CheckPhaseCompletion()
    {
        foreach (HexUnit unit in unitManager.GetUnits(CurrentTeam))
        {
            if (!unit.TurnEnded)
                return;
        }

        EndCurrentTeamPhase();
    }

    private void EndCurrentTeamPhase()
    {
        TeamPhaseFinished?.Invoke(CurrentTeam);

        if (CurrentTeam == Team.Team1)
        {
            if (TryStartTeamPhase(Team.Team2))
                return;

            EndRound();
            return;
        }

        EndRound();
    }

    private void EndRound()
    {
        Debug.Log($"Ending Round {CurrentRound}");
        ExecuteWorldPhase();

        RoundFinished?.Invoke(
            CurrentRound);

        CurrentRound++;

        StartRound();
    }

    private void ExecuteWorldPhase()
    {
        WorldPhaseStarted?.Invoke();

        if (worldTurnManager.HasActions)
        {
            worldTurnManager.ExecuteAll();
        }

        WorldPhaseFinished?.Invoke();
    }

    private bool TryStartTeamPhase(Team team)
    {
        if (!unitManager.HasAnyUnit(team))
        {
            Debug.Log($"Skipping {team} - no units alive");
            return false;
        }

        StartTeamPhase(team);
        return true;
    }
}