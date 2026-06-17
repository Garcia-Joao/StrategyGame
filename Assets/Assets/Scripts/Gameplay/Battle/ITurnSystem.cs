using System;

public interface ITurnSystem
{
    event Action<int> RoundStarted;
    event Action<int> RoundFinished;

    event Action WorldPhaseStarted;
    event Action WorldPhaseFinished;

    event Action<Team> TeamPhaseStarted;
    event Action<Team> TeamPhaseFinished;

    event Action<HexUnit> UnitTurnStarted;
    event Action<HexUnit> UnitTurnFinished;

    int CurrentRound { get; }

    BattlePhase CurrentPhase { get; }

    Team CurrentTeam { get; }

    HexUnit CurrentUnit { get; }

    void StartBattle();

    void EndCurrentTurn();
}