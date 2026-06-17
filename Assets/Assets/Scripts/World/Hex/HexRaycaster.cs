using UnityEngine;

public class HexRaycaster
{
    private readonly Camera camera;

    public HexRaycaster(Camera camera)
    {
        this.camera = camera;
    }

    public HexCellView RaycastCell(Vector2 mousePosition)
    {
        Ray ray =
            camera.ScreenPointToRay(
                mousePosition);

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit))
        {
            return null;
        }

        return hit.collider
            .GetComponentInParent<HexCellView>();
    }

    public HexUnitView RaycastUnit(Vector2 mousePosition)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.GetComponentInParent<HexUnitView>();
        }

        return null;
    }
}