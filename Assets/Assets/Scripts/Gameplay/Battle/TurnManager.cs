using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameStateMachine stateMachine;
    private ITurnSystem turnSystem;
    private bool battleStarted;

    public Team CurrentTeam => turnSystem.CurrentTeam;

    public HexUnit CurrentUnit => turnSystem.CurrentUnit;

    public TurnMode TurnMode => turnMode;

    [SerializeField] private TurnMode turnMode;

    public void Initialize(
        GameStateMachine stateMachine,
        UnitManager unitManager,
        UnitSelectionManager selectionManager,
        CameraController cameraController,
        UnitInteractionController interactionController,
        WorldTurnManager worldTurnManager)
    {
        this.stateMachine = stateMachine;

        switch (turnMode)
        {
            case TurnMode.Chaos:

                turnSystem =
                    new ChaosTurnSystem(
                        unitManager,
                        selectionManager,
                        worldTurnManager,
                        cameraController,
                        interactionController);

                break;

            case TurnMode.Dex:

                turnSystem =
                    new DexTurnSystem(
                        unitManager,
                        selectionManager,
                        cameraController,
                        worldTurnManager);

                break;
        }

        stateMachine.StateChanged +=
            OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.WorldPhase:
                ExecuteWorldPhase();
                break;

            case GameState.TeamTurn:
                StartTeamTurn();
                break;
        }
    }

    private void StartTeamTurn()
    {
        if (!battleStarted)
        {
            battleStarted = true;

            turnSystem.StartBattle();

            return;
        }
    }

    private void ExecuteWorldPhase()
    {
        stateMachine.SetState(GameState.TeamTurn);
    }

    internal void EndCurrentTurn()
    {
        turnSystem.EndCurrentTurn();
    }
}