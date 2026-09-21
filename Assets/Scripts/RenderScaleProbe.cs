using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Halve the render scale: big drop = GPU-bound, no change = CPU-bound
public class RenderScaleProbe : MonoBehaviour
{
    [Range(0.3f, 1f)] public float lowScale = 0.5f;

    UniversalRenderPipelineAsset _urp;
    float _full;

    void Start()
    {
        _urp = GraphicsSettings.currentRenderPipeline
               as UniversalRenderPipelineAsset;
        _full = _urp.renderScale;
    }

    public void Toggle()
    {
        bool atFull = Mathf.Approximately(_urp.renderScale, _full);
        _urp.renderScale = atFull ? lowScale : _full;

        Debug.Log($"[Probe] renderScale = {_urp.renderScale}");
    }
}