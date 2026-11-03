using UnityEngine;

public class MenuReady : MonoBehaviour
{
    void Update()
    {
        // Menu Start alone is too early: the persistent overlay may still block input.
        if (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading) return;
        Debug.Log($"[perf] interactive {Time.realtimeSinceStartup:F2}s");
        enabled = false;
    }
}
