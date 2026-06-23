using System;

public interface IUnitAction
{
    string Name { get; }

    ActionCost Cost { get; }

    bool CanExecute(
        HexUnit source,
        HexCell target);

    void Execute(
        HexUnit source,
        HexCell target);
}

[Serializable]
public class ActionCost
{
    public int ActionPoints;
    public int QuickActionPoints;
    public int Mana;
    public int Health;
    public int MovementPoints;
}