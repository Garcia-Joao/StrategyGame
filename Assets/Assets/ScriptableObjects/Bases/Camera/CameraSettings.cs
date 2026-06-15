using UnityEngine;

[CreateAssetMenu(
    fileName = "CameraSettings",
    menuName = "Scriptable Objects/Camera/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed = 20f;
    public float Acceleration = 40f;
    public float Deceleration = 60f;

    [Header("Rotation")]
    public float RotationSpeed = 0.25f;
    public float RotationSmoothness = 15f;

    [Header("Zoom")]
    public float ZoomSpeed = 0.15f;
    public float ZoomSmoothness = 10f;

    [Header("Zoom Positions")]
    public Vector3 ZoomedInLocalPosition = new(0f, 4f, -8f);
    public Vector3 ZoomedOutLocalPosition = new(0f, 16f, -32f);

    [Header("Pitch")]
    public float XRotationAtMin = 45f;
    public float XRotationAtMax = 70f;
}