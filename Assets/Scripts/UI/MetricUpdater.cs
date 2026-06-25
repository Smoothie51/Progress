using UnityEngine;
using UnityEngine.UI;

public class MetricUpdater : MonoBehaviour
{
    [Header("Environmental Sliders")]
    [SerializeField] private Slider airQualitySlider;
    [SerializeField] private Slider oceanCleanlinessSlider;
    [SerializeField] private Slider forestDensitySlider;
    [SerializeField] private Slider biodiversitySlider;
    [SerializeField] private Slider magneticFieldSlider;
    [SerializeField] private Slider volatilitySlider;

    [Header("Industrial & Socio Sliders")]
    [SerializeField] private Slider industrialCapacitySlider;
    [SerializeField] private Slider researchCapacitySlider;
    [SerializeField] private Slider economicGrowthSlider;
    [SerializeField] private Slider energyAvailabilitySlider;
    [SerializeField] private Slider resourceAvailabilitySlider;
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

    /// <summary>
    /// Event receiver loop. Updates visual fills only when the backend data actually mutates!
    /// </summary>
    private void RefreshSliderUI(EarthMetrics metrics)
    {
        // Update all environmental value points safely
        if (airQualitySlider != null) airQualitySlider.value = metrics.airQuality;
        if (oceanCleanlinessSlider != null) oceanCleanlinessSlider.value = metrics.oceanCleanliness;
        if (forestDensitySlider != null) forestDensitySlider.value = metrics.forestDensity;
        if (biodiversitySlider != null) biodiversitySlider.value = metrics.biodiversityHealth;
        if (magneticFieldSlider != null) magneticFieldSlider.value = metrics.magneticFieldStrength;
        if (volatilitySlider != null) volatilitySlider.value = metrics.climateVolatility;

        // Update industrial/socio points safely
        if (industrialCapacitySlider != null) industrialCapacitySlider.value = metrics.IndustrialCapacity;
        if (researchCapacitySlider != null) researchCapacitySlider.value = metrics.researchCapacity;
        if (economicGrowthSlider != null) economicGrowthSlider.value = metrics.economicGrowth;
        if (energyAvailabilitySlider != null) energyAvailabilitySlider.value = metrics.energyAvailability;
        if (resourceAvailabilitySlider != null) resourceAvailabilitySlider.value = metrics.resourceAvailability;    
    }
}