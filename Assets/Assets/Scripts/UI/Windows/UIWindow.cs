using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);

        transform.SetAsLastSibling();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Focus()
    {
        transform.SetAsLastSibling();
    }
}