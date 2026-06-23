using UnityEngine;

public class UnitInfoWindow : UIWindow
{
    private HexUnit currentUnit;

    public void Show(
        HexUnit unit)
    {
        currentUnit = unit;

        Refresh();

        Open();
    }

    private void Refresh()
    {
        if (currentUnit == null)
            return;

        Debug.Log(
            $"Showing info for {currentUnit.Name}");
    }
}