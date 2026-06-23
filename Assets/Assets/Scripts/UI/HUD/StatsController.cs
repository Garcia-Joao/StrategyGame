using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private TextMeshProUGUI manaValue;
    [SerializeField] private TextMeshProUGUI mpValue;
    [SerializeField] private TextMeshProUGUI apValue;
    [SerializeField] private TextMeshProUGUI qapValue;

    private HexUnit currentUnit;

    public void Bind(HexUnit unit)
    {
        Unbind();

        currentUnit = unit;

        if (currentUnit == null)
        {
            Clear();
            return;
        }

        currentUnit.Resources.Health.Changed += Refresh;
        currentUnit.Resources.Mana.Changed += Refresh;
        currentUnit.Resources.Movement.Changed += Refresh;

        currentUnit.ActionStats.ActionsChanged += Refresh;

        Refresh();
    }

    public void Unbind()
    {
        if (currentUnit == null)
            return;

        currentUnit.Resources.Health.Changed -= Refresh;
        currentUnit.Resources.Mana.Changed -= Refresh;
        currentUnit.Resources.Movement.Changed -= Refresh;

        currentUnit.ActionStats.ActionsChanged -= Refresh;

        currentUnit = null;
    }

    private void Refresh()
    {
        if (currentUnit == null)
            return;

        healthValue.text =
            $"{currentUnit.Resources.Health.Current}/{currentUnit.Resources.Health.Max}";

        manaValue.text =
            $"{currentUnit.Resources.Mana.Current}/{currentUnit.Resources.Mana.Max}";

        mpValue.text =
            $"{currentUnit.Resources.Movement.Current}/{currentUnit.Resources.Movement.Max}";

        apValue.text =
            $"{currentUnit.ActionStats.Actions.Current}/{currentUnit.ActionStats.Actions.Max}";

        qapValue.text =
            $"{currentUnit.ActionStats.QuickActions.Current}/{currentUnit.ActionStats.QuickActions.Max}";
    }

    private void Clear()
    {
        healthValue.text = "-";
        manaValue.text = "-";
        mpValue.text = "-";
        apValue.text = "-";
        qapValue.text = "-";
    }

    private void OnDestroy()
    {
        Unbind();
    }
}