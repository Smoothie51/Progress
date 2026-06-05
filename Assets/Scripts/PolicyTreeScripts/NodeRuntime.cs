using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PolicyRuntimeNode : MonoBehaviour
{
    public PolicyNodeData data;
    
    public enum NodeState { Locked, Available, Unlocked }
    public NodeState currentState = NodeState.Locked;

    // References to the UI buttons connecting this node to its neighbors
    public List<PolicyRuntimeNode> childNodes = new List<PolicyRuntimeNode>();

    [Header("Cached UI References")]
    [SerializeField] private GameObject ringObject;
    [SerializeField] private GameObject iconObject;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button nodeButton;

    void Start()
    {
        if (ringObject == null) ringObject = transform.Find("Ring").gameObject;
        if (iconObject == null) iconObject = transform.Find("Icon").gameObject;
        if (nodeButton == null) nodeButton = iconObject.GetComponent<Button>();
        if (iconImage == null) iconImage = iconObject.GetComponent<Image>();

        if (data.icon) iconImage.sprite = data.icon;
    }

    public void UpdateNode(HashSet<PolicyNodeData> unlockedPolicies, int currentYear)
    {
        currentState = CheckNodeState(unlockedPolicies, currentYear);
        UpdateUI(currentState);
    }

    public NodeState CheckNodeState(HashSet<PolicyNodeData> unlockedPolicies, int currentYear)
    {
        if (unlockedPolicies.Contains(data)) return NodeState.Unlocked;
        if (currentYear < data.minimumYearRequired) return NodeState.Locked;
        if (data.isRootNode) return NodeState.Available;
        
        foreach (var prereq in data.prerequisites) // Check if all prerequisites are unlocked 
        {
            if (!unlockedPolicies.Contains(prereq))return NodeState.Locked;
        }
        return NodeState.Available;
    }

    public void UpdateUI(NodeState state)
    {
        switch (state)
        {
            case NodeState.Locked:  // Greyed + no ring + non-interactable
                ringObject.SetActive(false);
                nodeButton.interactable = false;
                iconImage.color = Color.gray;
                break;
            case NodeState.Available: // no ring + interactable
                ringObject.SetActive(false);
                nodeButton.interactable = true;
                iconImage.color = Color.white;
                break;
            case NodeState.Unlocked: // ring + non-interactable
                ringObject.SetActive(true);
                nodeButton.interactable = false;
                iconImage.color = Color.white;
                break;
        }
    }

    public void SignPolicy()
    {
        if (currentState != NodeState.Available)
        {
            Debug.LogWarning("Policy not available for signature!");
            return;
        }

        PolicyTreeManager.Instance.UnlockPolicy(data);
        
        Debug.Log($"Law signed successfully: {data.policyName}");
        EdgeGenerator.Instance.UpdateLineColorsForNode(this, Color.white, Color.green);

    }


}