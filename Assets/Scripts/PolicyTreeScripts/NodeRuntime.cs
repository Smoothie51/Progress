using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PolicyRuntimeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PolicyNodeData data;
    
    public enum NodeState { Locked, Available, Unlocked }
    public NodeState currentState = NodeState.Locked;



    [Header("Cached UI References")]
    [SerializeField] private GameObject ringObject;
    [SerializeField] private GameObject iconObject;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button nodeButton;

    private bool permanentlyDisabled = false;

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
        if (permanentlyDisabled) return NodeState.Locked;
        foreach (var rival in data.mutuallyExclusiveWith)
        {
            if (rival != null && unlockedPolicies.Contains(rival)) {
                permanentlyDisabled = true;
                return NodeState.Locked;
            }
        }
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
                if (permanentlyDisabled) 
                    iconImage.color = new Color(0.4f, 0.1f, 0.1f, 0.5f);
                else
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

        if (PolicyTreeManager.Instance.UnlockPolicy(data))
        {
            Debug.Log($"Law signed successfully: {data.policyName}");
            EdgeGenerator.Instance.UpdateLineColorsForNode(this, Color.white, Color.green);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Hovering over {data.policyName} node. Current state: {currentState}");
        if (data == null || ToolTip.Instance == null) return;

        string displayDescription = data.description;
        if (data.mutuallyExclusiveWith != null && data.mutuallyExclusiveWith.Length > 0)
        {
            List<string> mutuallyExclusiveNames = new List<string>();
            foreach (var mutuallyExclusivePolicy in data.mutuallyExclusiveWith)
            {
                if (mutuallyExclusivePolicy != null && !string.IsNullOrEmpty(mutuallyExclusivePolicy.policyName))
                {
                    mutuallyExclusiveNames.Add(mutuallyExclusivePolicy.policyName);
                }
            }

            if (mutuallyExclusiveNames.Count > 0)
            {
                displayDescription = $"Mutually exclusive with: {string.Join(", ", mutuallyExclusiveNames )}\n{displayDescription}";
            }
        }

        string positiveModifier = data.positiveModifier;
        string negativeModifier = data.negativeModifier;
        if (currentState == NodeState.Locked)
        {
            displayDescription = "<color=red><i>Unavailable</i></color>";
            data.positiveModifier = "";
            data.negativeModifier = "";
        }

        // Pass the ScriptableObject string packages straight to the single UI display instance
        ToolTip.Instance.ShowTooltip(data.policyName, data.icon, displayDescription, positiveModifier, negativeModifier);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Stopped hovering over {data.policyName} node.");
        if (ToolTip.Instance != null)
            ToolTip.Instance.HideTooltip();
    }
}