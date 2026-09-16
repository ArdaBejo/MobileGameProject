using UnityEngine;
using UnityEngine.UI;

public class HapticsSettings : MonoBehaviour
{
    [SerializeField] private Toggle hapticsToggle;

    void Start()
    {
        hapticsToggle.isOn = Haptics.Enabled;
        hapticsToggle.onValueChanged.AddListener(OnHapticsChanged);
    }

    void OnDestroy()
    {
        hapticsToggle.onValueChanged.RemoveListener(OnHapticsChanged);
    }

    void OnHapticsChanged(bool value)
    {
        Haptics.Enabled = value;
    }
}