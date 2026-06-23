using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    private List<SlotView> slotViews;

    private SlotView lastSelectedSlot;

    private ActionSelectionManager actionSelectionManager;
    private UnitSelectionManager unitSelectionManager;

    [SerializeField]
    private GameObject slotsParent;

    public void Initialize(
        InputManager inputManager,
        ActionSelectionManager actionSelectionManager,
        UnitSelectionManager unitSelectionManager)
    {
        this.actionSelectionManager =
            actionSelectionManager;

        this.unitSelectionManager =
            unitSelectionManager;

        inputManager.ActionSlotPressed +=
            OnActionSlotPressed;

        unitSelectionManager.UnitSelected +=
            OnUnitSelected;

        unitSelectionManager.UnitDeselected +=
            OnUnitDeselected;

        actionSelectionManager.ActionCleared +=
            OnActionCleared;
    }

    private void OnActionCleared()
    {
        if (lastSelectedSlot != null)
        {
            if (lastSelectedSlot.AssignedAction != null)
            {
                lastSelectedSlot.SetState(
                    SlotState.Available);
            }
        }

        lastSelectedSlot = null;
    }

    private void Awake()
    {
        slotViews =
            slotsParent
                .GetComponentsInChildren<SlotView>()
                .OrderBy(x => x.SlotNumber)
                .ToList();
    }

    private void OnDestroy()
    {
        if (unitSelectionManager != null)
        {
            unitSelectionManager.UnitSelected -=
                OnUnitSelected;

            unitSelectionManager.UnitDeselected -=
                OnUnitDeselected;
        }
    }

    private void OnActionSlotPressed(int slot)
    {
        SlotView clickedSlot =
            slotViews.FirstOrDefault(
                s => s.SlotNumber == slot);

        if (clickedSlot == null)
            return;

        if (clickedSlot.AssignedAction == null)
            return;

        if (clickedSlot.IsEmpty)
            return;

        HexUnit unit =
        unitSelectionManager.SelectedUnit;

        if (unit == null)
            return;

        bool canUse =
            ActionUsageUtility.CanUseAgain(
                unit,
                clickedSlot.AssignedAction);

        if (!canUse)
            return;

        if (clickedSlot.currentState ==
            SlotState.Unavailable)
        {
            return;
        }

        // Deselecionar ação atual
        if (clickedSlot == lastSelectedSlot)
        {
            clickedSlot.SetState(
                SlotState.Available);

            lastSelectedSlot = null;

            actionSelectionManager
                .ClearAction();

            return;
        }

        // Remove seleção anterior
        if (lastSelectedSlot != null)
        {
            lastSelectedSlot.SetState(
                SlotState.Available);
        }

        clickedSlot.SetState(
            SlotState.Selected);

        lastSelectedSlot =
            clickedSlot;

        actionSelectionManager
            .SelectAction(
                clickedSlot.AssignedAction);
    }

    private void OnUnitSelected(
        HexUnit unit)
    {
        if (unit == null)
            return;

        unit.ActionStats.ActionsChanged -=
            RefreshAvailability;

        unit.ActionStats.ActionsChanged +=
            RefreshAvailability;

        ClearSelection();

        RefreshSlots(unit);

        RefreshAvailability();
    }

    private void RefreshSlots(
        HexUnit unit)
    {
        if (unit == null)
            return;

        if (slotViews == null)
            return;

        for (int i = 0; i < slotViews.Count; i++)
        {
            UnitActionDefinition action =
                i < unit.Actions.Count
                    ? unit.Actions[i]
                    : null;

            slotViews[i].SetAction(
                action);
        }
    }

    private void OnUnitDeselected(
        HexUnit unit)
    {
        if (unit != null)
        {
            unit.ActionStats.ActionsChanged -=
                RefreshAvailability;
        }

        foreach (SlotView slot in slotViews)
        {
            slot.SetAction(null);
        }

        ClearSelection();
    }

    private void ClearSelection()
    {
        if (lastSelectedSlot != null)
        {
            lastSelectedSlot.SetState(
                SlotState.Available);
        }

        lastSelectedSlot = null;

        actionSelectionManager
            .ClearAction();
    }

    private void RefreshAvailability()
    {
        HexUnit unit =
            unitSelectionManager.SelectedUnit;

        if (unit == null)
            return;

        foreach (SlotView slot in slotViews)
        {
            if (slot.AssignedAction == null)
                continue;

            bool canUse =
                ActionUsageUtility.CanUseAgain(
                    unit,
                    slot.AssignedAction);

            if (!canUse)
            {
                // Se a habilidade atualmente selecionada
                // ficou inválida, remove a seleção.
                if (slot == lastSelectedSlot)
                {
                    actionSelectionManager.ClearAction();
                }

                slot.SetState(
                    SlotState.Unavailable);
            }
            else if (slot != lastSelectedSlot)
            {
                slot.SetState(
                    SlotState.Available);
            }
        }
    }
}