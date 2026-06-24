using System.Collections;
using UnityEngine;

public class HexCellView : MonoBehaviour
{
    private Renderer cachedRenderer;

    [Header("Transition")]
    [SerializeField]
    private float colorTransitionSpeed = 10f;

    [Header("Colors")]
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color pathColor = Color.cyan;
    [SerializeField] private Color invalidPathColor = Color.red;
    [SerializeField] private Color moveRangeColor = Color.blue;

    [Header("Action Colors")]
    [SerializeField] private Color actionRangeColor = Color.magenta;

    [SerializeField]
    private Color validActionPreviewColor =
        new Color(0f, 1f, 0f, 1f);

    [SerializeField]
    private Color invalidActionPreviewColor =
        new Color(1f, 0f, 0f, 1f);

    [SerializeField] private Color validActionColor = Color.green;
    [SerializeField] private Color invalidActionColor = Color.red;

    public HexCell Cell { get; private set; }

    private bool isHovered;
    private bool isSelected;
    private bool isPath;
    private bool isInvalidPath;
    private bool isMoveRange;

    private bool isActionRange;
    private bool isValidActionTarget;
    private bool isInvalidActionTarget;

    private bool isActionPreview;
    private bool isValidPreview;

    private float brushPreviewIntensity;

    private Material cachedMaterial;

    private Color currentColor;
    private Color targetColor;

    private int pathVersion;

    private int pathIndex;
    private bool usePathWave;

    private void Awake()
    {
        cachedRenderer =
            GetComponentInChildren<Renderer>();

        if (cachedRenderer != null)
        {
            cachedMaterial =
                cachedRenderer.material;

            currentColor =
                cachedMaterial.color;

            targetColor =
                currentColor;
        }
    }

    private void Update()
    {
        if (cachedMaterial == null)
            return;

        Color animatedTarget = GetAnimatedTargetColor();

        currentColor =
            Color.Lerp(
                currentColor,
                animatedTarget,
                Time.deltaTime *
                colorTransitionSpeed);

        cachedMaterial.color =
            currentColor;
    }

    private Color GetAnimatedTargetColor()
    {
        if (isPath || isInvalidPath)
        {
            Color baseColor =
                isPath
                    ? pathColor
                    : invalidPathColor;

            float wave =
                Mathf.Sin(
                    Time.time * 2.67f
                    - pathIndex * 0.45f);

            wave =
                (wave + 1f) * 0.5f;

            return Color.Lerp(
                Color.white,
                baseColor,
                wave);
        }

        return targetColor;
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
        transform.position =
            Cell.WorldPosition;
    }

    public void ResetState()
    {
        isHovered = false;
        isSelected = false;
        isPath = false;
        isInvalidPath = false;
        isMoveRange = false;

        isActionRange = false;
        isValidActionTarget = false;
        isInvalidActionTarget = false;

        isActionPreview = false;
        isValidPreview = false;

        brushPreviewIntensity = 0f;

        RefreshVisual();
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

        if (value)
            isInvalidPath = false;

        RefreshVisual();
    }

    public void SetInvalidPath(bool value)
    {
        isInvalidPath = value;

        if (value)
            isPath = false;

        RefreshVisual();
    }

    public void SetMoveRange(bool value)
    {
        isMoveRange = value;
        RefreshVisual();
    }

    public void SetBrushPreview(float intensity)
    {
        brushPreviewIntensity =
            Mathf.Clamp01(intensity);

        RefreshVisual();
    }

    public void SetActionRange(bool value)
    {
        isActionRange = value;
        RefreshVisual();
    }

    public void SetValidActionTarget(bool value)
    {
        isValidActionTarget = value;
        RefreshVisual();
    }

    public void SetInvalidActionTarget(bool value)
    {
        isInvalidActionTarget = value;
        RefreshVisual();
    }

    public void SetActionPreview(
        bool value,
        bool isValid)
    {
        isActionPreview = value;
        isValidPreview = isValid;

        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (cachedMaterial == null)
            return;

        // Seleção

        if (isSelected)
        {
            targetColor = selectedColor;
            return;
        }

        // Pathfinding

        if (isInvalidPath)
        {
            targetColor = invalidPathColor;
            return;
        }

        if (isPath)
        {
            targetColor = pathColor;
            return;
        }

        // Hover

        if (isHovered)
        {
            targetColor = hoverColor;
            return;
        }

        // Movimento

        if (isMoveRange)
        {
            targetColor = moveRangeColor;
            return;
        }

        // Brush

        if (brushPreviewIntensity > 0f)
        {
            targetColor =
                Color.Lerp(
                    Color.yellow,
                    Color.red,
                    brushPreviewIntensity);

            return;
        }

        // Preview da habilidade
        // PRIORIDADE MÁXIMA DENTRO DO SISTEMA DE AÇÕES

        if (isActionPreview)
        {
            targetColor =
                isValidPreview
                    ? validActionPreviewColor
                    : invalidActionPreviewColor;

            return;
        }

        // Alvos válidos

        if (isValidActionTarget)
        {
            targetColor = validActionColor;
            return;
        }

        // Alvos inválidos

        if (isInvalidActionTarget)
        {
            targetColor = invalidActionColor;
            return;
        }

        // Alcance

        if (isActionRange)
        {
            targetColor = actionRangeColor;
            return;
        }

        targetColor = Color.white;
    }

    private Coroutine pathRoutine;

    public void SetPathAnimated(
        bool valid,
        float delay,
        int version)
    {
        pathVersion = version;

        if (pathRoutine != null)
            StopCoroutine(pathRoutine);

        pathRoutine =
            StartCoroutine(
                AnimatePath(
                    valid,
                    delay,
                    version));
    }

    private IEnumerator AnimatePath(
        bool valid,
        float delay,
        int version)
    {
        yield return new WaitForSeconds(delay);

        if (version != pathVersion)
            yield break;

        if (valid)
        {
            isPath = true;
            isInvalidPath = false;
        }
        else
        {
            isInvalidPath = true;
            isPath = false;
        }

        RefreshVisual();
    }

    public void InvalidatePathAnimations()
    {
        pathVersion++;

        isPath = false;
        isInvalidPath = false;

        if (pathRoutine != null)
        {
            StopCoroutine(pathRoutine);
            pathRoutine = null;
        }

        RefreshVisual();
    }

    public void SetPathWaveIndex(int index)
    {
        pathIndex = index;
    }
}