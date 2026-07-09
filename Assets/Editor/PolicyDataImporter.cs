#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class PolicyDatabaseImporter : EditorWindow
{
    [MenuItem("Tools/Import Policy Database")]
    public static void ImportPolicies()
    {
        string path = EditorUtility.OpenFilePanel("Select Policy Data (txt or csv)", Application.dataPath, "");
        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path);
        
        // Define where the assets will be saved
        string saveDirectory = "Assets/PoliciesScripts";

        Dictionary<string, PolicyNodeData> generatedPolicies = new Dictionary<string, PolicyNodeData>();
        Dictionary<string, string> prereqMap = new Dictionary<string, string>();
        Dictionary<string, string> mutuallyExclusiveMap = new Dictionary<string, string>();

        // Pass 1: Create all the ScriptableObjects
        for (int i = 1; i < lines.Length; i++) // Skip the header row
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] data = lines[i].Split('|');
            if (data.Length < 25) continue;

            string id = data[0].Trim();
            PolicyNodeData policy = ScriptableObject.CreateInstance<PolicyNodeData>();

            policy.id = id;
            policy.policyName = data[1].Trim();
            policy.description = data[2].Trim();
            policy.isRootNode = data[3].Trim().ToUpper() == "TRUE";
            policy.minimumYearRequired = int.Parse(data[4]);
            policy.policyPointsNeeded = int.Parse(data[5]);
            policy.executionDurationYears = int.Parse(data[6]);
            
            // Temporarily store the array links for Pass 2
            prereqMap[id] = data[7].Trim();
            mutuallyExclusiveMap[id] = data[8].Trim();

            policy.positiveModifier = data[9].Trim();
            policy.negativeModifier = data[10].Trim();

            // Assign the 14 float metric shifts safely
            policy.metricShifts = new EarthMetricsChangeRange
            {
                airQuality = ParseFloat(data[11]),
                oceanCleanliness = ParseFloat(data[12]),
                forestDensity = ParseFloat(data[13]),
                biodiversityHealth = ParseFloat(data[14]),
                magneticFieldStrength = ParseFloat(data[15]),
                climateVolatility = ParseFloat(data[18]),
                IndustrialCapacity = ParseFloat(data[19]),
                researchCapacity = ParseFloat(data[20]),
                economicGrowth = ParseFloat(data[21]),
                energyAvailability = ParseFloat(data[22]),
                resourceAvailability = ParseFloat(data[23]),
                publicApproval = ParseFloat(data[24])
            };

            string assetPath = $"{saveDirectory}/{id}_{policy.policyName.Replace(" ", "")}.asset";
            AssetDatabase.CreateAsset(policy, assetPath);
            generatedPolicies.Add(id, policy);
        }

        // Pass 2: Wire up the Prerequisites and Mutually Exclusive arrays automatically!
        foreach (var kvp in generatedPolicies)
        {
            PolicyNodeData currentPolicy = kvp.Value;
            string id = kvp.Key;

            // Wire Prerequisites
            if (!string.IsNullOrEmpty(prereqMap[id]))
            {
                string[] pReqs = prereqMap[id].Split(',');
                currentPolicy.prerequisites = pReqs.Where(req => generatedPolicies.ContainsKey(req))
                                                   .Select(req => generatedPolicies[req]).ToArray();
            }

            // Wire Mutually Exclusive
            if (!string.IsNullOrEmpty(mutuallyExclusiveMap[id]))
            {
                string[] mExcl = mutuallyExclusiveMap[id].Split(',');
                currentPolicy.mutuallyExclusiveWith = mExcl.Where(req => generatedPolicies.ContainsKey(req))
                                                           .Select(req => generatedPolicies[req]).ToArray();
            }

            EditorUtility.SetDirty(currentPolicy);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"<color=green>Successfully generated and linked {generatedPolicies.Count} Policy Nodes!</color>");
    }

    private static float ParseFloat(string value)
    {
        if (float.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float result))
            return result;
        return 0f;
    }
}
#endif