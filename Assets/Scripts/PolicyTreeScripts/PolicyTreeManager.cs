using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PolicyTreeManager : MonoBehaviour
{
    public static PolicyTreeManager Instance { get; private set; }

    [Header("Tracking Repositories")]
    public HashSet<PolicyNodeData> unlockedPolicies = new HashSet<PolicyNodeData>();
    private PolicyRuntimeNode[] allPolicyNodes;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI policyPointsText;

    [SerializeField] private int policyPoints;
    private float pointBuffer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Gather all visual/runtime nodes mapped across your UI canvas
        allPolicyNodes = FindObjectsByType<PolicyRuntimeNode>();

        EdgeGenerator.Instance.GenerateAllConnections();

        RefreshTreeLayout();
        UpdatePolicyPointsText();
    }

    public void RefreshTreeLayout()
    {
        // Grab current campaign parameters from your master calendar manager
        int currentYear = CampaignTimeManager.Instance.currentYear;

        foreach (var node in allPolicyNodes)
        {
            node.UpdateNode(unlockedPolicies, currentYear);
        }
    }

    private void UpdatePolicyPointsText()
    {
        if (policyPointsText == null) return;
        policyPointsText.text = policyPoints.ToString();
    }

    public bool UnlockPolicy(PolicyNodeData policy)
    {
        if (policy == null) return false;
        if (unlockedPolicies.Contains(policy)) return false;
        if (policyPoints < policy.policyPointsNeeded) return false;

        policyPoints -= policy.policyPointsNeeded;
        UpdatePolicyPointsText();

        unlockedPolicies.Add(policy);
        RefreshTreeLayout();
        EarthStateController.Instance.ProcessPolicySignature(policy);
        return true;
    }

    public void AddPoints(int pointsToAdd)
    {
        if (pointsToAdd <= 0) return;

        policyPoints += pointsToAdd;
        UpdatePolicyPointsText();
    }
    public void GeneratePoints()
    {
        float baseGeneration = EarthStateController.Instance.currentMetrics.economicGrowth * 10f; 
        float researchMultiplier = 1f + (EarthStateController.Instance.currentMetrics.researchCapacity * 1.5f); // Up to a 2.5x multiplier
        
        float pointsGeneratedThisYear = baseGeneration * researchMultiplier;

        pointBuffer += pointsGeneratedThisYear;
        
        if (pointBuffer >= 1f)
        {
            int wholePointsToAdd = Mathf.FloorToInt(pointBuffer);
            AddPoints(wholePointsToAdd);
            pointBuffer -= wholePointsToAdd; 
        }
    }
}
