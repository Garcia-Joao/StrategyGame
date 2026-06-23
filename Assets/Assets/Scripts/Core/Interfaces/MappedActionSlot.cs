using System;
using UnityEngine.InputSystem;

public class MappedActionSlot : IMappedAction
{
    public event Action<int> SlotPressed;

    private readonly InputAction inputAction;

    private readonly int slot;

    public MappedActionSlot(
        InputAction inputAction,
        int slot)
    {
        this.inputAction = inputAction;
        this.slot = slot;
    }

    public void BindActions()
    {
        inputAction.performed += OnPerformed;
    }

    public void UnbindActions()
    {
        inputAction.performed -= OnPerformed;
    }

    private void OnPerformed(
        InputAction.CallbackContext context)
    {
        SlotPressed?.Invoke(slot);
    }
}