using UnityEngine;

[System.Serializable]
public struct EarthMetrics
{
    [Header("Environmental Sub-Metrics (0.0 to 1.0)")]
    [Range(0f, 1f)] public float airQuality;
    [Range(0f, 1f)] public float oceanCleanliness;
    [Range(0f, 1f)] public float forestDensity;
    [Range(0f, 1f)] public float biodiversityHealth;
    [Range(0f, 1f)] public float magneticFieldStrength;
    [Range(0f, 1f)] public float climateVolatility;

    [Header("Technological & Industrial Sub-Metrics (0.0 to 1.0)")]
    [Range(0f, 1f)] public float IndustrialCapacity;
    [Range(0f, 1f)] public float researchCapacity;
    [Range(0f, 1f)] public float economicGrowth; 
    [Range(0f, 1f)] public float energyAvailability;
    [Range(0f, 1f)] public float resourceAvailability;

    [Header("Socio-Economic Constraints (0.0 to 1.0)")]
    [Range(0f, 1f)] public float publicApproval;
    

    // Helper method to keep values safely clamped within simulation bounds
    public void ClampMetrics()
    {
        airQuality = Mathf.Clamp01(airQuality);
        oceanCleanliness = Mathf.Clamp01(oceanCleanliness);
        forestDensity = Mathf.Clamp01(forestDensity);
        biodiversityHealth = Mathf.Clamp01(biodiversityHealth);
        climateVolatility = Mathf.Clamp01(climateVolatility);
        magneticFieldStrength = Mathf.Clamp01(magneticFieldStrength);

        IndustrialCapacity = Mathf.Clamp01(IndustrialCapacity);
        researchCapacity = Mathf.Clamp01(researchCapacity);
        economicGrowth = Mathf.Clamp01(economicGrowth);
        energyAvailability = Mathf.Clamp01(energyAvailability);
        resourceAvailability = Mathf.Clamp01(resourceAvailability);

        publicApproval = Mathf.Clamp01(publicApproval);
    }
}