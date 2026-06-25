using UnityEngine;

public class AtmosphereController : MonoBehaviour
{
    [SerializeField] private Material atmoMaterial;
    public static AtmosphereController Instance { get; private set; }
    private static readonly int ColorID = Shader.PropertyToID("_Color");
    private static readonly int ThicknessID = Shader.PropertyToID("_Thickness"); //1 normal // 1.01 slightly thicker // 1.05 medium // 1.1 thick // >1.1 not much difference
    private static readonly int OpacityID = Shader.PropertyToID("_Opacity"); //0 not visible // 0.1 barely visible // 0.4 visble against backgroun // 1 sharp 
    private static readonly int WindSpeedID = Shader.PropertyToID("_WindSpeed"); // stay at one

    [Header("Atmosphere Color Presets")]

    [ColorUsage(true, true)] public Color pristineAtmoColor = new Color(0.3f, 0.6f, 1.0f, 1.0f); // Clean Cyan-Blue Glow
    [ColorUsage(true, true)] public Color pollutedAtmoColor = new Color(0.6f, 0.45f, 0.3f, 1.0f); // Sickly Amber-Brown Smog

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void UpdateAtmosphere(EarthMetrics metrics)
    {
        if (atmoMaterial == null) return;

        // 1. CALCULATE THICKNESS: Maps from 1.0 (normal) up to 1.1 (thick storm envelope)
        // Driven entirely by climate volatility expansion
        float targetThickness = Mathf.Lerp(1.0f, 1.1f, metrics.climateVolatility);
        atmoMaterial.SetFloat(ThicknessID, targetThickness);

        // 2. CALCULATE OPACITY: Maps from 0.4 (soft/clear) up to 1.0 (sharp/choked)
        // As air quality drops, the atmosphere becomes intensely thick and dense with soot
        float targetOpacity = Mathf.Lerp(1.0f, 0.4f, metrics.airQuality);
        atmoMaterial.SetFloat(OpacityID, targetOpacity);

        // 3. CALCULATE COLOR: Smoothly shifts from vibrant blue to toxic industrial amber
        Color dynamicColor = Color.Lerp(pollutedAtmoColor, pristineAtmoColor, metrics.airQuality);
        atmoMaterial.SetColor(ColorID, dynamicColor);

        // 4. WIND SPEED: Kept stable at 1.0f as specified by your shader requirements
        atmoMaterial.SetFloat(WindSpeedID, 1.0f);
    }
}