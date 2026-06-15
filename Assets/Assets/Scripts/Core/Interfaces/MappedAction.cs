using System;
using UnityEngine.InputSystem;

public class MappedAction<T> : IMappedAction where T : struct
{
    #region Public Actions

    public Action<T> ActionStarted;
    public Action<T> ActionPerformed;
    public Action<T> ActionCanceled;

    #endregion

    #region Private Fields

    private readonly InputAction mappedInput;

    #endregion

    #region Constructor

    public MappedAction(InputAction inputToMap)
    {
        mappedInput = inputToMap;
    }

    #endregion

    #region Binding

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

    #endregion

    #region Input Callbacks

    private void OnStarted(InputAction.CallbackContext context)
    {
        ActionStarted?.Invoke(context.ReadValue<T>());
    }

    private void OnPerformed(InputAction.CallbackContext context)
    {
        ActionPerformed?.Invoke(context.ReadValue<T>());
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        ActionCanceled?.Invoke(context.ReadValue<T>());
    }

    #endregion

    #region Public Methods

    public T GetCurrentValue()
    {
        return mappedInput.ReadValue<T>();
    }

    #endregion
}

public interface IMappedAction
{
    void BindActions();
    void UnbindActions();
}