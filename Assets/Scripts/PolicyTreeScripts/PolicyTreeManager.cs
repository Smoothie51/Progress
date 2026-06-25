using UnityEngine;
using System.Collections.Generic;

public class PolicyTreeManager : MonoBehaviour
{
    public static PolicyTreeManager Instance { get; private set; }

    [Header("Tracking Repositories")]
    public HashSet<PolicyNodeData> unlockedPolicies = new HashSet<PolicyNodeData>();
    private PolicyRuntimeNode[] allPolicyNodes;


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

    public void UnlockPolicy(PolicyNodeData policy)
    {
        if (!unlockedPolicies.Contains(policy))
        {
            unlockedPolicies.Add(policy);
            RefreshTreeLayout();
            EarthStateController.Instance.ProcessPolicySignature(policy);
        }
    }
}