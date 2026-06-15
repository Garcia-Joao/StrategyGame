using UnityEngine;

public class HexUnitView : MonoBehaviour
{
    public HexUnit Unit { get; private set; }

    public void Initialize(
        HexUnit unit)
    {
        Unit = unit;
    }
}