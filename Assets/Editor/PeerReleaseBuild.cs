using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class PeerReleaseBuild
{
    [MenuItem("Tools/Distribution/Build Signed Peer APK")]
    public static void Build()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            throw new InvalidOperationException("Switch the active build target to Android.");
        if (PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android) != ScriptingImplementation.IL2CPP
            || PlayerSettings.Android.targetArchitectures != AndroidArchitecture.ARM64)
            throw new InvalidOperationException("Peer release requires IL2CPP and ARM64 only.");
        if (!PlayerSettings.Android.useCustomKeystore ||
            !File.Exists(PlayerSettings.Android.keystoreName) ||
            string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ||
            string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
            string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
            throw new InvalidOperationException("Select the existing release keystore and enter both passwords in Unity Publishing Settings.");
        var form = Resources.Load<TextAsset>("PeerFeedbackUrl");
        if (form == null || !Uri.TryCreate(form.text.Trim(), UriKind.Absolute, out var url)
            || url.Scheme != Uri.UriSchemeHttps
            || !(url.Host == "forms.gle" || (url.Host == "docs.google.com" && url.AbsolutePath.StartsWith("/forms/", StringComparison.Ordinal))))
            throw new InvalidOperationException("Add the verified Google Forms responder link to Assets/Resources/PeerFeedbackUrl.txt.");

        string path = $"Builds/NoStarsRemain-{PlayerSettings.bundleVersion}-arm64.apk";
        if (File.Exists(path))
            throw new InvalidOperationException("Archive the existing APK before building a new binary under this version.");
        Directory.CreateDirectory("Builds");
        bool appBundle = EditorUserBuildSettings.buildAppBundle;
        BuildReport report;
        try
        {
            EditorUserBuildSettings.buildAppBundle = false;
            report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                locationPathName = path,
                target = BuildTarget.Android,
                options = BuildOptions.None
            });
        }
        finally { EditorUserBuildSettings.buildAppBundle = appBundle; }
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Peer APK build: {report.summary.result}");
        using var apk = File.OpenRead(path);
        using var sha = SHA256.Create();
        string hash = BitConverter.ToString(sha.ComputeHash(apk)).Replace("-", "").ToLowerInvariant();
        string guid = report.summary.guid.ToString();
        Debug.Log($"[Peer APK] {path} version={PlayerSettings.bundleVersion} code={PlayerSettings.Android.bundleVersionCode} guid={guid} sha256={hash}");
    }
}
