using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private Material cloudMaterial;
    public static CloudController Instance { get; private set; }
    private static readonly int cloudColor = Shader.PropertyToID("_CloudColor"); 
    private static readonly int cloudDensity = Shader.PropertyToID("_CloudDensity"); // 2 minimal cloud // 1.5 normal // 1 cloudy // 0.5 max cloud 
    private static readonly int cloudSpeed = Shader.PropertyToID("_CloudSpeed");  //0.0005 slow wind // 0.01 typhoon

    [Header("Historical Baseline Color Adjustments")]
    
    [ColorUsage(true,true)] public Color pristineCloudColor = new Color(0.9f, 0.9f, 0.9f, 1.0f); // Bright clean white-gray
    [ColorUsage(true,true)] public Color industrialSmogColor = new Color(0.4f, 0.38f, 0.35f, 1.0f); // Sooty, heavy charcoal-brown gray

    private void Awake()
    {
        // Singleton assignment
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateCloud(EarthMetrics metrics)
    {
        if (cloudMaterial == null) return;

        float environmentalStress = (metrics.climateVolatility + (1f - metrics.airQuality)) / 2f;
        environmentalStress = Mathf.Clamp01(environmentalStress);
        float targetDensity = Mathf.Lerp(2.0f, 0.5f, environmentalStress);
        cloudMaterial.SetFloat(cloudDensity, targetDensity);

        Color dynamicCloudColor = Color.Lerp(industrialSmogColor, pristineCloudColor, metrics.airQuality);
        cloudMaterial.SetColor(cloudColor, dynamicCloudColor);

        float speedCurve = Mathf.Pow(metrics.climateVolatility, 3f); // Volatility cubed
        
        // Map 0.0 to a lazy crawl (0.0002) and 1.0 to a jupiter-level disaster typhoon (0.01)
        float targetSpeed = Mathf.Lerp(0.0005f, 0.01f, speedCurve);
        cloudMaterial.SetFloat(cloudSpeed, targetSpeed);
    }
}
