using System;

[Serializable]
public class ActionStats
{
    public ResourcePool Actions =
        new();

    public ResourcePool QuickActions =
        new();

    public event Action ActionsChanged;

    public void Initialize(
        int actions,
        int quickActions)
    {
        Actions.Initialize(actions);
        QuickActions.Initialize(quickActions);

        Actions.Changed += RaiseChanged;
        QuickActions.Changed += RaiseChanged;
    }

    private void RaiseChanged()
    {
        ActionsChanged?.Invoke();
    }

    public bool CanPay(
        int actionCost,
        int quickCost)
    {
        return
            Actions.Has(actionCost) &&
            QuickActions.Has(quickCost);
    }

    public void Consume(
        int actionCost,
        int quickCost)
    {
        Actions.Consume(actionCost);
        QuickActions.Consume(quickCost);
    }

    public void Reset()
    {
        Actions.Reset();
        QuickActions.Reset();
    }
}