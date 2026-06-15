using UnityEngine;

[CreateAssetMenu(
    fileName = "HexPathRules",
    menuName = "Hex Grid/Path Rules")]
public class HexPathRules : ScriptableObject
{
    [Header("Height Rules")]
    [SerializeField]
    private float maxClimbHeight = 1;

    [SerializeField]
    private bool allowUnlimitedDescent = true;

    [Header("Movement Cost")]
    [SerializeField]
    private int baseMoveCost = 1;

    [SerializeField]
    private int climbCostPerLevel = 1;

    public float MaxClimbHeight => maxClimbHeight;

    public bool AllowUnlimitedDescent => allowUnlimitedDescent;

    public bool CanMove(
        HexCell from,
        HexCell to)
    {
        float heightDifference =
            to.Height - from.Height;

        if (heightDifference > maxClimbHeight)
        {
            return false;
        }

        if (!allowUnlimitedDescent)
        {
            if (Mathf.Abs(heightDifference)
                > maxClimbHeight)
            {
                return false;
            }
        }

        return true;
    }

    public int GetMoveCost(
        HexCell from,
        HexCell to)
    {
        float climbHeight =
            Mathf.Max(
                0f,
                to.Height - from.Height);

        float cost =
            baseMoveCost +
            climbHeight *
            climbCostPerLevel +
            to.MovementCost;

        return Mathf.RoundToInt(cost);
    }

    public float GetHeightDifference(
        HexCell from,
        HexCell to)
    {
        return to.Height - from.Height;
    }
}