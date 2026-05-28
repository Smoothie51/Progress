using UnityEngine;

public class SmogController : MonoBehaviour
{
    [Header("Atmosphere Reference")]
    [SerializeField] private Renderer smogRenderer;

    [Header("Pristine Sky Settings (High Environmental Integrity)")]
    [SerializeField] private Color cleanSkyColor = new Color(0.3f, 0.6f, 0.9f, 0.05f); // Soft, thin blue halo

    [Header("Toxic Smog Settings (Low Environmental Integrity)")]
    [SerializeField] private Color toxicSmogColor = new Color(0.45f, 0.35f, 0.25f, 0.75f); // Dense industrial brown

    private Material smogMaterial;

    void Start()
    {
        if (smogRenderer != null)
        {
            // Use .material to instantiate a local copy so it doesn't overwrite your project asset permanently
            smogMaterial = smogRenderer.material;
        }
        else
        {
            Debug.LogError("Smog Visuals Controller: Smog Renderer reference is missing!");
        }
    }

    /// <summary>
    /// Updates the atmosphere's visual look based on a 0 to 1 decay value.
    /// </summary>
    /// <param name="ecologicalDecay">0 = Pristine/Clean, 1 = Max Pollution/Choked</param>
    public void SetSmogLevel(float ecologicalDecay)
    {
        if (smogMaterial == null) return;

        // Clamp the input value between 0 and 1 just to be safe
        ecologicalDecay = Mathf.Clamp01(ecologicalDecay);

        // Linearly interpolate (Lerp) color and transparency smoothly based on decay severity
        Color targetColor = Color.Lerp(cleanSkyColor, toxicSmogColor, ecologicalDecay);
        
        // Push the calculated color directly to the material
        smogMaterial.color = targetColor;
    }
}