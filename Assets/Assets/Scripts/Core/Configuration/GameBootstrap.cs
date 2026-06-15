using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameBootstrap : MonoBehaviour
{
    [Header("Services")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private HexGridManager gridManager;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private HexSelectionManager hexSelectionManager;
    [SerializeField] private UnitSelectionManager unitSelectionManager;
    [SerializeField] private MovementRangeVisualizer movementRangeVisualizer;
    [SerializeField] private PathPreviewSystem pathPreviewSystem;
    [SerializeField] private UnitMovementController unitMovementController;

    [Header("Consumers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private HexMouseController hexMouseController;
    [SerializeField] private UnitInteractionController unitInteractionController;
    [SerializeField] private TerrainBrushManager terrainBrushManager;

    private void Awake()
    {
        InjectCameraController();
        InjectHexMouseController();
        InjectUnitInteractionController();
        InjectTerrainBrushManager();

        InjectVisualizers();
        InjectGridManager();
    }

    private void InjectCameraController()
    {
        cameraController.Initialize(
            inputManager);
    }

    private void InjectHexMouseController()
    {
        hexMouseController.Initialize(
            inputManager,
            hexSelectionManager);
    }

    private void InjectUnitInteractionController()
    {
        unitInteractionController.Initialize(
            inputManager,
            gridManager,
            hexSelectionManager,
            unitSelectionManager,
            movementRangeVisualizer,
            pathPreviewSystem,
            unitMovementController);
    }

    private void InjectTerrainBrushManager()
    {
        terrainBrushManager.Initialize(inputManager, gridManager, hexSelectionManager);
    }

    private void InjectVisualizers()
    {
        movementRangeVisualizer.Initialize(
            gridManager);

        pathPreviewSystem.Initialize(
            gridManager);
    }

    private void InjectGridManager()
    {
        gridManager.Initialize(
            unitManager);
    }
}