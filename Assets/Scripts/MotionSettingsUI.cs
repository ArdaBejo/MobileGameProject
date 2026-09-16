using UnityEngine;
using UnityEngine.UI;

public class MotionSettingsUI : MonoBehaviour
{
    [SerializeField] Toggle reduceMotionToggle;

    void Start()
    {
        reduceMotionToggle.isOn = MotionSetting.ReduceMotion;
        reduceMotionToggle.onValueChanged.AddListener(OnChanged);
    }

    void OnChanged(bool value)
    {
        MotionSetting.ReduceMotion = value;
    }
}