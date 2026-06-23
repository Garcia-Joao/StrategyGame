using TMPro;
using UnityEngine;

public class UnitInfoView : UIWindow
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text healthText;

    [SerializeField]
    private TMP_Text manaText;

    [SerializeField]
    private TMP_Text strengthText;

    [SerializeField]
    private TMP_Text dexterityText;

    [SerializeField]
    private TMP_Text reflexesText;

    [SerializeField]
    private TMP_Text vitalityText;

    HexUnit currentUnit;

    public void Show(HexUnit unit)
    {
        currentUnit = unit;

        Refresh();

        Bind(unit);
        Open();
    }

    private void Refresh()
    {
        if (currentUnit == null)
            return;

        Debug.Log(
            $"Showing info for {currentUnit.Name}");
    }

    public void Bind(HexUnit unit)
    {
        nameText.text =
            unit.Name;

        healthText.text =
            $"{unit.Resources.Health.Current}/{unit.Resources.Health.Max}";

        manaText.text =
            $"{unit.Resources.Mana.Current}/{unit.Resources.Mana.Max}";

        strengthText.text =
            unit.Stats.Strength.ToString();

        dexterityText.text =
            unit.Stats.Dexterity.ToString();

        reflexesText.text =
            unit.Stats.Reflexes.ToString();

        vitalityText.text =
            unit.Stats.Vitality.ToString();
    }
}