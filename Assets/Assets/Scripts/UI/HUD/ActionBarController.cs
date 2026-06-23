using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    private List<SlotView> slotViews;
    private SlotView lastSelectedSlot;
    [SerializeField] private GameObject slotsParent;

    public void Initialize(InputManager inputManager)
    {
        inputManager.ActionSlotPressed += OnActionSlotPressed;
    }

    void Start()
    {
        slotViews = slotsParent.GetComponentsInChildren<SlotView>().ToList();
    }

    private void OnActionSlotPressed(int slot)
    {
        SlotView clickedSlot =
            slotViews.FirstOrDefault(
                s => s.SlotNumber == slot);

        if (clickedSlot == null)
        {
            return;
        }

        if (clickedSlot.IsEmpty)
        {
            return;
        }

        if (clickedSlot.currentState == SlotState.Unavailable)
        {
            return;
        }

        if (clickedSlot == lastSelectedSlot)
        {
            clickedSlot.SetState(
                SlotState.Available);

            lastSelectedSlot = null;

            return;
        }

        if (lastSelectedSlot != null)
        {
            lastSelectedSlot.SetState(
                SlotState.Available);
        }

        clickedSlot.SetState(
            SlotState.Selected);

        lastSelectedSlot =
            clickedSlot;
    }
}
