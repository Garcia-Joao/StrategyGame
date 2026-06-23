using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;

    [SerializeField]
    private float unitHeightOffset = 0.5f;

    public float UnitHeightOffset =>
        unitHeightOffset;

    public Player LocalPlayer
    {
        get;
        private set;
    }

    private readonly List<HexUnit> units = new();

    private readonly List<Player> players = new();

    public IReadOnlyList<HexUnit> Units => units;

    public IReadOnlyList<Player> Players => players;

    public IEnumerable<HexUnit> GetUnits(Team team)
    {
        return units.Where(x => x.Team == team);
    }

    public bool HasAnyUnit(Team team)
    {
        return units.Any(x => x.Team == team);
    }

    public void RegisterPlayer(
        Player player)
    {
        if (player == null)
        {
            return;
        }

        if (players.Contains(player))
        {
            return;
        }

        players.Add(player);

        if (player.IsLocalPlayer)
        {
            LocalPlayer = player;
        }
    }

    public HexUnit SpawnUnit(
        HexCell cell,
        Player owner,
        UnitStats stats,
        MovementProperties movementProperties,
        ActionStats actionStats)
    {
        Vector3 spawnPos =
            cell.WorldPosition +
            Vector3.up * unitHeightOffset;

        GameObject instance =
            Instantiate(
                unitPrefab,
                spawnPos,
                Quaternion.identity);

        HexUnitView view =
            instance.GetComponent<HexUnitView>();

        HexUnit unit =
            new HexUnit(
                owner.Name,
                owner,
                stats,
                movementProperties,
                actionStats
                );

        unit.SetCell(cell);
        unit.SetView(view);

        cell.SetOccupyingUnit(
            unit);

        view.Initialize(unit);

        units.Add(unit);

        return unit;
    }

    public HexUnit SpawnUnit(
        HexCell cell,
        Player owner,
        string name,
        UnitStats stats,
        MovementProperties movementProperties,
        ActionStats actionStats)
    {
        Vector3 spawnPos =
            cell.WorldPosition +
            Vector3.up * unitHeightOffset;

        GameObject instance = Instantiate(unitPrefab,spawnPos,Quaternion.identity);

        instance.name = name;

        HexUnitView view =
            instance.GetComponent<HexUnitView>();

        HexUnit unit =
            new HexUnit(
                name,
                owner,
                stats,
                movementProperties,
                actionStats);

        unit.SetCell(cell);
        unit.SetView(view);

        cell.SetOccupyingUnit(
            unit);

        view.Initialize(unit);

        units.Add(unit);

        return unit;
    }

    public IEnumerable<HexUnit> GetUnits(
        Player player)
    {
        return units.Where(
            x => x.Owner == player);
    }

    internal void SetLocalPlayer(Player localPlayer)
    {
        LocalPlayer = localPlayer;
    }
}