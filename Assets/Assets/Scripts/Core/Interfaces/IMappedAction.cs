using System;
using UnityEngine.InputSystem;

public class MappedInputAction : IMappedAction
{
    public Action<InputAction.CallbackContext>
        ActionStarted;

    public Action<InputAction.CallbackContext>
        ActionPerformed;

    public Action<InputAction.CallbackContext>
        ActionCanceled;

    private readonly InputAction mappedInput;

    public MappedInputAction(
        InputAction inputToMap)
    {
        mappedInput = inputToMap;
    }

    public void BindActions()
    {
        mappedInput.started += OnStarted;
        mappedInput.performed += OnPerformed;
        mappedInput.canceled += OnCanceled;
    }

    public void UnbindActions()
    {
        mappedInput.started -= OnStarted;
        mappedInput.performed -= OnPerformed;
        mappedInput.canceled -= OnCanceled;
    }

    private void OnStarted(
        InputAction.CallbackContext context)
    {
        ActionStarted?.Invoke(context);
    }

    private void OnPerformed(
        InputAction.CallbackContext context)
    {
        ActionPerformed?.Invoke(context);
    }

    private void OnCanceled(
        InputAction.CallbackContext context)
    {
        ActionCanceled?.Invoke(context);
    }
}