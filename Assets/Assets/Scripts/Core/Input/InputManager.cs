using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-80)]
public class InputManager : MonoBehaviour
{
    #region Fields

    private GameInputActions inputActions;

    private readonly List<IMappedAction> mappedInputs = new();

    #endregion

    #region Public Inputs

    public MappedAction<Vector2> CameraMoveAction { get; private set; }

    public MappedAction<Vector2> CameraRotateAction { get; private set; }

    public MappedAction<float> CameraZoomAction { get; private set; }

    public MappedAction<Vector2> MousePositionAction { get; private set; }

    public MappedAction<float> MouseSelectAction { get; private set; }
    public MappedAction<float> MouseCancelAction { get; private set; }

    public MappedAction<float> EndTurnAction { get; private set; }

    public MappedAction<float> Brush1Action { get; private set; }
    public MappedAction<float> Brush2Action { get; private set; }
    public MappedAction<float> Brush3Action { get; private set; }
    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        inputActions = new GameInputActions();
        RegisterInputs();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        foreach (IMappedAction input in mappedInputs)
        {
            input.BindActions();
        }
    }

    private void OnDisable()
    {
        foreach (IMappedAction input in mappedInputs)
        {
            input.UnbindActions();
        }

        inputActions.Disable();
    }

    #endregion

    #region Registration

    private void RegisterInputs()
    {
        CameraMoveAction =
            RegisterInput<Vector2>(
                inputActions.Gameplay.Camera_Movement);

        CameraRotateAction =
            RegisterInput<Vector2>(
                inputActions.Gameplay.Camera_Rotation);

        CameraZoomAction =
            RegisterInput<float>(
                inputActions.Gameplay.Camera_Zoom);

        MousePositionAction =
            RegisterInput<Vector2>(
                inputActions.Gameplay.Mouse_Position);

        MouseSelectAction =
            RegisterInput<float>(
                inputActions.Gameplay.Select);

        MouseCancelAction =
            RegisterInput<float>(
                inputActions.Gameplay.Cancel);


        EndTurnAction = 
            RegisterInput<float>(
                inputActions.Gameplay.End_Turn);

        Brush1Action =
            RegisterInput<float>(
                inputActions.Gameplay.Brush_1);

        Brush2Action =
            RegisterInput<float>(
                inputActions.Gameplay.Brush_2);

        Brush3Action =
            RegisterInput<float>(
                inputActions.Gameplay.Brush_3);
    }

    private MappedAction<T> RegisterInput<T>(
        InputAction inputAction)
        where T : struct
    {
        MappedAction<T> mappedAction = new(inputAction);

        mappedInputs.Add(mappedAction);

        return mappedAction;
    }

    #endregion
}