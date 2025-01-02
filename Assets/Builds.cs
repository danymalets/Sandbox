#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class Builds
{
    [MenuItem("Custom/Builds/Windows Build")]
    public static void WindowsBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/SampleScene.unity" };
        buildPlayerOptions.locationPathName = $"Builds/Windows/Sandbox/{System.DateTime.Now.ToString($"yyyyMMddHHmmss")}.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
       
        Debug.Log($"Done, result {report.summary.result}");
    }
}

#endif