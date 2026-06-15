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

    private void Start()
    {
        inputManager =
    ServiceLocator.Locate<InputManager>();

        selectionManager =
            ServiceLocator.Locate<HexSelectionManager>();
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