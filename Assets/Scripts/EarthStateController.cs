using UnityEngine;
using System.Collections.Generic;

public class EarthStateController : MonoBehaviour
{
    public static EarthStateController Instance { get; private set; }
    [SerializeField] private EarthMetrics currentMetrics;

    [Header("Active Legislation Trackers")]
    [SerializeField] private List<PolicyEnacting> activeEnactingPolicies = new List<PolicyEnacting>();
    public HashSet<PolicyNodeData> fullyImplementedPolicies = new HashSet<PolicyNodeData>();
    
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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // Initialize metrics to neutral values if not set
        if (currentMetrics.airQuality == 0)
        {
            ResetMetricsToDefaults();
        }
        
        RecalculateDerivedValues();
    }

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
        currentMetrics.resourceAvailability = Mathf.Clamp01(currentMetrics.resourceAvailability + policyNode.metricShifts.resourceAvailability);

        currentMetrics.publicApproval = Mathf.Clamp01(currentMetrics.publicApproval + policyNode.metricShifts.publicApproval);

        // Recalculate derived values
        RecalculateDerivedValues();
    }
    private void RecalculateDerivedValues()
    {
        _environmentalIntegrity = CalculateEnvironmentalIntegrity();
        _technologicalAdvancement = CalculateTechnologicalAdvancement();
    }
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
    private float CalculateTechnologicalAdvancement()
    {
        float advancement = 
            (currentMetrics.researchCapacity * WEIGHT_RESEARCH_CAPACITY) +
            (currentMetrics.IndustrialCapacity * WEIGHT_INDUSTRIAL_CAPACITY) +
            (currentMetrics.economicGrowth * WEIGHT_ECONOMIC_GROWTH) +
            (currentMetrics.energyAvailability * WEIGHT_ENERGY_AVAILABILITY) +
            (currentMetrics.resourceAvailability * WEIGHT_RESOURCE_AVAILABILITY);

        return Mathf.Clamp01(advancement);
    }
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
        currentMetrics.resourceAvailability = 0.75f;   // High; vast coal veins and timber stocks sit completely untapped

        // === SOCIO-ECONOMIC ===
        currentMetrics.publicApproval = 0.65f;         // Stable but agrarian lifestyle baseline; highly vulnerable to crop failures
    }

    public void ProcessPolicySignature(PolicyNodeData signedPolicy)
    {
        // 1. Instead of modifying variables immediately, wrap it into our active incremental tracer
        PolicyEnacting newEnactment = new PolicyEnacting(signedPolicy);
        activeEnactingPolicies.Add(newEnactment);

        // 2. Refresh the UI Tree layout immediately so neighbor nodes know a path has been opened/started
        // (If your tree requirements state a node must be fully complete to progress, move this step!)
        if (PolicyTreeManager.Instance != null)
        {
            PolicyTreeManager.Instance.RefreshTreeLayout();
        }
    }
    public void OnCalendarYearAdvanced()
    {
        if (activeEnactingPolicies.Count == 0) return;

        // Process backward through the array so we can safely remove items while looping
        for (int i = activeEnactingPolicies.Count - 1; i >= 0; i--)
        {
            PolicyEnacting activePolicy = activeEnactingPolicies[i];

            // Apply this year's incremental slice to your active structural dataset
            currentMetrics.airQuality += activePolicy.incrementalShiftsPerYear.airQuality;
            currentMetrics.oceanCleanliness += activePolicy.incrementalShiftsPerYear.oceanCleanliness;
            currentMetrics.forestDensity += activePolicy.incrementalShiftsPerYear.forestDensity;
            currentMetrics.biodiversityHealth += activePolicy.incrementalShiftsPerYear.biodiversityHealth;
            currentMetrics.magneticFieldStrength += activePolicy.incrementalShiftsPerYear.magneticFieldStrength;
            currentMetrics.climateVolatility += activePolicy.incrementalShiftsPerYear.climateVolatility;
            // ... apply your remaining fields ...

            activePolicy.yearsRemaining--;

            // If the duration hits zero, the policy is officially completed!
            if (activePolicy.yearsRemaining <= 0)
            {
                fullyImplementedPolicies.Add(activePolicy.policyData);
                activeEnactingPolicies.RemoveAt(i);
                Debug.Log($"<color=green><b>Policy Alert:</b> {activePolicy.policyData.policyName} has been 100% phased in!</color>");
            }
        }

        // Keep values structurally verified inside 0-1 bounds
        currentMetrics.ClampMetrics();

        // Push new fractional averages to your Atmosphere, Aurora, and Terrain surface shaders/particles!
        RecalculateDerivedValues();
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
