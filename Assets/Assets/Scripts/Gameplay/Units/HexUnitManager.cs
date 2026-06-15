using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private float unitHeightOffset = 0.5f;

    private readonly List<HexUnit>
        units = new();

    public IReadOnlyList<HexUnit>
        Units => units;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    public HexUnit SpawnUnit(HexCell cell)
    {
        Vector3 spawnPos = cell.WorldPosition + Vector3.up * unitHeightOffset;

        GameObject instance = Instantiate(unitPrefab, spawnPos, Quaternion.identity);

        HexUnitView view = instance.GetComponent<HexUnitView>();

        HexUnit unit = new HexUnit("Unit", 6);

        unit.SetCell(cell);
        unit.SetView(view);

        view.Initialize(unit);

        return unit;
    }
}