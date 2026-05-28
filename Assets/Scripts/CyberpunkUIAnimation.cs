using UnityEngine;

public class CyberpunkUIAnimations : MonoBehaviour
{
    [Header("Animation Speeds")]
    [SerializeField] private float horizontalStageDuration = 0.30f;
    [SerializeField] private float verticalStageDuration = 0.25f;

    private RectTransform rectTransform;
    private readonly float lineThicknessBaseline = 0.005f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0f, lineThicknessBaseline, 1f);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        LeanTween.cancel(gameObject);

        // 1. Line shoots out across the screen
        LeanTween.scaleX(gameObject, 1f, horizontalStageDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => 
            {
                // 2. Explode vertically into full panel
                LeanTween.scaleY(gameObject, 1f, verticalStageDuration)
                    .setEase(LeanTweenType.easeOutExpo)
                    .setIgnoreTimeScale(true);
            });
    }

    public void Close()
    {
        LeanTween.cancel(gameObject);

        // 1. Crush panel down to the thin baseline line
        LeanTween.scaleY(gameObject, lineThicknessBaseline, verticalStageDuration)
            .setEase(LeanTweenType.easeInExpo)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => 
            {
                // 2. Collapse line horizontally to nothing
                LeanTween.scaleX(gameObject, 0f, horizontalStageDuration)
                    .setEase(LeanTweenType.easeInQuad)
                    .setIgnoreTimeScale(true)
                    .setOnComplete(() => gameObject.SetActive(false));
            });
    }
}