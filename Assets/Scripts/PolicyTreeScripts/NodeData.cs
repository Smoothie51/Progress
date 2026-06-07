using UnityEngine;

// This attribute allows you to populate your tree layout directly in the project view!
[CreateAssetMenu(fileName = "NewPolicyNode", menuName = "Simulation/Policy Node Data")]
public class PolicyNodeData : ScriptableObject
{

    [Header("UI Display Details")]
    public string id;
    public string policyName;
    [TextArea(3, 5)] public string description;
    public Sprite icon;

    [Header("Tree Topology")]
    
    [Tooltip("If any policy in this list is unlocked, this node becomes permanently disabled.")]
    public PolicyNodeData[] mutuallyExclusiveWith;
    
    [Tooltip("All policies in this list must be unlocked before this node becomes available.")]
    public PolicyNodeData[] prerequisites;
    public bool isRootNode = false;
    
    [Header("Unlock Constraints")]  
    public int minimumYearRequired = 1760;
}