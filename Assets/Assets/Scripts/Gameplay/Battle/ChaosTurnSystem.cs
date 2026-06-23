using System;
using System.Linq;
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

    public HexUnit CurrentUnit => selectionManager.SelectedUnit;

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

        //Debug.Log($"Local Player: {unitManager.LocalPlayer?.Name ?? "NULL"}");
    }

    public void StartBattle()
    {
        CurrentRound = 1;

        StartRound();
    }

    private void StartRound()
    {
        CurrentPhase = BattlePhase.World;

        foreach (HexUnit unit in unitManager.Units)
        {
            unit.ResetTurn();
        }

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
        CurrentPhase = BattlePhase.Team;
        CurrentTeam = team;

        TeamPhaseStarted?.Invoke(team);

        selectionManager.ClearSelection();

        SelectNextAvailableUnit();
    }

    public void EndCurrentTurn()
    {
        HexUnit CurrentUnit = selectionManager.SelectedUnit;

        if (CurrentUnit == null)
        {
            return;
        }

        if (CurrentUnit.TurnEnded)
        {
            return;
        }

        if (CurrentUnit.Team != CurrentTeam)
        {
            return;
        }

        bool canControl =
            CurrentUnit.Owner.IsLocalPlayer;

        if (!canControl)
        {
            return;
        }

        CurrentUnit.EndTurn();

        UnitTurnFinished?.Invoke(CurrentUnit);

        selectionManager.ClearSelection();

        SelectNextAvailableUnit();
    }

    private void SelectNextAvailableUnit()
    {
        bool controlCurrentTeam =
            unitManager.Players.Any(
                p =>
                    p.Team == CurrentTeam &&
                    p.IsLocalPlayer);

        foreach (HexUnit unit in unitManager.GetUnits(CurrentTeam))
        {
            if (unit.TurnEnded)
            {
                continue;
            }

            if (controlCurrentTeam)
            {
                selectionManager.SelectUnit(unit);

                cameraController.FocusOn(unit);
            }

            UnitTurnStarted?.Invoke(unit);

            return;
        }

        selectionManager.ClearSelection();

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
        //Debug.Log($"Ending Round {CurrentRound}");
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
            //Debug.Log($"Skipping {team} - no units alive");
            return false;
        }

        StartTeamPhase(team);
        return true;
    }
}