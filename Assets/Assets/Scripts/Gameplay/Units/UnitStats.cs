using System;

[Serializable]
public class UnitStats
{
    public int Strength;
    public int Dexterity;
    public int Reflexes;
    public int Vitality;

    public float MoveSpeed;

    public UnitStats Clone()
    {
        return new UnitStats
        {
            Strength = Strength,
            Dexterity = Dexterity,
            Reflexes = Reflexes,
            Vitality = Vitality,
            MoveSpeed = MoveSpeed
        };
    }
}