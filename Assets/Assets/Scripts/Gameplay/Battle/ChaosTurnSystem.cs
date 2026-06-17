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
    private readonly Player localPlayer;
    private readonly CameraController cameraController;

    public ChaosTurnSystem(UnitManager unitManager, UnitSelectionManager selectionManager, WorldTurnManager worldTurnManager,
                           Player localPlayer, CameraController cameraController)
    {
        this.unitManager = unitManager;
        this.selectionManager = selectionManager;
        this.worldTurnManager = worldTurnManager;

        this.localPlayer = localPlayer;
        this.cameraController = cameraController;
    }

    public void StartBattle()
    {
        CurrentRound = 1;

        StartRound();
    }

    private void StartRound()
    {
        CurrentPhase =
            BattlePhase.World;

        RoundStarted?.Invoke(
            CurrentRound);

        WorldPhaseStarted?.Invoke();

        WorldPhaseFinished?.Invoke();

        StartTeamPhase(
            Team.Team1);
    }

    private void StartTeamPhase(Team team)
    {
        CurrentPhase =
            BattlePhase.Team;

        CurrentTeam =
            team;

        foreach (HexUnit unit in unitManager.GetUnits(team))
        {
            unit.ResetTurn();

            unit.Stats.ResetMovement();
        }

        TeamPhaseStarted?.Invoke(team);

        SelectNextAvailableUnit();
    }

    public void EndCurrentTurn()
    {
        HexUnit unit =
            selectionManager.SelectedUnit;

        if (unit == null)
        {
            return;
        }

        if (unit.Owner != localPlayer)
        {
            return;
        }

        if (unit.TurnEnded)
        {
            return;
        }

        unit.EndTurn();

        UnitTurnFinished?.Invoke(unit);

        foreach (HexUnit other
                 in unitManager.GetUnits(localPlayer))
        {
            if (!other.TurnEnded)
            {
                selectionManager.SelectUnit(other);

                cameraController.FocusOn(other);

                return;
            }
        }

        CheckPhaseCompletion();
    }

    private void SelectNextAvailableUnit()
    {
        foreach (HexUnit unit
                 in unitManager.GetUnits(localPlayer))
        {
            if (unit.TurnEnded)
            {
                continue;
            }

            selectionManager.SelectUnit(unit);

            cameraController.FocusOn(unit);

            return;
        }
    }

    private void CheckPhaseCompletion()
    {
        foreach (HexUnit unit
                 in unitManager.GetUnits(
                     CurrentTeam))
        {
            if (!unit.TurnEnded)
            {
                return;
            }
        }

        EndCurrentTeamPhase();
    }

    private void EndCurrentTeamPhase()
    {
        TeamPhaseFinished?.Invoke(
            CurrentTeam);

        if (CurrentTeam ==
            Team.Team1)
        {
            StartTeamPhase(
                Team.Team2);

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
}