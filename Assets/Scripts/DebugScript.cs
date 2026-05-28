using UnityEngine;

public class SmogDebugger : MonoBehaviour
{
    [Header("Target Controller")]
    [SerializeField] private SmogController smogController;

    [Header("Test Slider")]
    [Range(0f, 1f)] public float testPollutionLevel = 0f;

    private float lastValue;

    void Update()
    {
        // Only trigger the visual update if the slider value actually changes in the Inspector
        if (!Mathf.Approximately(testPollutionLevel, lastValue))
        {
            lastValue = testPollutionLevel;
            if (smogController != null)
            {
                smogController.SetSmogLevel(testPollutionLevel);
            }
        }
    }
}