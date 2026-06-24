using System.Collections;

public class UnitAIBrain
{
    private readonly BTNode root;

    public UnitAIBrain(BTNode root)
    {
        this.root = root;
    }

    public IEnumerator Execute()
    {
        yield return root.Evaluate();
    }
}