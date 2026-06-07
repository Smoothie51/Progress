using UnityEngine;
using UnityEngine.EventSystems;

public class PolicyMapInput : MonoBehaviour, IDragHandler, IScrollHandler
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.01f;
    public Vector3 minScale = new Vector3(0.3f, 0.3f, 1f);
    public Vector3 maxScale = new Vector3(5f, 5f, 1f);

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnScroll(PointerEventData eventData)
    {
        float scrollInput = eventData.scrollDelta.y;

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            Vector3 newScale = rectTransform.localScale + Vector3.one * scrollInput * zoomSpeed;

            // Clamp the scale so it doesn't get too big or too small
            newScale.x = Mathf.Clamp(newScale.x, minScale.x, maxScale.x);
            newScale.y = Mathf.Clamp(newScale.y, minScale.y, maxScale.y);
            newScale.z = 1f;

            rectTransform.localScale = newScale;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta;

        rectTransform.anchoredPosition += delta;
    }
}
