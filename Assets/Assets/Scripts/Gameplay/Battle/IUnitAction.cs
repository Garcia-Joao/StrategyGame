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