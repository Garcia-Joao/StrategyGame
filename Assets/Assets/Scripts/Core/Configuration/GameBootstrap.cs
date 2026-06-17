using System.Linq;
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
    [SerializeField] private GameStateMachine stateMachine;

    [Header("Consumers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private HexMouseController hexMouseController;
    [SerializeField] private UnitInteractionController unitInteractionController;
    [SerializeField] private TerrainBrushManager terrainBrushManager;

    [Header("Debug")]
    [SerializeField]
    private bool spawnDebugUnit = true;

    private WorldTurnManager worldTurnManager;

    private void Start()
    {
        stateMachine.SetState(GameState.Initializing);
        worldTurnManager =
            new WorldTurnManager();

        gridManager.Initialize(
            unitManager);

        movementRangeVisualizer.Initialize(
            gridManager);

        pathPreviewSystem.Initialize(
            gridManager);

        cameraController.Initialize(
            inputManager);

        hexMouseController.Initialize(
            inputManager,
            hexSelectionManager);

        terrainBrushManager.Initialize(
            inputManager,
            gridManager,
            hexSelectionManager);

        debugManager.Initialize(
            unitSelectionManager,
            unitManager);

        turnInputController.Initialize(
            inputManager, stateMachine);

        unitSelectionManager.Initialize(
            unitManager,
            movementRangeVisualizer);

        if (spawnDebugUnit)
        {
            stateMachine.SetState(GameState.Spawning);
            SpawnDebugUnits();
        }

        stateMachine.SetState(GameState.WorldPhase);
    }

    private void SpawnDebugUnits()
    {
        Player localPlayer =
            new Player(
                "Player1",
                Team.Team1,
                true);

        unitManager.RegisterPlayer(
            localPlayer);

        unitManager.SetLocalPlayer(localPlayer);

        HexCell spawnCell =
            gridManager.Grid
                .GetAllCells()
                .OrderBy(_ => Random.value)
                .First();

        UnitStats stats =
            new UnitStats
            {
                Strength = 5,
                Dexterity = 5,
                Reflexes = 5,
                Vitality = 5,
                MovementPoints = 8,
                MoveSpeed = 12f
            };

        stats.ResetMovement();

        unitManager.SpawnUnit(
            spawnCell,
            localPlayer,
            stats);
    }
}