using UnityEngine;
using UnityEngine.UI;

public class MetricUpdater : MonoBehaviour
{
    [System.Serializable]
    public struct SliderGroup
    {
        public Slider slider;
        public Image fillImage; // Drag the "Fill" GameObject image component here!
    }
    [Header("Environmental Sliders")]
    [SerializeField] private SliderGroup airQualitySlider;
    [SerializeField] private SliderGroup oceanCleanlinessSlider;
    [SerializeField] private SliderGroup forestDensitySlider;
    [SerializeField] private SliderGroup biodiversitySlider;
    [SerializeField] private SliderGroup magneticFieldSlider;
    [SerializeField] private SliderGroup volatilitySlider;

    [Header("Industrial & Socio Sliders")]
    [SerializeField] private SliderGroup industrialCapacitySlider;
    [SerializeField] private SliderGroup researchCapacitySlider;
    [SerializeField] private SliderGroup economicGrowthSlider;
    [SerializeField] private SliderGroup energyAvailabilitySlider;
    [SerializeField] private SliderGroup resourceAvailabilitySlider;

    [Header("Compound Sliders")]
    [SerializeField] private SliderGroup environmentIntegrity;
    [SerializeField] private SliderGroup technologicalAdvancement;

    [Header("Color Gradient Presets")]
    [SerializeField] private Gradient healthColorGradient;
    private void OnEnable()
    {
        // Subscribe to the event when this menu is turned on/opened
        EarthStateController.OnMetricsUpdated += RefreshSliderUI;
        
        // Initial setup pass so sliders aren't empty when first opening the screen
        if (EarthStateController.Instance != null)
        {
            Debug.Log("TESTING");
            RefreshSliderUI(EarthStateController.Instance.currentMetrics);
        }
    }

    private void OnDisable()
    {
        // Unsubscribe when the menu is closed to prevent nasty memory leaks
        EarthStateController.OnMetricsUpdated -= RefreshSliderUI;
    }

    void Start()
    {
        RefreshSliderUI(EarthStateController.Instance.currentMetrics);
    }

    void Update()
    {
        refreshCompoundedSliders();
    }

    private void RefreshSliderUI(EarthMetrics metrics)
    {
        // Route values and apply dynamic color grading instantly
        UpdateSliderState(airQualitySlider, metrics.airQuality, isInverted: false);
        UpdateSliderState(oceanCleanlinessSlider, metrics.oceanCleanliness, isInverted: false);
        UpdateSliderState(forestDensitySlider, metrics.forestDensity, isInverted: false);
        UpdateSliderState(biodiversitySlider, metrics.biodiversityHealth, isInverted: false);
        UpdateSliderState(magneticFieldSlider, metrics.magneticFieldStrength, isInverted: false);
        UpdateSliderState(volatilitySlider, metrics.climateVolatility, isInverted: true);

        //
        UpdateSliderState(industrialCapacitySlider, metrics.IndustrialCapacity, isInverted: false);
        UpdateSliderState(researchCapacitySlider, metrics.researchCapacity, isInverted: false);
        UpdateSliderState(economicGrowthSlider, metrics.economicGrowth, isInverted: false);
        UpdateSliderState(energyAvailabilitySlider, metrics.energyAvailability, isInverted: false);
        UpdateSliderState(resourceAvailabilitySlider, metrics.resourceAvailability, isInverted: false);
    }

    private void UpdateSliderState(SliderGroup group, float rawValue, bool isInverted)
    {
        if (group.slider == null) return;

        // Set the fill value instantly
        group.slider.value = rawValue;

        // Apply dynamic color shift if a fill reference is provided
        if (group.fillImage != null)
        {
            // Evaluate color based on inversion layout rules
            float evaluationTime = isInverted ? (1f - rawValue) : rawValue;
            group.fillImage.color = healthColorGradient.Evaluate(evaluationTime);
        }
    }

    private void refreshCompoundedSliders()
    {
        UpdateSliderState(environmentIntegrity,EarthStateController.Instance.EnvironmentalIntegrity, false);
        UpdateSliderState(technologicalAdvancement,EarthStateController.Instance.TechnologicalAdvancement, false);

    }
}