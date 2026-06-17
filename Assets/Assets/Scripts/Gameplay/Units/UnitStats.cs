using UnityEngine;

[System.Serializable]
public class UnitStats
{
    public int Strength;
    public int Dexterity;
    public int Reflexes;
    public int Vitality;

    public int MovementPoints;
    public int CurrentMovementPoints;

    public float MoveSpeed;

    public UnitStats Clone()
    {
        return new UnitStats
        {
            Strength = Strength,
            Dexterity = Dexterity,
            Reflexes = Reflexes,
            Vitality = Vitality,
            MovementPoints = MovementPoints,
            CurrentMovementPoints = MovementPoints,
            MoveSpeed = MoveSpeed
        };
    }

    public void ResetMovement()
    {
        CurrentMovementPoints =
            MovementPoints;
    }

    public void ConsumeMovement(
        int amount)
    {
        CurrentMovementPoints =
            Mathf.Max(
                0,
                CurrentMovementPoints - amount);
    }
}