public abstract class BTNode
{
    public abstract BTState Evaluate();
}
public enum BTState
{
    Success,
    Failure,
    Running
}