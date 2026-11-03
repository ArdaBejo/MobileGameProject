using UnityEngine;

public static class MotionSetting
{
    const string Key = "reduceMotion";

    public static bool ReduceMotion
    {
        get => PlayerPrefs.GetInt(Key, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(Key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}