using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
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

    public MappedAction<float> MouseLeftClickAction { get; private set; }

    public MappedAction<float> Brush1Action { get; private set; }
    public MappedAction<float> Brush2Action { get; private set; }
    public MappedAction<float> Brush3Action { get; private set; }
    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        ServiceLocator.Register(this);

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

        MouseLeftClickAction =
            RegisterInput<float>(
                inputActions.Gameplay.Mouse_LClicked);

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