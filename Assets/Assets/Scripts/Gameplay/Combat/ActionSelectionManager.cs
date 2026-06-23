using System;
using UnityEngine;

public class ActionSelectionManager : MonoBehaviour
{
    public PlayerActionMode CurrentMode { get; private set; }
        = PlayerActionMode.Movement;

    public event Action<PlayerActionMode> ModeChanged;
    public event Action ActionCleared;

    public bool IsMovementMode => CurrentMode == PlayerActionMode.Movement;

    public bool IsAbilityMode => CurrentMode == PlayerActionMode.Ability;

    public UnitActionDefinition SelectedAction
    {
        get;
        private set;
    }

    public void EnterMovementMode()
    {
        if (CurrentMode == PlayerActionMode.Movement)
            return;

        CurrentMode = PlayerActionMode.Movement;

        ModeChanged?.Invoke(CurrentMode);
    }

    public void EnterAbilityMode()
    {
        if (CurrentMode == PlayerActionMode.Ability)
            return;

        CurrentMode = PlayerActionMode.Ability;

        ModeChanged?.Invoke(CurrentMode);
    }

    public void SelectAction(UnitActionDefinition action)
    {
        SelectedAction = action;

        EnterAbilityMode();
    }

    public void ClearAction()
    {
        SelectedAction = null;

        EnterMovementMode();

        ActionCleared?.Invoke();
    }
}