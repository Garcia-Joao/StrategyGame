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

    [Header("UI")]
    [SerializeField] private ActionBarController actionBarController;

    [Header("Debug")]
    [SerializeField] private bool spawnDebugUnit = true;
    [SerializeField] private bool controlTeam2 = true;

    [SerializeField] private int team1UnitCount = 2;
    [SerializeField] private int team2UnitCount = 2;

    private WorldTurnManager worldTurnManager;

    private void Start()
    {
        stateMachine.SetState(GameState.Initializing);
        worldTurnManager =
            new WorldTurnManager();

        gridManager.Initialize(
            unitManager);

        pathPreviewSystem.Initialize(
            gridManager);

        movementRangeVisualizer.Initialize(
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

        turnInputController.Initialize(inputManager, stateMachine, turnManager);

        unitSelectionManager.Initialize(
            unitManager,
            movementRangeVisualizer);

        unitInteractionController.Initialize(
        inputManager,
        gridManager,
        hexSelectionManager,
        unitSelectionManager,
        movementRangeVisualizer,
        pathPreviewSystem,
        unitMovementController,
        unitManager,
        stateMachine,
        turnManager);

        unitMovementController.Initialize(unitManager, unitInteractionController.PathRules, stateMachine);

        turnManager.Initialize(
            stateMachine,
            unitManager,
            unitSelectionManager,
            cameraController,
            unitInteractionController,
            worldTurnManager);

        actionBarController.Initialize(inputManager);

        if (spawnDebugUnit)
        {
            stateMachine.SetState(GameState.Spawning);
            SpawnDebugUnits();
        }

        stateMachine.SetState(GameState.WorldPhase);


    }

    private void SpawnDebugUnits()
    {
        Player team1 =
            new Player(
                "Player1",
                Team.Team1,
                true);

        Player team2 =
            new Player(
                "Player2",
                Team.Team2,
                controlTeam2);

        unitManager.RegisterPlayer(team1);
        unitManager.RegisterPlayer(team2);

        unitManager.SetLocalPlayer(team1);

        for (int i = 0; i < team1UnitCount; i++)
        {
            SpawnDebugUnit(team1, 8);
        }

        for (int i = 0; i < team2UnitCount; i++)
        {
            SpawnDebugUnit(team2, 8);
        }
    }


    private void SpawnDebugUnit(Player owner, int movement)
    {
        HexCell spawnCell =
            gridManager.Grid
                .GetAllCells()
                .Where(x => x.OccupyingUnit == null)
                .OrderBy(_ => Random.value)
                .First();

        UnitStats stats = new UnitStats
        {
            Strength = 5,
            Dexterity = 5,
            Reflexes = 5,
            Vitality = 5,
            MovementPoints = movement,
            MoveSpeed = 12f
        };

        stats.ResetMovement();

        MovementProperties movementProperties = new MovementProperties
        {
            IgnoreOccupants = false,
            CanShareTile = false,
            IgnoreHeight = false
        };

        string unitName =
            RandomUnitNameGenerator
                .Generate(owner.Team);

        ActionStats actions =
        new ActionStats
        {
            ActionPoints = 2,
            QuickActionPoints = 1
        };

        actions.Reset();

        unitManager.SpawnUnit(
            spawnCell,
            owner,
            unitName,
            stats,
            movementProperties,
            actions
            );
    }
}