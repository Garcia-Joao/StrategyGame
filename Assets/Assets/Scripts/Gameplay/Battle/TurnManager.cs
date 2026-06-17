using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private TurnMode turnMode = TurnMode.Chaos;
    private WorldTurnManager worldTurnManager;
    private ITurnSystem activeSystem;

    public ITurnSystem ActiveSystem =>
        activeSystem;

    public void Initialize(
        UnitManager unitManager,
        UnitSelectionManager selectionManager,
        WorldTurnManager worldTurnManager,
        CameraController cameraController,
        HexGridManager gridManager)
    {
        this.worldTurnManager =
            worldTurnManager;

        switch (turnMode)
        {
            case TurnMode.Chaos:

                activeSystem =
                    new ChaosTurnSystem(
                        unitManager,
                        selectionManager,
                        worldTurnManager,
                        unitManager.LocalPlayer,
                        cameraController);

                break;

            case TurnMode.Dexterity:

                activeSystem =
                    new DexTurnSystem();

                break;
        }

        activeSystem.StartBattle();
    }

    public void EndCurrentTurn()
    {
        Debug.Log("Ending Current Turn");
        activeSystem.EndCurrentTurn();
    }
}