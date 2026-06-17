using System.Collections.Generic;

public class WorldTurnManager
{
    private readonly Queue<IWorldAction>
        pendingActions =  new();

    public bool HasActions =>
        pendingActions.Count > 0;

    public void Enqueue(
        IWorldAction action)
    {
        pendingActions.Enqueue(
            action);
    }

    public void ExecuteAll()
    {
        while (pendingActions.Count > 0)
        {
            IWorldAction action =
                pendingActions.Dequeue();

            action.Execute();
        }
    }
}