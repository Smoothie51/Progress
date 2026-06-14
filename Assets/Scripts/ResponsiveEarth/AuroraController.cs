using UnityEngine;

public class AuroraController : MonoBehaviour
{
    [Header("Aurora Presentation Links")]
    [SerializeField] private Material auroraMaterial;

    public static AuroraController Instance { get; private set; }

    private static readonly int AuroraIntensity = Shader.PropertyToID("_DissolvePower");
    private static readonly int AuroraOpacity = Shader.PropertyToID("_Opacity");


    private void Awake()
    {
        // Singleton assignment
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateAuroraIntensity(float magneticFieldStrength)
    {
        if (auroraMaterial == null) return;

        float clampedStrength = Mathf.Clamp01(magneticFieldStrength);

        // 1. DISSOLVE LOGIC 
        // Weak shield (0.0) -> Low dissolve (0.8f) -> Full, vibrant aurora shape
        // Strong shield (1.0) -> High dissolve (10.0f) -> Dissolved away
        float dissolveTarget = Mathf.Lerp(0.8f, 6f, clampedStrength);

        // 2. OPACITY LOGIC
        // Weak shield (0.0) -> High opacity (1.0f) -> Fully visible
        // Strong shield (1.0) -> Low opacity (0.0f) -> Completely gone
        float opacityTarget = Mathf.Lerp(1.0f, 0.4f, clampedStrength);

        // If the shield is strong enough (e.g., above 0.9), make sure the aurora is 100% gone
        if (clampedStrength > 0.99f)
        {
            dissolveTarget = 20f;
            opacityTarget = 0f;
        }

        // Apply to the material
        auroraMaterial.SetFloat(AuroraIntensity, dissolveTarget);
        auroraMaterial.SetFloat(AuroraOpacity, opacityTarget);
    }
}