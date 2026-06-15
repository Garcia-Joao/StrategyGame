using UnityEngine;

public class HexCellView : MonoBehaviour
{
    private Renderer cachedRenderer;

    [Header("Colors")]
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color pathColor = Color.cyan;
    [SerializeField] private Color invalidPathColor = Color.red;
    [SerializeField] private Color moveRangeColor = Color.blue;

    public HexCell Cell { get; private set; }

    private bool isHovered;
    private bool isSelected;
    private bool isPath;
    private bool isInvalidPath;
    private bool isMoveRange;

    private float brushPreviewIntensity;

    private void Awake()
    {
        if (cachedRenderer == null)
        {
            cachedRenderer = GetComponentInChildren<Renderer>();
        }
    }

    public void Initialize(HexCell cell)
    {
        Cell = cell;

        ResetState();
        RefreshPosition();
        RefreshVisual();
    }

    public void RefreshPosition()
    {
        transform.position = Cell.WorldPosition;
    }

    public void ResetState()
    {
        isHovered = false;
        isSelected = false;
        isPath = false;
        isInvalidPath = false;
        isMoveRange = false;
        brushPreviewIntensity = 0f;
    }

    public void SetHovered(bool value)
    {
        isHovered = value;
        RefreshVisual();
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        RefreshVisual();
    }

    public void SetPath(bool value)
    {
        isPath = value;
        RefreshVisual();
    }

    public void SetInvalidPath(bool value)
    {
        isInvalidPath = value;
        RefreshVisual();
    }

    public void SetMoveRange(bool value)
    {
        isMoveRange = value;
        RefreshVisual();
    }

    public void SetBrushPreview(float intensity)
    {
        brushPreviewIntensity = Mathf.Clamp01(intensity);
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (cachedRenderer == null) return;

        Material mat = cachedRenderer.material;

        if (isSelected)
        {
            mat.color = selectedColor;
            return;
        }

        if (isInvalidPath)
        {
            mat.color = invalidPathColor;
            return;
        }

        if (isPath)
        {
            mat.color = pathColor;
            return;
        }

        if (isMoveRange)
        {
            mat.color = moveRangeColor;
            return;
        }

        if (brushPreviewIntensity > 0f)
        {
            mat.color = Color.Lerp(Color.yellow, Color.red, brushPreviewIntensity);
            return;
        }

        if (isHovered)
        {
            mat.color = hoverColor;
            return;
        }

        mat.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Cell.WorldPosition, 0.15f);
    }
}