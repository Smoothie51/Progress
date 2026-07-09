using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EdgeGenerator : MonoBehaviour
{
    public static EdgeGenerator Instance { get; private set; }
    [Header("References")]
    [Tooltip("Drag the 'Nodes' parent GameObject here")]
    public Transform nodesContainer;
    
    [Tooltip("The UI Image line prefab")]
    public GameObject linePrefab;

    [Header("Line Settings")]
    public float lineWidth = 2f;
    private List<(Image edge, PolicyRuntimeNode[] nodeConnected)> nodeLineMap = new List<(Image, PolicyRuntimeNode[])>();
    private Dictionary<PolicyNodeData, PolicyRuntimeNode> dataToNodeMap = new Dictionary<PolicyNodeData, PolicyRuntimeNode>();

    [ContextMenu("Generate Tree Lines")] 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void GenerateAllConnections()
    {
        ClearExistingLines();

        if (nodesContainer == null || linePrefab == null)
        {
            Debug.LogWarning("EdgeGenerator: Missing nodesContainer or linePrefab assignment!");
            return;
        }

        // 1. Get all PolicyNodes and build a map from PolicyNodeData to PolicyRuntimeNode
        PolicyRuntimeNode[] nodes = nodesContainer.GetComponentsInChildren<PolicyRuntimeNode>();
        dataToNodeMap.Clear();
        
        foreach (PolicyRuntimeNode node in nodes)
        {
            if (node != null && node.data != null)
            {
                dataToNodeMap[node.data] = node;
            }
        }

        // 2. Iterate through all nodes and use their prerequisites to draw dependency lines
        foreach (PolicyRuntimeNode node in nodes)
        {
            if (node == null || node.data == null || node.data.prerequisites == null) continue;

            foreach (PolicyNodeData prereqData in node.data.prerequisites)
            {
                if (prereqData == null || !dataToNodeMap.TryGetValue(prereqData, out PolicyRuntimeNode prereqNode)) continue;

                GameObject lineObj = Instantiate(linePrefab, transform);
                lineObj.name = $"{prereqNode.gameObject.name} -> {node.gameObject.name} Line";

                Image lineImg = lineObj.GetComponent<Image>();
                RectTransform lineRect = lineObj.GetComponent<RectTransform>();

                if (lineImg != null && lineRect != null)
                {
                    RectTransform parentRect = prereqNode.GetComponent<RectTransform>();
                    RectTransform childRect = node.GetComponent<RectTransform>();

                    // 3. Set pivot to middle-left so it stretches and rotates from its starting edge
                    lineRect.pivot = new Vector2(0f, 0.5f);

                    // 4. Snap the start of the line exactly to the prerequisite node's UI center coordinates
                    lineRect.anchoredPosition = parentRect.anchoredPosition; 

                    // 5. Calculate the directional vector using pure UI coordinates
                    Vector2 uiDirection = childRect.anchoredPosition - parentRect.anchoredPosition;

                    // 6. Calculate UI distance and set line length
                    float distance = uiDirection.magnitude;
                    lineRect.sizeDelta = new Vector2(distance, 2f);

                    // 7. Rotate line to point directly from prerequisite node to dependent node
                    float angle = Mathf.Atan2(uiDirection.y, uiDirection.x) * Mathf.Rad2Deg;
                    lineRect.localRotation = Quaternion.Euler(0, 0, angle);

                    lineObj.SetActive(true);
                    lineObj.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f); // Default locked color

                    // Store reference to update states visually later
                    nodeLineMap.Add((lineImg, new PolicyRuntimeNode[] { prereqNode, node }));
                }
            }
        }
    }

    private void ClearExistingLines()
    {
        // Destroy all old children under Edges container
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        nodeLineMap.Clear();
    }

    // Call this whenever a node changes state (Unlocked, Available, etc.)
    public void UpdateLineColorsForNode(PolicyRuntimeNode node, Color toChild, Color toParent)
    {
        foreach (var (edge, connectedNodes) in nodeLineMap)
        {
            if(connectedNodes[1] == node) // if node is child
            {
                edge.color = toParent;
            }
            else if (connectedNodes[0] == node) // if node is parent
            {
                edge.color = toChild;
            }
        }   
    }
}

