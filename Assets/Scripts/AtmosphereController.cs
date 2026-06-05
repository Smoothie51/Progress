using UnityEngine;

public class AtmosphereController : MonoBehaviour
{
    [Header("Atmosphere Mesh Reference")]
    [SerializeField] private Renderer atmoRenderer;

    private Material atmoMaterial;

    private static readonly int ColorID = Shader.PropertyToID("_Color");
    private static readonly int ThicknessID = Shader.PropertyToID("_Thickness");
    private static readonly int OpacityID = Shader.PropertyToID("_Opacity");
    private static readonly int WindSpeedID = Shader.PropertyToID("_WindSpeed");

    void Start()
    {
        if (atmoRenderer != null)
        {
            atmoMaterial = atmoRenderer.material;
        }
        else
        {
            Debug.LogError("Atmosphere Controller: Missing Renderer Reference!");
        }
    }
    public void UpdateAtmosphere(Color targetColor, float thickness, float opacity, float windSpeed)
    {
        if (atmoMaterial == null) return;

        // Apply properties to the Shader Graph Material
        atmoMaterial.SetColor(ColorID, targetColor);
        atmoMaterial.SetFloat(ThicknessID, thickness); // Prevent breaking fresnel math
        atmoMaterial.SetFloat(OpacityID, opacity);
        atmoMaterial.SetFloat(WindSpeedID, windSpeed);
    }
}