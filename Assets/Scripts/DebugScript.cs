using UnityEngine;

public class AtmoDebugger : MonoBehaviour
{
    [Header("Target Controller")]
    [SerializeField] private GameObject GameMaster;
    private AtmosphereController Atmosphere;
    private AuroraController Aurora;

    [ColorUsage(showAlpha: true, hdr: true)]
    [SerializeField] private Color Color;
    [Range(0f, 1f)] public float Thickness = 0f;
    [Range(0f, 1f)] public float Opacity = 0f;
    [SerializeField] private float WindSpeed;

    [Range(0f, 1f)] public float magneticFieldStrength;
    [Range(0f, 1f)] public float climateVolatility;

    [SerializeField] public bool Ranged = true;

    void Start()
    {
        if (GameMaster != null)
        {
            Atmosphere = GameMaster.GetComponent<AtmosphereController>();
            Aurora = GameMaster.GetComponent<AuroraController>();
        }
    }

    void Update()
    {
        if (Atmosphere == null) return;

        if (Ranged)
        {
            //Atmosphere.UpdateAtmosphere(Color, Thickness, Opacity, WindSpeed);
        }
        if (Aurora != null)
        {
            Aurora.UpdateAuroraIntensity(magneticFieldStrength);
        }


    }
}