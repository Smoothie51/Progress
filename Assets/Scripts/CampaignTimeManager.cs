using UnityEngine;
using TMPro; 

public class CampaignTimeManager : MonoBehaviour
{
    public static CampaignTimeManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI yearTextDisplay;
    [SerializeField] private GameObject EndScreenUI;
    [SerializeField] private TextMeshProUGUI EndScoreText;

    [Header("Calendar Settings (1x Speed)")]
    public int currentYear = 1760;
    [Tooltip("How many real-world seconds does it take for 1 Year to pass at 1x speed?")]
    [SerializeField] private float secondsPerYearBase = 50.0f; 

    [Header("Current Rotations (Degrees Per Second)")]
    public float currentSunOrbitSpeed;
    public float currentMoonOrbitSpeed;


    private float yearProgressTimer = 0f;

    void Awake()
    {
        // Simple Singleton pattern so other scripts can easily check the current year
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        CalculateRotationSpeeds();
    }

    void Update()
    {
        // 1. Progress time using Time.deltaTime (automatically scales with x1, x2, x4, or Pause)
        if (Time.timeScale > 0)
        {
            yearProgressTimer += Time.deltaTime;

            if (yearProgressTimer >= secondsPerYearBase)
            {
                yearProgressTimer -= secondsPerYearBase;
                currentYear++;
                OnYearAdvanced();
            }
        }
    }

    void OnYearAdvanced()
    {
        UpdateUI();
        if (currentYear >= 2026)
        {
            float[] scores = EarthStateController.Instance.CalculateFinalScore();
            EndScreenUI.SetActive(true);
            EndScoreText.text = $"Final Score: <color=#FFD700>{scores[2]:F2}</color>\nEnvironmental Score: <color=green>{scores[0]:F2}</color>\nTechnological Score: <color=#4169E1>{scores[1]:F2}</color>";
            Time.timeScale = 0f;
        }

        EarthStateController.Instance.AdvancePolicy();
        PolicyTreeManager.Instance.GeneratePoints();
        PolicyTreeManager.Instance.RefreshTreeLayout();
    }

    void UpdateUI()
    {
        if (yearTextDisplay != null)
        {
            if (currentYear > 2000)
                yearTextDisplay.text = $"YEAR: <color=red> {currentYear}</color> / 2026";
            else
                yearTextDisplay.text = $"YEAR: {currentYear} / 2026";
        }
    }

    // Calculates exactly how fast your planets need to spin based on the calendar speed
    void CalculateRotationSpeeds()
    {
        // 360 degrees divided by how long a year lasts
        currentSunOrbitSpeed = 90f / secondsPerYearBase;
        
        // Moon moves 12.4x faster than Earth's annual cycle
        currentMoonOrbitSpeed = currentSunOrbitSpeed * 3.5f;
    }
}
