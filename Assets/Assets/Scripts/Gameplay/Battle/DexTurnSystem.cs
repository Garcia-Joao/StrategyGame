using System;

public class DexTurnSystem : ITurnSystem
{
    public int CurrentRound => throw new NotImplementedException();

    public BattlePhase CurrentPhase => throw new NotImplementedException();

    public Team CurrentTeam => throw new NotImplementedException();

    public HexUnit CurrentUnit => throw new NotImplementedException();

    public event Action<int> RoundStarted;
    public event Action<int> RoundFinished;
    public event Action WorldPhaseStarted;
    public event Action WorldPhaseFinished;
    public event Action<Team> TeamPhaseStarted;
    public event Action<Team> TeamPhaseFinished;
    public event Action<HexUnit> UnitTurnStarted;
    public event Action<HexUnit> UnitTurnFinished;

    public void EndCurrentTurn()
    {
        throw new NotImplementedException();
    }

    public void StartBattle()
    {
        throw new NotImplementedException();
    }
}