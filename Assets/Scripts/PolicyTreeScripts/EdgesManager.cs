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

        // 1. Get all PolicyNodes sitting inside the Nodes container
        PolicyRuntimeNode[] nodes = nodesContainer.GetComponentsInChildren<PolicyRuntimeNode>();



        foreach (PolicyRuntimeNode node in nodes)
        {
            if (node == null || node.childNodes == null) continue;

            foreach (PolicyRuntimeNode childNode in node.childNodes)
            {
                if (childNode == null) continue;

                GameObject lineObj = Instantiate(linePrefab, transform);
                lineObj.name = $"{node.gameObject.name} -> {childNode.gameObject.name} Line";

                Image lineImg = lineObj.GetComponent<Image>();
                RectTransform lineRect = lineObj.GetComponent<RectTransform>();

                if (lineImg != null && lineRect != null)
                {
                    RectTransform parentRect = node.GetComponent<RectTransform>();
                    RectTransform childRect = childNode.GetComponent<RectTransform>();

                    // 2. Set pivot to middle-left so it stretches and rotates from its starting edge
                    lineRect.pivot = new Vector2(0f, 0.5f);

                    // 3. Snap the start of the line exactly to the parent node's UI center coordinates
                    lineRect.anchoredPosition = parentRect.anchoredPosition; 

                    // 4. Calculate the directional vector using pure UI coordinates
                    Vector2 uiDirection = childRect.anchoredPosition - parentRect.anchoredPosition;

                    // 5. Calculate UI distance and set line length
                    float distance = uiDirection.magnitude;
                    lineRect.sizeDelta = new Vector2(distance, 2f);

                    // 6. Rotate line to point directly from parent node to child node
                    float angle = Mathf.Atan2(uiDirection.y, uiDirection.x) * Mathf.Rad2Deg;
                    lineRect.localRotation = Quaternion.Euler(0, 0, angle);

                    lineObj.SetActive(true);
                    lineObj.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f); // Default locked color

                    // Store reference to update states visually later
                    nodeLineMap.Add((lineImg, new PolicyRuntimeNode[] { node, childNode }));
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
    // public void GenerateConnectionLines()
    // {
    //     if (linePrefab == null) return;

    //     foreach (var child in childNodes)
    //     {
    //         if (child == null) continue;

    //         GameObject lineObj = Instantiate(linePrefab, transform.Find("Edges"));
    //         lineObj.name = $"{gameObject.name} - {child.gameObject.name} Line";

    //         Image lineImg = lineObj.GetComponent<Image>();
    //         RectTransform lineRect = lineObj.GetComponent<RectTransform>();

    //         if (lineImg != null && lineRect != null)
    //         {
    //             // Set pivot to middle-left so it scales and rotates from the start node smoothly
    //             lineRect.pivot = new Vector2(0f, 0.5f);
    //             lineRect.anchoredPosition = Vector2.zero; // Start at this node's center

    //             // Calculate local direction and distance to child node
    //             Vector3 localChildPos = transform.InverseTransformPoint(child.transform.position);
    //             float distance = localChildPos.magnitude;

    //             // Set the line thickness and length
    //             lineRect.sizeDelta = new Vector2(distance, 2f);

    //             // Rotate line to point directly at the child node
    //             float angle = Mathf.Atan2(localChildPos.y, localChildPos.x) * Mathf.Rad2Deg;
    //             lineRect.localRotation = Quaternion.Euler(0, 0, angle);

    //             lineObj.SetActive(true);

    //             activeLines.Add((lineImg, child));
    //         }
    //     }
    // }

    // public void UpdateLineVisuals()
    // {
    //     foreach (var (line, child) in activeLines)
    //     {
    //         if (line == null) continue;

    //         switch (child.currentState)
    //         {
    //             case NodeState.Unlocked:
    //                 line.color = Color.cyan;
    //                 break;
    //             case NodeState.Available:
    //                 line.color = Color.white; 
    //                 break;
    //             case NodeState.Locked:
    //                 line.color = new Color(0.2f, 0.2f, 0.2f);
    //                 break;
    //         }
    //     }
    // }
}

