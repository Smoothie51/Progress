using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance { get; private set; }

    [Header("UI Element Connections")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI positiveModifierText;
    [SerializeField] private TextMeshProUGUI negativeModifierText;
    [SerializeField] private RectTransform rectTransform;

    [Header("Layout Tuning")]
    [SerializeField] private Vector2 cursorOffset = new Vector2(15f, -15f);

    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        rectTransform.position = mousePosition + cursorOffset;
    }

    public void ShowTooltip(string title, Sprite icon, string description, string positiveModifier = "", string negativeModifier = "")
    {
        gameObject.SetActive(true);
        titleText.text = title;
        iconImage.sprite = icon;
        descriptionText.text = description;
        positiveModifierText.text = positiveModifier;
        negativeModifierText.text = negativeModifier;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void FollowCursor(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        rectTransform.position = mousePosition + cursorOffset;
    }
}