using UnityEngine;

public class AtmoDebugger : MonoBehaviour
{
    [Header("Target Controller")]
    [SerializeField] private AtmosphereController Atmosphere;

    [SerializeField] private Color Color;
    [Range(0f, 1f)] public float Thickness = 0f;
    [Range(0f, 1f)] public float Opacity = 0f;
    [SerializeField] private float WindSpeed;

    [SerializeField] public bool Ranged = true;

    void Update()
    {
        if (Atmosphere == null) return;

        if (Ranged)
        {
            Atmosphere.UpdateAtmosphere(Color, Thickness, Opacity, WindSpeed);
        }
        

    }

    [ContextMenu("Set Pristine Atmosphere")]  
    public void SetPristineAtmosphere()
    {
        Color = new Color(0.3f, 0.6f, 0.9f, 1.0f);
        Thickness = 0.3f;   // 0.3 maps beautifully toward a softer, wide blue gas bloom
        Opacity = 0.25f;    // Light, clear baseline density
        WindSpeed = 0.01f;  // Peaceful, slow drift

        Atmosphere.UpdateAtmosphere(Color, Thickness, Opacity, WindSpeed);
    }

    [ContextMenu("Set Heavy Industrial Atmosphere")]
    public void SetHeavyIndustrialAtmosphere()
    {
        Color = new Color(0.48f, 0.42f, 0.35f, 1.0f);
        Thickness = 0.8f;   // 0.8 compresses the haze tighter toward the surface
        Opacity = 0.75f;    // Highly opaque, dark, and choking smoke representation
        WindSpeed = 0.08f;  // Fast, turbulent cloud dispersal acceleration

        Atmosphere.UpdateAtmosphere(Color, Thickness, Opacity, WindSpeed);
    }

    [ContextMenu("Set Advanced Shield Atmosphere")]
    public void SetAdvancedShieldAtmosphere()
    {
        Color = new Color(0.2f, 0.8f, 0.7f, 1.0f);
        Thickness = 0.95f;  // 0.95 forces it into an ultra-thin, sharp futuristic boundary ring
        Opacity = 0.4f;     // Clean but visibly energetic emission line
        WindSpeed = 0.04f;  // Controlled, steady particle flow rate

        Atmosphere.UpdateAtmosphere(Color, Thickness, Opacity, WindSpeed);
    }
}