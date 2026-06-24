using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameStateMachine stateMachine;
    private ITurnSystem turnSystem;
    private bool battleStarted;
    private bool isAIPhase;
    private AIManager aiManager;

    [SerializeField] private TurnMode turnMode;

    public Team CurrentTeam => turnSystem.CurrentTeam;
    public HexUnit CurrentUnit => turnSystem.CurrentUnit;
    public TurnMode TurnMode => turnMode;

    public void Initialize(
        GameStateMachine stateMachine,
        UnitManager unitManager,
        UnitSelectionManager selectionManager,
        CameraController cameraController,
        UnitInteractionController interactionController,
        WorldTurnManager worldTurnManager,
        AIManager aiManager)
    {
        this.stateMachine = stateMachine;
        this.aiManager = aiManager;

        switch (turnMode)
        {
            case TurnMode.Chaos:
                turnSystem = new ChaosTurnSystem(
                    unitManager,
                    selectionManager,
                    worldTurnManager,
                    cameraController,
                    interactionController);
                break;

            case TurnMode.Dex:
                turnSystem = new DexTurnSystem(
                    unitManager,
                    selectionManager,
                    cameraController,
                    worldTurnManager);
                break;
        }

        stateMachine.StateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        if (stateMachine != null)
            stateMachine.StateChanged -= OnStateChanged;
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

        isAIPhase = turnSystem.CurrentUnit != null &&
                    !turnSystem.CurrentUnit.Owner.IsLocalPlayer;

        if (isAIPhase)
        {
            StartCoroutine(RunAITurn());
        }
    }

    private IEnumerator RunAITurn()
    {
        var aiSystem = aiManager.ExecuteTeamTurn(turnSystem.CurrentTeam);

        yield return aiSystem;

        turnSystem.EndCurrentTurn();
    }

    private IEnumerator RunTurn()
    {
        if (CurrentTeam == Team.Team1)
        {
            // player turn (normal flow)
            yield break;
        }

        // IA turn
        yield return aiManager.ExecuteTeamTurn(CurrentTeam);

        turnSystem.EndCurrentTurn();

        stateMachine.SetState(GameState.TeamTurn);
    }

    private IEnumerator ExecuteAITurn()
    {
        yield return aiManager.ExecuteTeamTurn(CurrentTeam);

        EndCurrentTurn();
    }

    private bool IsAITurn()
    {
        // regra simples (você pode evoluir depois)
        // se não for player local -> AI
        return turnSystem.CurrentTeam != Team.Team1;
    }

    private void ExecuteWorldPhase()
    {
        stateMachine.SetState(GameState.TeamTurn);
    }

    public void EndCurrentTurn()
    {
        turnSystem.EndCurrentTurn();
        stateMachine.SetState(GameState.TeamTurn);
    }
}