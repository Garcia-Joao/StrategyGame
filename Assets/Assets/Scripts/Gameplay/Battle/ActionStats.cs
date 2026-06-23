using System;

[Serializable]
public class ActionStats
{
    public int ActionPoints;

    public int CurrentActionPoints;

    public int QuickActionPoints;

    public int CurrentQuickActionPoints;

    public event Action ActionsChanged;

    public void Reset()
    {
        CurrentActionPoints =
            ActionPoints;

        CurrentQuickActionPoints =
            QuickActionPoints;

        ActionsChanged?.Invoke();
    }

    public bool CanPay(
        int actionCost,
        int quickCost)
    {
        return
            CurrentActionPoints >= actionCost &&
            CurrentQuickActionPoints >= quickCost;
    }

    public void Consume(
        int actionCost,
        int quickCost)
    {
        CurrentActionPoints -= actionCost;

        CurrentQuickActionPoints -= quickCost;

        ActionsChanged?.Invoke();
    }
}

public struct ActionCost
{
    public int ActionPoints;

    public int QuickActionPoints;

    public int Mana;

    public int Health;
    public int MovementPoints;
}