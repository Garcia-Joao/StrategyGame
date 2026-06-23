using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DexTurnSystem : ITurnSystem
{
    public event Action<int> RoundStarted;
    public event Action<int> RoundFinished;

    public event Action WorldPhaseStarted;
    public event Action WorldPhaseFinished;

    public event Action<Team> TeamPhaseStarted;
    public event Action<Team> TeamPhaseFinished;

    public event Action<HexUnit> UnitTurnStarted;
    public event Action<HexUnit> UnitTurnFinished;

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

    public Team CurrentTeam =>
        CurrentUnit != null
            ? CurrentUnit.Team
            : Team.Team1;

    public HexUnit CurrentUnit
    {
        get;
        private set;
    }

    private readonly UnitManager unitManager;
    private readonly UnitSelectionManager selectionManager;
    private readonly CameraController cameraController;
    private readonly WorldTurnManager worldTurnManager;

    private readonly List<InitiativeEntry>
        initiativeOrder = new();

    private int currentIndex;

    public DexTurnSystem(
        UnitManager unitManager,
        UnitSelectionManager selectionManager,
        CameraController cameraController,
        WorldTurnManager worldTurnManager)
    {
        this.unitManager = unitManager;
        this.selectionManager = selectionManager;
        this.cameraController = cameraController;
        this.worldTurnManager = worldTurnManager;
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

        BuildInitiativeOrder();

        //LogInitiativeOrder(initiativeOrder);

        RoundStarted?.Invoke(CurrentRound);

        StartNextUnit();
    }

    private void BuildInitiativeOrder()
    {
        initiativeOrder.Clear();

        foreach (HexUnit unit in unitManager.Units)
        {
            initiativeOrder.Add(new InitiativeEntry
            {
                Unit = unit,

                Roll = UnityEngine.Random.Range(
                            1,
                            21),

                TieBreaker = UnityEngine.Random.Range(1,21),

                Dexterity = unit.Stats.Dexterity
            });
        }

        initiativeOrder.Sort(CompareInitiative);
    }

    private int CompareInitiative(
        InitiativeEntry a,
        InitiativeEntry b)
    {
        int initiative =
            b.Initiative.CompareTo(
                a.Initiative);

        if (initiative != 0)
        {
            return initiative;
        }

        return b.TieBreaker.CompareTo(
            a.TieBreaker);
    }

    private void StartNextUnit()
    {
        if (currentIndex >= initiativeOrder.Count)
        {
            EndRound();
            return;
        }

        CurrentPhase =
            BattlePhase.Team;

        CurrentUnit =
            initiativeOrder[currentIndex]
                .Unit;

        selectionManager.ClearSelection();

        selectionManager.SelectUnit(
            CurrentUnit);

        cameraController.FocusOn(
            CurrentUnit);

        UnitTurnStarted?.Invoke(
            CurrentUnit);
    }

    public void EndCurrentTurn()
    {
        if (CurrentUnit == null)
        {
            return;
        }

        if (CurrentUnit.TurnEnded)
        {
            return;
        }

        CurrentUnit.EndTurn();

        UnitTurnFinished?.Invoke(
            CurrentUnit);

        selectionManager.ClearSelection();

        currentIndex++;

        StartNextUnit();
    }

    private void EndRound()
    {
        ExecuteWorldPhase();

        RoundFinished?.Invoke(
            CurrentRound);

        CurrentRound++;

        currentIndex = 0;

        StartRound();
    }

    private void ExecuteWorldPhase()
    {
        CurrentPhase =
            BattlePhase.World;

        WorldPhaseStarted?.Invoke();

        if (worldTurnManager.HasActions)
        {
            worldTurnManager.ExecuteAll();
        }

        WorldPhaseFinished?.Invoke();
    }

    private void LogInitiativeOrder(List<InitiativeEntry> order)
    {
        System.Text.StringBuilder sb =
            new();

        sb.AppendLine(
            $"=== ROUND {CurrentRound} INITIATIVE ORDER ===");

        for (int i = 0;
             i < order.Count;
             i++)
        {
            InitiativeEntry entry =
                order[i];

            sb.AppendLine(
                $"{i + 1} - {entry.Unit.Name} " +
                $"[Dex:{entry.Dexterity}, Roll:{entry.Roll}, Total:{entry.Initiative}]");
        }

        Debug.Log(sb.ToString());
    }
}