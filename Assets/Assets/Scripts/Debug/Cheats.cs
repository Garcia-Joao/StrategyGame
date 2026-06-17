using System.Linq;
using IngameDebugConsole;
using UnityEngine;

public static class Cheats
{
    [ConsoleMethod(
        "stats",
        "stats <unit_name>")]
    public static void Stats(
        string unitName)
    {
        HexUnit unit =
            FindUnit(unitName);

        if (unit == null)
        {
            Debug.LogWarning(
                $"Unit '{unitName}' not found.");

            return;
        }

        Debug.Log(
$@"=== {unit.Name} ===

Movement: {unit.Stats.MovementPoints}
Move Speed: {unit.Stats.MoveSpeed}

Strength: {unit.Stats.Strength}
Dexterity: {unit.Stats.Dexterity}
Reflexes: {unit.Stats.Reflexes}
Vitality: {unit.Stats.Vitality}");
    }

    [ConsoleMethod(
        "set_stat",
        "set_stat <unit_name> <stat> <value>")]
    public static void SetStat(
        string unitName,
        string stat,
        int value)
    {
        HexUnit unit =
            FindUnit(unitName);

        if (unit == null)
        {
            Debug.LogWarning(
                $"Unit '{unitName}' not found.");

            return;
        }

        switch (stat.ToLower())
        {
            case "strength":
            case "str":
                unit.Stats.Strength = value;
                break;

            case "dexterity":
            case "dex":
                unit.Stats.Dexterity = value;
                break;

            case "reflexes":
            case "ref":
                unit.Stats.Reflexes = value;
                break;

            case "vitality":
            case "vit":
                unit.Stats.Vitality = value;
                break;

            case "movement":
            case "move":
            case "mp":
                unit.Stats.MovementPoints = value;
                break;

            case "movespeed":
            case "speed":
                unit.Stats.MoveSpeed = value;
                break;

            default:
                Debug.LogWarning(
                    $"Unknown stat '{stat}'.");

                return;
        }

        Debug.Log(
            $"{unit.Name}: {stat} = {value}");
    }

    [ConsoleMethod(
        "max_stats",
        "max_stats <unit_name>")]
    public static void MaxStats(
        string unitName)
    {
        HexUnit unit =
            FindUnit(unitName);

        if (unit == null)
        {
            Debug.LogWarning(
                $"Unit '{unitName}' not found.");

            return;
        }

        unit.Stats.Strength = 999;
        unit.Stats.Dexterity = 999;
        unit.Stats.Reflexes = 999;
        unit.Stats.Vitality = 999;
        unit.Stats.MovementPoints = 99;
        unit.Stats.MoveSpeed = 50f;

        Debug.Log(
            $"{unit.Name} maxed.");
    }

    [ConsoleMethod(
        "list_units",
        "Lists all units")]
    public static void ListUnits()
    {
        UnitManager unitManager =
            DebugManager
                .Instance
                .UnitManager;

        foreach (HexUnit unit in unitManager.Units)
        {
            Debug.Log(unit.Name);
        }
    }

    private static HexUnit FindUnit(
        string unitName)
    {
        UnitManager unitManager =
            DebugManager
                .Instance
                .UnitManager;

        return unitManager.Units.FirstOrDefault(
            u => u.Name.Equals(
                unitName,
                System.StringComparison.OrdinalIgnoreCase));
    }
}