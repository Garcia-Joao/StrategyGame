using UnityEngine;

public class UnitMovementController : MonoBehaviour
{
    public void MoveUnit(
        HexUnit unit,
        HexCell destination)
    {
        if (unit == null)
        {
            return;
        }

        unit.SetCell(
            destination);

        HexUnitView view =
            unit.View;

        if (view != null)
        {
            view.transform.position =
                destination.WorldPosition +
                Vector3.up * 0.5f;
        }
    }
}