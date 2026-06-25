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

    public void UpdateAuroraIntensity(EarthMetrics metrics)
    {
        if (auroraMaterial == null) return;

        float clampedStrength = Mathf.Clamp01(metrics.magneticFieldStrength);

        float dissolveTarget = Mathf.Lerp(0.8f, 6f, clampedStrength);

        float opacityTarget = Mathf.Lerp(1.0f, 0.4f, clampedStrength);

        if (clampedStrength > 0.99f)
        {
            dissolveTarget = 20f;
            opacityTarget = 0f;
        }

        auroraMaterial.SetFloat(AuroraIntensity, dissolveTarget);
        auroraMaterial.SetFloat(AuroraOpacity, opacityTarget);
    }
}