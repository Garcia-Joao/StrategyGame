using UnityEngine;

public class TurnInputController : MonoBehaviour
{
    private InputManager inputManager;
    private GameStateMachine stateMachine;

    public void Initialize(
        InputManager inputManager,
        GameStateMachine stateMachine)
    {
        this.inputManager = inputManager;
        this.stateMachine = stateMachine;

        inputManager.EndTurnAction.ActionStarted += OnEndTurnPressed;
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.EndTurnAction.ActionStarted -= OnEndTurnPressed;
        }
    }

    private void OnEndTurnPressed(float _)
    {
        Debug.Log("End Turn Pressed");

        stateMachine.SetState(GameState.WorldPhase);
    }
}