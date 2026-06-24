using System.Collections.Generic;

public class Sequence : BTNode
{
    private readonly List<BTNode> children;

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override BTState Evaluate()
    {
        foreach (var child in children)
        {
            var result = child.Evaluate();
            if (result != BTState.Success)
                return result;
        }

        return BTState.Success;
    }
}