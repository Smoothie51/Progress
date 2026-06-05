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
    public PolicyNodeData[] prerequisites;
    public bool isRootNode = false;
    
    [Header("Unlock Constraints")]  
    public int minimumYearRequired = 1760;
}