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
            Debug.Log("Hitted Any Cell");
            return null;
        }

        Debug.Log("Hitted A Cell");

        return hit.collider
            .GetComponentInParent<HexCellView>();
    }

    public HexUnitView RaycastUnit(Vector2 mousePosition)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hit object: " + hit.collider.name);
            return hit.collider.GetComponentInParent<HexUnitView>();
        }

        Debug.Log("NO UNIT HIT");
        return null;
    }
}