using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance
    {
        get;
        private set;
    }

    public UnitManager UnitManager
    {
        get;
        private set;
    }

    public UnitSelectionManager UnitSelection
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(
        UnitSelectionManager unitSelection,
        UnitManager unitManager)
    {
        UnitSelection = unitSelection;
        UnitManager = unitManager;
    }
}