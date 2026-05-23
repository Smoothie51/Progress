using UnityEngine;

public class SynchronizedRotation : MonoBehaviour
{
    public enum OrbitType { SunYear, MoonMonth }
    public OrbitType type;

    void Update()
    {
        if (CampaignTimeManager.Instance == null) return;

        float speed = 0f;

        if (type == OrbitType.SunYear)
        {
            speed = CampaignTimeManager.Instance.currentSunOrbitSpeed;
        }
        else if (type == OrbitType.MoonMonth)
        {
            speed = CampaignTimeManager.Instance.currentMoonOrbitSpeed;
        }

        // Time.deltaTime automatically handles your x1 to x4 time scales!
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}