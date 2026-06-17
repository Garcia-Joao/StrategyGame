using UnityEngine;

public class TurnInputController : MonoBehaviour
{
    private InputManager inputManager;

    private TurnManager turnManager;

    public void Initialize(
        InputManager inputManager,
        TurnManager turnManager)
    {
        this.inputManager =
            inputManager;

        this.turnManager =
            turnManager;

        inputManager.EndTurnAction.ActionStarted += OnEndTurnPressed;
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager
                .EndTurnAction
                .ActionStarted -=
                    OnEndTurnPressed;
        }
    }

    private void OnEndTurnPressed(
        float _)
    {
        Debug.Log("End Turn Pressed");
        turnManager.EndCurrentTurn();
    }
}