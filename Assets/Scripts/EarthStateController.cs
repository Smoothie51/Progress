using UnityEngine;

public class EarthStateController : MonoBehaviour
{
    [SerializeField] private EarthMetrics currentMetrics;
    
    // Backing fields for read-only properties
    private float _environmentalIntegrity;
    private float _technologicalAdvancement;

    // Public read-only properties
    public float EnvironmentalIntegrity => _environmentalIntegrity;
    public float TechnologicalAdvancement => _technologicalAdvancement;

    // Weights for Environmental Integrity calculation
    private const float WEIGHT_AIR_QUALITY = 0.20f;
    private const float WEIGHT_OCEAN_CLEANLINESS = 0.15f;
    private const float WEIGHT_FOREST_DENSITY = 0.15f;
    private const float WEIGHT_BIODIVERSITY_HEALTH = 0.15f;
    private const float WEIGHT_SOIL_FERTILITY = 0.10f;
    private const float WEIGHT_FRESHWATER_AVAILABILITY = 0.10f;
    private const float WEIGHT_MAGNETIC_FIELD_STRENGTH = 0.20f;
    // Weights for Technological Advancement calculation
    private const float WEIGHT_RESEARCH_CAPACITY = 0.30f;
    private const float WEIGHT_INDUSTRIAL_CAPACITY = 0.25f;
    private const float WEIGHT_ECONOMIC_GROWTH = 0.20f;
    private const float WEIGHT_ENERGY_AVAILABILITY = 0.15f;
    private const float WEIGHT_RESOURCE_AVAILABILITY = 0.10f;

    private void Awake()
    {
        // Initialize metrics to neutral values if not set
        if (currentMetrics.airQuality == 0)
        {
            ResetMetricsToDefaults();
        }
        
        RecalculateDerivedValues();
    }

    /// <summary>
    /// Applies a policy's metric modifiers to the current state and recalculates derived values.
    /// Call this when a policy node is clicked.
    /// </summary>
    public void ApplyPolicyModifier(PolicyNodeData policyNode)
    {
        if (policyNode == null) return;

        // Add the policy's metric shifts to current metrics
        currentMetrics.airQuality = Mathf.Clamp01(currentMetrics.airQuality + policyNode.metricShifts.airQuality);
        currentMetrics.oceanCleanliness = Mathf.Clamp01(currentMetrics.oceanCleanliness + policyNode.metricShifts.oceanCleanliness);
        currentMetrics.forestDensity = Mathf.Clamp01(currentMetrics.forestDensity + policyNode.metricShifts.forestDensity);
        currentMetrics.biodiversityHealth = Mathf.Clamp01(currentMetrics.biodiversityHealth + policyNode.metricShifts.biodiversityHealth);
        currentMetrics.soilFertility = Mathf.Clamp01(currentMetrics.soilFertility + policyNode.metricShifts.soilFertility);
        currentMetrics.freshwaterAvailability = Mathf.Clamp01(currentMetrics.freshwaterAvailability + policyNode.metricShifts.freshwaterAvailability);
        currentMetrics.climateVolatility = Mathf.Clamp01(currentMetrics.climateVolatility + policyNode.metricShifts.climateVolatility);

        currentMetrics.IndustrialCapacity = Mathf.Clamp01(currentMetrics.IndustrialCapacity + policyNode.metricShifts.IndustrialCapacity);
        currentMetrics.researchCapacity = Mathf.Clamp01(currentMetrics.researchCapacity + policyNode.metricShifts.researchCapacity);
        currentMetrics.economicGrowth = Mathf.Clamp01(currentMetrics.economicGrowth + policyNode.metricShifts.economicGrowth);
        currentMetrics.energyAvailability = Mathf.Clamp01(currentMetrics.energyAvailability + policyNode.metricShifts.energyAvailability);
        currentMetrics.resourceavailability = Mathf.Clamp01(currentMetrics.resourceavailability + policyNode.metricShifts.resourceavailability);

        currentMetrics.publicApproval = Mathf.Clamp01(currentMetrics.publicApproval + policyNode.metricShifts.publicApproval);

        // Recalculate derived values
        RecalculateDerivedValues();
    }

    /// <summary>
    /// Recalculates Environmental Integrity and Technological Advancement based on current metrics.
    /// </summary>
    private void RecalculateDerivedValues()
    {
        _environmentalIntegrity = CalculateEnvironmentalIntegrity();
        _technologicalAdvancement = CalculateTechnologicalAdvancement();
    }

    /// <summary>
    /// Calculates Environmental Integrity as a weighted average of environmental metrics.
    /// Returns a value strictly between 0.0f and 1.0f.
    /// </summary>
    private float CalculateEnvironmentalIntegrity()
    {
        float integrity = 
            (currentMetrics.airQuality * WEIGHT_AIR_QUALITY) +
            (currentMetrics.oceanCleanliness * WEIGHT_OCEAN_CLEANLINESS) +
            (currentMetrics.forestDensity * WEIGHT_FOREST_DENSITY) +
            (currentMetrics.biodiversityHealth * WEIGHT_BIODIVERSITY_HEALTH) +
            (currentMetrics.soilFertility * WEIGHT_SOIL_FERTILITY) +
            (currentMetrics.freshwaterAvailability * WEIGHT_FRESHWATER_AVAILABILITY) +
            (currentMetrics.magneticFieldStrength * WEIGHT_MAGNETIC_FIELD_STRENGTH);

        return Mathf.Clamp01(integrity);
    }

    /// <summary>
    /// Calculates Technological Advancement as a weighted average of technology metrics.
    /// Returns a value strictly between 0.0f and 1.0f.
    /// </summary>
    private float CalculateTechnologicalAdvancement()
    {
        float advancement = 
            (currentMetrics.researchCapacity * WEIGHT_RESEARCH_CAPACITY) +
            (currentMetrics.IndustrialCapacity * WEIGHT_INDUSTRIAL_CAPACITY) +
            (currentMetrics.economicGrowth * WEIGHT_ECONOMIC_GROWTH) +
            (currentMetrics.energyAvailability * WEIGHT_ENERGY_AVAILABILITY) +
            (currentMetrics.resourceavailability * WEIGHT_RESOURCE_AVAILABILITY);

        return Mathf.Clamp01(advancement);
    }

    /// <summary>
    /// Resets all metrics to neutral/starting values (0.5f).
    /// </summary>
    private void ResetMetricsToDefaults()
    {
        currentMetrics.airQuality = 0.95f;             // Crisp, unpolluted skies globally
        currentMetrics.oceanCleanliness = 0.95f;       // Clean waters completely free of industrial runoff
        currentMetrics.forestDensity = 0.85f;          // Massive old-growth forests cover the landmasses
        currentMetrics.biodiversityHealth = 0.90f;     // Rich, uninterrupted wildlife biomes
        currentMetrics.soilFertility = 0.80f;          // High organic nutrient content via traditional crop rotation
        currentMetrics.freshwaterAvailability = 0.85f; // Unpolluted rivers, though distribution is manual
        currentMetrics.climateVolatility = 0.10f;      // Very low; stable global weather baseline patterns
        currentMetrics.magneticFieldStrength = 0.90f;  // Strong, stable magnetic field protecting the planet

        currentMetrics.IndustrialCapacity = 0.05f;     // Low; cottage industries running on manual looms/forges
        currentMetrics.researchCapacity = 0.15f;       // Limited; pre-scientific method expansion, manual libraries
        currentMetrics.economicGrowth = 0.08f;         // Slow, agrarian-based mercantile economy
        currentMetrics.energyAvailability = 0.10f;     // Dependent on timber, charcoal, and watermills
        currentMetrics.resourceavailability = 0.75f;   // High; vast coal veins and timber stocks sit completely untapped

        // === SOCIO-ECONOMIC ===
        currentMetrics.publicApproval = 0.65f;         // Stable but agrarian lifestyle baseline; highly vulnerable to crop failures
    }

    public EarthMetrics GetCurrentMetrics()
    {
        return currentMetrics;
    }

    public void SetMetrics(EarthMetrics metrics)
    {
        currentMetrics = metrics;
        currentMetrics.ClampMetrics();
        RecalculateDerivedValues();
    }
}
