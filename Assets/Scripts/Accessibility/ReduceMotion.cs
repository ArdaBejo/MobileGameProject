using UnityEngine;
using UnityEngine.UI;

public class ReduceMotion : MonoBehaviour
{
    private const string Key = "reduceMotion";
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.isOn = PlayerPrefs.GetInt(Key, 0) == 1;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool enabled)
    {
        PlayerPrefs.SetInt(Key, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }
}
