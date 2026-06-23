using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow :
    UIWindow,
    IBeginDragHandler,
    IDragHandler
    
{
    private RectTransform rectTransform;

    private Vector2 offset;

    private void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();
    }

    public void OnBeginDrag(
        PointerEventData eventData)
    {
        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out offset);
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        RectTransform parent =
            rectTransform.parent as RectTransform;

        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 pos);

        rectTransform.localPosition =
            pos - offset;
    }
}