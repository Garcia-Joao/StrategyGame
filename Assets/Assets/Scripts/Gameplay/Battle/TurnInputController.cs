using UnityEngine;

public class TurnInputController : MonoBehaviour
{
    private InputManager inputManager;
    private GameStateMachine stateMachine;
    private TurnManager turnManager;

    public void Initialize(
        InputManager inputManager,
        GameStateMachine stateMachine,
        TurnManager turnManager)
    {
        this.inputManager = inputManager;
        this.stateMachine = stateMachine;
        this.turnManager = turnManager;

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
        turnManager.EndCurrentTurn();
    }
}