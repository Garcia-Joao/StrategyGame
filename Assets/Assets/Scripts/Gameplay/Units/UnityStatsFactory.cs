public static class UnitStatsFactory
{
    public static UnitStats Create(
        BaseStatsData baseStats,
        ClassDefinition classData)
    {
        return new UnitStats
        {
            MovementPoints =
                baseStats.movementPoints +
                classData.movementPointsBonus,

            MoveSpeed =
                baseStats.moveSpeed,

            Strength =
                baseStats.strength +
                classData.strengthBonus,

            Dexterity =
                baseStats.dexterity +
                classData.dexterityBonus,

            Reflexes =
                baseStats.reflexes +
                classData.reflexesBonus,

            Vitality =
                baseStats.vitality +
                classData.vitalityBonus
        };
    }
}