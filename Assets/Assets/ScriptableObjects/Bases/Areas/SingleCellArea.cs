using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Single Cell Area",
    menuName = "Combat/Areas/Single Cell")]
public class SingleCellArea : AreaDefinition
{
    public override IEnumerable<HexCell>
        GetAffectedCells(
            HexGrid grid,
            HexCell center)
    {
        yield return center;
    }
}