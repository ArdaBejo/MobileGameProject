using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class ThermalSoakBuild
{
    [MenuItem("Tools/Performance/Build Android Thermal Soak APK")]
    public static void Build()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            throw new InvalidOperationException("Switch the active build target to Android first.");

        const string path = "Builds/Week7-ThermalSoak-dev.apk";
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        bool customKeystore = PlayerSettings.Android.useCustomKeystore;
        string package = PlayerSettings.applicationIdentifier;
        bool appBundle = EditorUserBuildSettings.buildAppBundle;
        BuildReport report;
        try
        {
            // Keep diagnostic signing and saved data separate from the installed release.
            PlayerSettings.Android.useCustomKeystore = false;
            PlayerSettings.applicationIdentifier = package + ".soak";
            EditorUserBuildSettings.buildAppBundle = false;
            report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                locationPathName = path,
                target = BuildTarget.Android,
                options = BuildOptions.Development | BuildOptions.ConnectWithProfiler
            });
        }
        finally
        {
            PlayerSettings.Android.useCustomKeystore = customKeystore;
            PlayerSettings.applicationIdentifier = package;
            EditorUserBuildSettings.buildAppBundle = appBundle;
        }
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Thermal soak build: {report.summary.result}");
        Debug.Log($"[Soak Build] {path} ({report.summary.totalSize} bytes)");
    }
}
