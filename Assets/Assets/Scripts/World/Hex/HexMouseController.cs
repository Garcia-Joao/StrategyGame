using UnityEngine;

public class HexMouseController : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    private InputManager inputManager;

    private HexSelectionManager selectionManager;

    private HexRaycaster raycaster;

    private HexCellView currentHoveredView;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    public void Initialize(
    InputManager inputManager,
    HexSelectionManager selectionManager)
    {
        this.inputManager = inputManager;
        this.selectionManager = selectionManager;

        raycaster =
            new HexRaycaster(
                targetCamera);
    }

    private void Update()
    {
        UpdateHover();
    }

    private void UpdateHover()
    {
        Vector2 mousePosition =
            inputManager
                .MousePositionAction
                .GetCurrentValue();

        HexCellView hitView =
            raycaster.RaycastCell(
                mousePosition);

        if (hitView == currentHoveredView)
        {
            return;
        }

        currentHoveredView = hitView;

        selectionManager.SetHoveredCell(
            hitView != null
                ? hitView.Cell
                : null);
    }
}