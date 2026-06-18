using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameStateMachine stateMachine;

    private ChaosTurnSystem chaosSystem;

    public void Initialize(
        GameStateMachine stateMachine,
        UnitManager unitManager,
        UnitSelectionManager selectionManager,
        CameraController cameraController,
        UnitInteractionController interactionController,
        WorldTurnManager worldTurnManager)
    {
        this.stateMachine = stateMachine;

        chaosSystem = new ChaosTurnSystem(
            unitManager,
            selectionManager,
            worldTurnManager,
            cameraController,
            interactionController);

        stateMachine.StateChanged += OnStateChanged;
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

    private bool battleStarted;

    private void StartTeamTurn()
    {
        //Debug.Log("Team Turn Started");
        if (!battleStarted)
        {
            battleStarted = true;

            chaosSystem.StartBattle();

            return;
        }
    }

    private void ExecuteWorldPhase()
    {
        //Debug.Log("World Phase Executed");

        stateMachine.SetState(GameState.TeamTurn);
    }

    internal void EndCurrentTurn()
    {
        chaosSystem.EndCurrentTurn();
    }
}