using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

// Main-thread, event-based local logging. Never call this once per frame.
public static class Telemetry
{
    static string session;
    static string filePath;
    static bool sessionStarted;

    public static string SessionId => session ??= Guid.NewGuid().ToString("N").Substring(0, 8);
    public static string FilePath => filePath ??= Path.Combine(Application.persistentDataPath, "telemetry.log");

    // Also reset between Editor sessions when Domain Reload is disabled.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetSession()
    {
        session = null;
        filePath = null;
        sessionStarted = false;
    }

    public static void StartSession()
    {
        if (sessionStarted) return;
        sessionStarted = true;
        Debug.Log($"[telemetry] file {FilePath}");
        Log("session_start", ("app_version", Application.version),
            ("device_model", SystemInfo.deviceModel), ("android", SystemInfo.operatingSystem));
    }

    public static void Log(string eventName, params (string k, object v)[] parameters)
    {
        var line = new StringBuilder(128);
        line.Append(Time.realtimeSinceStartup.ToString("F2", CultureInfo.InvariantCulture))
            .Append(' ').Append(SessionId).Append(' ').Append(eventName);
        foreach (var (key, value) in parameters)
        {
            // Device/OS metadata can contain spaces. Encode values as single tokens.
            string formatted = value is IFormattable number
                ? number.ToString(null, CultureInfo.InvariantCulture)
                : value?.ToString() ?? "unknown";
            line.Append(' ').Append(key).Append('=').Append(Uri.EscapeDataString(formatted));
        }

        string text = line.ToString();
        Debug.Log("[telemetry] " + text);
        try
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            File.AppendAllText(FilePath, text + "\n", Encoding.UTF8);
        }
        catch (IOException exception)
        {
            Debug.LogWarning($"[telemetry] Local file write failed: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            Debug.LogWarning($"[telemetry] Local file access denied: {exception.Message}");
        }
    }
}
