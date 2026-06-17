using System.Collections.Generic;

public class Player
{
    public string Name { get; }

    public Team Team { get; }

    public bool IsLocalPlayer { get; }

    private readonly List<HexUnit>
        ownedUnits = new();

    public IReadOnlyList<HexUnit>
        OwnedUnits => ownedUnits;

    public Player(
        string name,
        Team team,
        bool isLocalPlayer = false)
    {
        Name = name;
        Team = team;
        IsLocalPlayer = isLocalPlayer;
    }

    public void AddUnit(
        HexUnit unit)
    {
        if (!ownedUnits.Contains(unit))
        {
            ownedUnits.Add(unit);
        }
    }

    public void RemoveUnit(
        HexUnit unit)
    {
        ownedUnits.Remove(unit);
    }
}