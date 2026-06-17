public static class UnitStatsFactory
{
    public static UnitStats Create(UnitDefinition definition)
    {
        return new UnitStats
        {
            MovementPoints =
                definition.MovementPoints,

            MoveSpeed =
                definition.MoveSpeed,

            Strength =
                definition.Strength,

            Dexterity =
                definition.Dexterity,

            Reflexes =
                definition.Reflexes,

            Vitality =
                definition.Vitality
        };
    }

    public static UnitStats Create(
        BaseStatsData baseStats,
        ClassDefinition classDefinition)
    {
        return new UnitStats
        {
            MovementPoints =
                baseStats.movementPoints +
                classDefinition.movementPointsBonus,

            MoveSpeed = baseStats.moveSpeed,

            Strength =
                baseStats.strength + classDefinition.strengthBonus,

            Dexterity =
                baseStats.dexterity + classDefinition.dexterityBonus,

            Reflexes =
                baseStats.reflexes + classDefinition.reflexesBonus,

            Vitality = baseStats.vitality + classDefinition.vitalityBonus
        };
    }
}