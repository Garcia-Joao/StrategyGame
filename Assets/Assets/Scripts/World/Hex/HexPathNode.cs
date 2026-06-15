public class HexPathNode
{
    public HexCell Cell { get; }

    public HexPathNode Parent { get; set; }

    public int GCost { get; set; }

    public int HCost { get; set; }

    public int FCost => GCost + HCost;

    public HexPathNode(HexCell cell)
    {
        Cell = cell;

        GCost = int.MaxValue;
        HCost = 0;
        Parent = null;
    }
}