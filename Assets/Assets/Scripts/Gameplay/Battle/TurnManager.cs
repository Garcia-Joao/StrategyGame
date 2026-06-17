using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameStateMachine stateMachine;

    public void Initialize(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
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

    private void StartTeamTurn()
    {
        Debug.Log("Team Turn Started");

        // aqui você inicia lógica de turno (select unit etc)
        stateMachine.SetState(GameState.UnitSelected);
    }

    private void ExecuteWorldPhase()
    {
        Debug.Log("World Phase Executed");

        stateMachine.SetState(GameState.TeamTurn);
    }
}