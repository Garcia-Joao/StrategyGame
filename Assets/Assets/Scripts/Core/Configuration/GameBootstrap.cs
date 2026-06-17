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
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private DebugManager debugManager;
    [SerializeField] private TurnInputController turnInputController;

    private WorldTurnManager worldTurnManager;

    [Header("Consumers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private HexMouseController hexMouseController;
    [SerializeField] private UnitInteractionController unitInteractionController;
    [SerializeField] private TerrainBrushManager terrainBrushManager;


    private void Start()
    {
        worldTurnManager = new WorldTurnManager();

        gridManager.Initialize(unitManager);

        movementRangeVisualizer.Initialize(gridManager);
        pathPreviewSystem.Initialize(gridManager);

        cameraController.Initialize(inputManager);

        hexMouseController.Initialize(
            inputManager,
            hexSelectionManager);

        terrainBrushManager.Initialize(
            inputManager,
            gridManager,
            hexSelectionManager);

        unitInteractionController.Initialize(
            inputManager,
            gridManager,
            hexSelectionManager,
            unitSelectionManager,
            movementRangeVisualizer,
            pathPreviewSystem,
            unitMovementController,
            unitManager);

        unitMovementController.Initialize(unitManager, unitInteractionController.PathRules);

        debugManager.Initialize(unitSelectionManager, unitManager);
        turnManager.Initialize(unitManager, unitSelectionManager, worldTurnManager, cameraController, gridManager);
        turnInputController.Initialize(inputManager, turnManager);
    }
}