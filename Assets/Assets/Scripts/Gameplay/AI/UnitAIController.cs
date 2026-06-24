using System.Collections;
using UnityEngine;

public class UnitAIController
{
    private readonly HexUnit unit;
    private readonly UnitAIBrain brain;

    public UnitAIController(HexUnit unit, UnitAIBrain brain)
    {
        this.unit = unit;
        this.brain = brain;
    }

    public IEnumerator ExecuteTurn()
    {
        if (unit.IsMoving || unit.TurnEnded)
            yield break;

        yield return brain.Execute();
    }
}