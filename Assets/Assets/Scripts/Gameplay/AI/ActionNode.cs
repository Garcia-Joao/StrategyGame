using System;

public class ActionNode : BTNode
{
    private readonly Func<BTState> action;

    public ActionNode(Func<BTState> action)
    {
        this.action = action;
    }

    public override BTState Evaluate()
    {
        return action();
    }
}