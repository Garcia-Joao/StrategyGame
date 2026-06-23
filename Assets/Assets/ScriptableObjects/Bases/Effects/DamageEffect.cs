using UnityEngine;

[CreateAssetMenu(
    menuName = "Game/Actions/Effects/Damage Effect")]
public class DamageEffect : ActionEffect
{
    [SerializeField]
    private int damage = 10;

    public override void Apply(
        ActionContext context)
    {
        foreach (HexUnit unit
                 in context.AffectedUnits)
        {
            if (unit == null)
                continue;

            unit.Resources.TakeDamage(damage);
        }
    }
}