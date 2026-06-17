using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    public event Action<GameState> StateChanged;

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        Debug.Log($"STATE → {newState}");

        StateChanged?.Invoke(newState);
    }

    public bool Is(GameState state)
    {
        return CurrentState == state;
    }
}