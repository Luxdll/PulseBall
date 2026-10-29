using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.1f;

    public Vector2 Direction { get; private set; }

    private RectTransform background;
    private bool isDragging;
    private int activePointerId;

    private void Awake()
    {
        background = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDragging || handle == null)
            return;

        isDragging = true;
        activePointerId = eventData.pointerId;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || eventData.pointerId != activePointerId)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position,
            eventData.pressEventCamera, out Vector2 localPoint))
            return;

        float radius = Mathf.Max(1f,
            (Mathf.Min(background.rect.width, background.rect.height)
            - Mathf.Max(handle.rect.width, handle.rect.height)) * 0.5f);

        Vector2 input = Vector2.ClampMagnitude(localPoint / radius, 1f);

        handle.anchoredPosition = input * radius;
        Direction = input.magnitude < deadZone ? Vector2.zero : input;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && eventData.pointerId == activePointerId)
            ResetInput();
    }

    private void OnDisable()
    {
        ResetInput();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            ResetInput();
    }

    private void ResetInput()
    {
        isDragging = false;
        Direction = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}