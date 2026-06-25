using UnityEngine;

public class EarthShaderController : MonoBehaviour
{
    [SerializeField] private Material earthShaderModel;
    public static EarthShaderController Instance { get; private set; }
    private static readonly int cityLightsAmountID = Shader.PropertyToID("_CityLightsAmount"); // 1 Max all lights, 2 less, 3 least
    private static readonly int cityLightsBrightnessID = Shader.PropertyToID("_CityLightsBrightness"); // 0 barely shining, 2 normal, 3 ultra bright 
    private static readonly int earthSaturationID = Shader.PropertyToID("_EarthSaturation"); // 1.5 is vibrant - normal, 1. less vibrant, 0.8 bleak.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Singleton assignment
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateCityLights(EarthMetrics metrics, int currentYear)
    {
        if (earthShaderModel == null) return;   

        float startYear = 1760f;
        float targetMaxYear = 2026f; 

        // Clamp timeline progress strictly between 0 and 1
        float timelineProgress = Mathf.Clamp01((currentYear - startYear) / (targetMaxYear - startYear));

        float targetAmount = Mathf.Lerp(3f, 1f, timelineProgress);
        earthShaderModel.SetFloat(cityLightsAmountID, targetAmount);

        // The brightness of the existing grid is a direct factor of industrial capacity and power availability
        float gridPower = (metrics.IndustrialCapacity + metrics.energyAvailability) / 2f;

        float targetBrightness = Mathf.Lerp(0f, 3f, gridPower);
        earthShaderModel.SetFloat(cityLightsBrightnessID, targetBrightness);
    }
    public void UpdateEarthSaturation(EarthMetrics metrics)
    {
        if (earthShaderModel == null) return;
        float ecoVitality = (metrics.forestDensity + metrics.biodiversityHealth) / 2f;
        float targetSaturation = Mathf.Lerp(0.8f, 1.5f, ecoVitality);
        earthShaderModel.SetFloat(earthSaturationID, targetSaturation);
    }
}

