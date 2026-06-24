using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
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

    public event Action<HexUnit> UnitSpawned;

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

    public bool IsAIControlled(Team team)
    {
        return LocalPlayer.Team != team;
    }

    public HexUnit SpawnUnit(
    HexCell cell,
    Player owner,
    UnitDefinition definition)
    {
        Vector3 spawnPos =
            cell.WorldPosition +
            Vector3.up * unitHeightOffset;

        GameObject instance =
            Instantiate(
                definition.Prefab,
                spawnPos,
                Quaternion.identity);

        instance.name = definition.UnitName;

        HexUnitView view = instance.GetComponent<HexUnitView>();

        UnitStats stats = UnitStatsFactory.Create(definition);

        MovementProperties movementProperties =
            new MovementProperties
            {
                IgnoreOccupants = false,
                CanShareTile = false,
                IgnoreHeight = false
            };

        ActionStats actionStats =
            new ActionStats();

        actionStats.Initialize(definition.ActionPoints, definition.QuickActionPoints);

        UnitResources resources =
            new UnitResources();

        resources.Initialize(
            health: definition.StartHealth,
            mana: definition.StartMana,
            movement: definition.MovementPoints);

        HexUnit unit =
            new HexUnit(
                definition.UnitName,
                owner,
                stats,
                movementProperties,
                actionStats,
                resources,
                definition.Actions,
                instance.transform);

        unit.SetCell(cell);
        unit.SetView(view);

        cell.SetOccupyingUnit(unit);

        view.Initialize(unit);

        units.Add(unit);
        UnitSpawned?.Invoke(unit);
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