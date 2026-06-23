public class InitiativeEntry
{
    public HexUnit Unit;

    public int Roll;

    public int TieBreaker;

    public int Dexterity;

    public int Initiative =>
        Roll + Dexterity;
}