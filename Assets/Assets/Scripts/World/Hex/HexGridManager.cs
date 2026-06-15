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
    }

    public void Initialize(UnitManager unitManager)
    {
        this.unitManager = unitManager;
    }

    private void Start()
    {
        GenerateGrid();
        SpawnVisuals();
        List<HexCell> cells = Grid.GetAllCells().ToList();

        int randomIndex = Random.Range(1, cells.Count);

        HexCell spawnCell = cells[randomIndex];
        unitManager.SpawnUnit(spawnCell);
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