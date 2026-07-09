using UnityEngine;
using UnityEngine.EventSystems;

public class PolicyMapInput : MonoBehaviour, IDragHandler, IScrollHandler
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.01f;
    public Vector3 minScale = new Vector3(0.3f, 0.3f, 1f);
    public Vector3 maxScale = new Vector3(5f, 5f, 1f);

    private RectTransform rectTransform;

    [SerializeField] private RectTransform viewportTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnScroll(PointerEventData eventData)
    {
        float scrollInput = eventData.scrollDelta.y;

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out Vector2 mouseLocalBeforeScale
            );

            // 2. CALCULATE SCALE CHANGE
            Vector3 oldScale = rectTransform.localScale;
            Vector3 newScale = oldScale + Vector3.one * scrollInput * zoomSpeed;

            // Clamp your values safely
            newScale.x = Mathf.Clamp(newScale.x, minScale.x, maxScale.x);
            newScale.y = Mathf.Clamp(newScale.y, minScale.y, maxScale.y);
            newScale.z = 1f;

            // 3. EXECUTE SCALE UPDATE
            rectTransform.localScale = newScale;

            // 4. MOUSE POSITION SHIFT CORRECTION: 
            // Calculate how much the pixel position under the mouse changed due to the scale jump
            Vector2 positionOffset = mouseLocalBeforeScale * (newScale.x - oldScale.x);

            // Subtract that offset vector straight from your position matrix to pin the map under the cursor!
            rectTransform.anchoredPosition -= positionOffset;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || viewportTransform == null) return;
        Vector2 targetPosition = rectTransform.anchoredPosition + eventData.delta;

        float contentWidth = rectTransform.rect.width * rectTransform.localScale.x;
        float contentHeight = rectTransform.rect.height * rectTransform.localScale.y;
        float viewWidth = viewportTransform.rect.width;
        float viewHeight = viewportTransform.rect.height;
        float maxHorizontalShift = Mathf.Max(0f, (contentWidth - viewWidth) * 0.5f);
        float maxVerticalShift = Mathf.Max(0f, (contentHeight - viewHeight) * 0.5f);

        
        targetPosition.x = Mathf.Clamp(targetPosition.x, -maxHorizontalShift, maxHorizontalShift);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -maxVerticalShift, maxVerticalShift);

        
        rectTransform.anchoredPosition = targetPosition;
    }
}
