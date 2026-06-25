using UnityEngine;

[System.Serializable]
public class PolicyEnacting
{
    public PolicyNodeData policyData;
    public int yearsRemaining;
    public EarthMetrics incrementalShiftsPerYear;

    public PolicyEnacting(PolicyNodeData data)
    {
        policyData = data;
        yearsRemaining = data.executionDurationYears;

        // Divide the total target shift matrix by the duration to get the exact yearly fractional chunk
        float duration = Mathf.Max(1, data.executionDurationYears);
        
        incrementalShiftsPerYear.airQuality = data.metricShifts.airQuality / duration;
        incrementalShiftsPerYear.oceanCleanliness = data.metricShifts.oceanCleanliness / duration;
        incrementalShiftsPerYear.forestDensity = data.metricShifts.forestDensity / duration;
        incrementalShiftsPerYear.biodiversityHealth = data.metricShifts.biodiversityHealth / duration;
        incrementalShiftsPerYear.magneticFieldStrength = data.metricShifts.magneticFieldStrength / duration;
        incrementalShiftsPerYear.climateVolatility = data.metricShifts.climateVolatility / duration;
        incrementalShiftsPerYear.IndustrialCapacity = data.metricShifts.IndustrialCapacity / duration;
        incrementalShiftsPerYear.researchCapacity = data.metricShifts.researchCapacity / duration;
        incrementalShiftsPerYear.economicGrowth = data.metricShifts.economicGrowth / duration;
        incrementalShiftsPerYear.energyAvailability = data.metricShifts.energyAvailability / duration;
        incrementalShiftsPerYear.resourceAvailability = data.metricShifts.resourceAvailability / duration;
    }
}