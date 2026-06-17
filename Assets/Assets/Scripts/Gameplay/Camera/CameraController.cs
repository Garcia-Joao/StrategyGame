using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private CameraSettings cameraSettings;

    public bool IsRotating { get; private set; }

    private InputManager inputManager;

    private Vector3 currentVelocity;

    private float currentYaw;
    private float targetYaw;

    private float currentZoom;
    private float targetZoom;

    private void Start()
    {
        currentYaw = transform.eulerAngles.y;
        targetYaw = currentYaw;

        currentZoom = 0.5f;
        targetZoom = currentZoom;
    }

    public void Initialize(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputManager.CameraMoveAction.GetCurrentValue();

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        Vector3 desiredVelocity =
            moveDirection * cameraSettings.MoveSpeed;

        float currentAcceleration =
            moveDirection.sqrMagnitude > 0.01f
                ? cameraSettings.Acceleration
                : cameraSettings.Deceleration;

        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            desiredVelocity,
            currentAcceleration * Time.deltaTime
        );

        transform.position += currentVelocity * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector2 rotationInput =
            inputManager
                .CameraRotateAction
                .GetCurrentValue();

        IsRotating =
            rotationInput.sqrMagnitude > 0.001f;

        targetYaw +=
            rotationInput.x *
            cameraSettings.RotationSpeed;

        currentYaw = Mathf.LerpAngle(
            currentYaw,
            targetYaw,
            cameraSettings.RotationSmoothness * Time.deltaTime
        );

        Vector3 rotation =
            transform.eulerAngles;

        rotation.y =
            currentYaw;

        transform.eulerAngles =
            rotation;
    }

    private Coroutine focusRoutine;

    public void FocusOn(Vector3 worldPosition)
    {
        if (focusRoutine != null)
            StopCoroutine(focusRoutine);

        focusRoutine = StartCoroutine(FocusRoutine(worldPosition));
    }

    public void FocusOn(HexUnit unit)
    {
        if (unit == null)
        {
            return;
        }

        FocusOn(unit.CurrentCell.WorldPosition);
    }

    private IEnumerator FocusRoutine(Vector3 targetPosition)
    {
        Vector3 start = transform.position;

        Vector3 end = new Vector3(
            targetPosition.x,
            start.y,
            targetPosition.z
        );

        float duration = 0.25f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(
                start,
                end,
                Mathf.SmoothStep(0f, 1f, t)
            );

            yield return null;
        }

        transform.position = end;
        focusRoutine = null;
    }

    private void HandleZoom()
    {
        float zoomInput =
            inputManager.CameraZoomAction.GetCurrentValue();

        targetZoom -=
            zoomInput *
            cameraSettings.ZoomSpeed;

        targetZoom = Mathf.Clamp01(targetZoom);

        currentZoom = Mathf.Lerp(
            currentZoom,
            targetZoom,
            cameraSettings.ZoomSmoothness * Time.deltaTime
        );

        cameraTransform.localPosition = Vector3.Lerp(
            cameraSettings.ZoomedInLocalPosition,
            cameraSettings.ZoomedOutLocalPosition,
            currentZoom
        );

        float targetPitch = Mathf.Lerp(
            cameraSettings.XRotationAtMin,
            cameraSettings.XRotationAtMax,
            currentZoom
        );

        cameraTransform.localRotation = Quaternion.Euler(
            targetPitch,
            0f,
            0f
        );
    }
}