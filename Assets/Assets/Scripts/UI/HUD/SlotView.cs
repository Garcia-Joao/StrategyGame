using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color availableColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unavailableColor;

    [SerializeField] private int slotNumber;

    private Image slotImage;

    public SlotState currentState;

    public int SlotNumber => slotNumber;

    public bool IsEmpty =>
        currentState == SlotState.Empty;

    private void Awake()
    {
        slotImage = GetComponent<Image>();

        SetState(SlotState.Empty);
    }

    public void SetState(
        SlotState state)
    {
        currentState = state;

        switch (state)
        {
            case SlotState.Empty:
                slotImage.color = emptyColor;
                break;

            case SlotState.Available:
                slotImage.color = availableColor;
                break;

            case SlotState.Selected:
                slotImage.color = selectedColor;
                break;

            case SlotState.Unavailable:
                slotImage.color = unavailableColor;
                break;
        }
    }
}