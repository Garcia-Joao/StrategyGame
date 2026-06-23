using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDefinition : ScriptableObject
{
    public abstract IEnumerable<HexCell>
        GetAffectedCells(
            HexGrid grid,
            HexCell center);
}