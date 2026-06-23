using UnityEngine;

public class UnitStatsHUDController : MonoBehaviour
{
    [SerializeField]
    private StatsController statsController;

    private UnitSelectionManager unitSelection;

    public void Initialize(
        UnitSelectionManager unitSelection)
    {
        this.unitSelection = unitSelection;

        unitSelection.UnitSelected += OnUnitSelected;
        unitSelection.UnitDeselected += OnUnitDeselected;
    }

    private void OnUnitSelected(
        HexUnit unit)
    {
        statsController.Bind(unit);
    }

    private void OnUnitDeselected(
        HexUnit unit)
    {
        statsController.Unbind();
    }

    private void OnDestroy()
    {
        if (unitSelection == null)
            return;

        unitSelection.UnitSelected -= OnUnitSelected;
        unitSelection.UnitDeselected -= OnUnitDeselected;
    }
}