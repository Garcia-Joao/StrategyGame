using UnityEngine;

public class HexUnitView : MonoBehaviour
{
    [SerializeField]
    private Renderer cachedRenderer;

    public HexUnit Unit
    {
        get;
        private set;
    }

    public string Name;

    public void Initialize(
        HexUnit unit)
    {
        Unit = unit;
        Name = unit.Name;

        Unit.TurnStateChanged += RefreshVisual;

        RefreshVisual();
    }

    private void OnDestroy()
    {
        if (Unit != null)
        {
            Unit.TurnStateChanged -= RefreshVisual;
        }
    }

    public void RefreshVisual()
    {
        if (Unit == null)
        {
            return;
        }

        if (cachedRenderer == null)
        {
            return;
        }

        if (Unit.TurnEnded)
        {
            cachedRenderer.material.color =
                Color.gray;

            return;
        }

        switch (Unit.Team)
        {
            case Team.Team1:

                cachedRenderer.material.color =
                    Color.blue;

                break;

            case Team.Team2:

                cachedRenderer.material.color =
                    Color.red;

                break;
        }
    }
}