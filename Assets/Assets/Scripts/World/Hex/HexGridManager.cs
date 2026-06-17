using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField]
    private int gridRadius = 5;

    [Header("Visual")]
    [SerializeField]
    private GameObject hexCellPrefab;

    [Header("Noise")]
    [SerializeField]
    private float noiseScale = 0.15f;

    [SerializeField]
    private float maxTerrainHeight = 5f;

    [SerializeField]
    private int seed = 12345;

    private readonly Dictionary<HexCell, HexCellView>
        cellViews = new();

    private HexGrid grid;

    public HexGrid Grid => grid;

    private float noiseOffsetX;
    private float noiseOffsetY;

    private UnitManager unitManager;

    private void Awake()
    {
        grid = new HexGrid();

        Random.InitState(seed);

        noiseOffsetX =
            Random.Range(-10000f, 10000f);

        noiseOffsetY =
            Random.Range(-10000f, 10000f);

        GenerateGrid();
    }

    public void Initialize(UnitManager unitManager)
    {
        this.unitManager = unitManager;

        Debug.Assert(
            unitManager != null,
            "UnitManager NULL");
    }

    private void Start()
    {
        SpawnVisuals();

        List<HexCell> cells = Grid.GetAllCells().ToList();

        if (cells.Count < 2)
        {
            Debug.LogError(
                "Not enough cells to spawn units.");

            return;
        }

        Player localPlayer =
            new Player(
                "Player1",
                Team.Team1,
                true);

        //Player enemyPlayer = new Player("Enemy",Team.Team2);

        unitManager.RegisterPlayer(
            localPlayer);

        //unitManager.RegisterPlayer(enemyPlayer);

        HexCell playerCell = GetRandomFreeCell(cells);

        HexCell enemyCell = GetRandomFreeCell(cells);

        UnitStats playerStats =
            new UnitStats
            {
                Strength = 5,
                Dexterity = 5,
                Reflexes = 5,
                Vitality = 5,
                MovementPoints = 8,
                MoveSpeed = 12f
            };

        playerStats.ResetMovement();

        UnitStats enemyStats =
            new UnitStats
            {
                Strength = 3,
                Dexterity = 3,
                Reflexes = 3,
                Vitality = 3,
                MovementPoints = 6,
                MoveSpeed = 10f
            };

        enemyStats.ResetMovement();

        unitManager.SpawnUnit(playerCell,localPlayer,playerStats);

        //unitManager.SpawnUnit(enemyCell,enemyPlayer,enemyStats);
    }

    private HexCell GetRandomFreeCell(
    List<HexCell> cells)
    {
        List<HexCell> freeCells =
            cells
                .Where(x => !x.IsOccupied)
                .ToList();

        if (freeCells.Count == 0)
        {
            return null;
        }

        return freeCells[
            Random.Range(
                0,
                freeCells.Count)];
    }

    private void GenerateGrid()
    {
        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            int zMin =
                Mathf.Max(
                    -gridRadius,
                    -x - gridRadius);

            int zMax =
                Mathf.Min(
                    gridRadius,
                    -x + gridRadius);

            for (int z = zMin; z <= zMax; z++)
            {
                int y = -x - z;

                HexCoord coordinate =
                    new HexCoord(x, y, z);

                float noiseValue =
                    Mathf.PerlinNoise(
                        coordinate.X * noiseScale + noiseOffsetX,
                        coordinate.Z * noiseScale + noiseOffsetY);

                float height =
                    noiseValue * maxTerrainHeight;

                HexCell cell =
                    new HexCell(
                        coordinate,
                        height);

                grid.AddCell(cell);
            }
        }
    }

    private void SpawnVisuals()
    {
        foreach (HexCell cell in grid.GetAllCells())
        {
            GameObject instance =
                Instantiate(
                    hexCellPrefab,
                    cell.WorldPosition,
                    Quaternion.identity,
                    transform);

            HexCellView view =
                instance.GetComponent<HexCellView>();

            view.Initialize(cell);

            cellViews.Add(cell, view);
        }
    }

    public HexCellView GetView(
        HexCell cell)
    {
        return cellViews.TryGetValue(
            cell,
            out HexCellView view)
            ? view
            : null;
    }

    public void RefreshCell(
        HexCell cell)
    {
        if (!cellViews.TryGetValue(
                cell,
                out HexCellView view))
        {
            return;
        }

        view.RefreshPosition();
    }

    public void RefreshCells(
        IEnumerable<HexCell> cells)
    {
        foreach (HexCell cell in cells)
        {
            RefreshCell(cell);
        }
    }
}