using UnityEngine;
using System.Collections.Generic;
using System;

public class EarthStateController : MonoBehaviour
{
    public static EarthStateController Instance { get; private set; }
    [SerializeField] public EarthMetrics currentMetrics;

    [Header("Active Legislation Trackers")]
    [SerializeField] private List<PolicyEnacting> activeEnactingPolicies = new List<PolicyEnacting>();
    public HashSet<PolicyNodeData> fullyImplementedPolicies = new HashSet<PolicyNodeData>();

    public static event Action<EarthMetrics> OnMetricsUpdated;
    
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
    }

    public void Start()
    {
        RecalculateDerivedValues();
    }

    public void Update(){
        OnMetricsUpdated?.Invoke(currentMetrics);
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
        PolicyEnacting newEnactment = new PolicyEnacting(signedPolicy);
        activeEnactingPolicies.Add(newEnactment);

        if (PolicyTreeManager.Instance != null)
        {
            PolicyTreeManager.Instance.RefreshTreeLayout();
        }
    }
    public void OnCalendarYearAdvanced()
    {
        if (activeEnactingPolicies.Count > 0) progressPolicy();

        RecalculateDerivedValues();
        OnMetricsUpdated?.Invoke(currentMetrics);
        updateEarth();
    }

    private void progressPolicy() {
        for (int i = activeEnactingPolicies.Count - 1; i >= 0; i--)
        {
            PolicyEnacting activePolicy = activeEnactingPolicies[i];

            //Environmental
            currentMetrics.airQuality += activePolicy.incrementalShiftsPerYear.airQuality;
            currentMetrics.oceanCleanliness += activePolicy.incrementalShiftsPerYear.oceanCleanliness;
            currentMetrics.forestDensity += activePolicy.incrementalShiftsPerYear.forestDensity;
            currentMetrics.biodiversityHealth += activePolicy.incrementalShiftsPerYear.biodiversityHealth;
            currentMetrics.magneticFieldStrength += activePolicy.incrementalShiftsPerYear.magneticFieldStrength;
            currentMetrics.climateVolatility += activePolicy.incrementalShiftsPerYear.climateVolatility;

            //Technological
            currentMetrics.IndustrialCapacity += activePolicy.incrementalShiftsPerYear.IndustrialCapacity;
            currentMetrics.researchCapacity += activePolicy.incrementalShiftsPerYear.researchCapacity;
            currentMetrics.economicGrowth += activePolicy.incrementalShiftsPerYear.economicGrowth;
            currentMetrics.energyAvailability += activePolicy.incrementalShiftsPerYear.energyAvailability;
            currentMetrics.resourceAvailability += activePolicy.incrementalShiftsPerYear.resourceAvailability;

            //social
            currentMetrics.publicApproval += activePolicy.incrementalShiftsPerYear.publicApproval;

            activePolicy.yearsRemaining--;

            // If the duration hits zero, the policy is officially completed!
            if (activePolicy.yearsRemaining <= 0)
            {
                activeEnactingPolicies.RemoveAt(i);
                Debug.Log($"<color=green><b>Policy Alert:</b> {activePolicy.policyData.policyName} has been 100% phased in!</color>");
            }
        }

        // Keep values structurally verified inside 0-1 bounds
        currentMetrics.ClampMetrics();
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

    public void updateEarth()
    {
        int year = CampaignTimeManager.Instance.currentYear;
        AtmosphereController.Instance.UpdateAtmosphere(currentMetrics);
        AuroraController.Instance.UpdateAuroraIntensity(currentMetrics);
        CloudController.Instance.UpdateCloud(currentMetrics);
        EarthShaderController.Instance.UpdateCityLights(currentMetrics,year);
        EarthShaderController.Instance.UpdateEarthSaturation(currentMetrics);
    }
}
