using System.Collections.Generic;

public class Selector : BTNode
{
    private readonly List<BTNode> children;

    public Selector(List<BTNode> children)
    {
        this.children = children;
    }

    public override BTState Evaluate()
    {
        foreach (var child in children)
        {
            var result = child.Evaluate();

            if (result == BTState.Success)
                return BTState.Success;
        }

        return BTState.Failure;
    }
}