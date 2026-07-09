using UnityEngine;
using TMPro; 

public class CampaignTimeManager : MonoBehaviour
{
    public static CampaignTimeManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI yearTextDisplay;

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

        EarthStateController.Instance.AdvancePolicy();
        PolicyTreeManager.Instance.GeneratePoints();
        PolicyTreeManager.Instance.RefreshTreeLayout();
        Debug.Log("Year Advanced");
        // Dynamic Era Check Example
        if (currentYear == 2000)
        {
            Debug.Log("Welcome to the 4th Industrial Revolution!");
            // Trigger 4IR specific graphics or global baseline updates here
        }
    }

    void UpdateUI()
    {
        if (yearTextDisplay != null)
        {
            yearTextDisplay.text = $"YEAR: {currentYear}";
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
