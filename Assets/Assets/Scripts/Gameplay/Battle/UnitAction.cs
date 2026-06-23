public abstract class UnitAction :
    IUnitAction
{
    public abstract string Name { get; }

    public abstract ActionCost Cost { get; }

    public virtual bool CanExecute(
        HexUnit source,
        HexCell target)
    {
        return source.ActionStats.CanPay(
            Cost.ActionPoints,
            Cost.QuickActionPoints);
    }

    public abstract void Execute(
        HexUnit source,
        HexCell target);
}