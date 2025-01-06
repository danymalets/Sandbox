#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace _Project.Sources
{
    public static class Builds
    {
        [MenuItem("Custom/Builds/Build Windows", priority = 0)]
        public static void BuildWindows() => Build(BuildTarget.StandaloneWindows, "Windows", "Sandbox.exe");
        
        [MenuItem("Custom/Builds/Build Android", priority = 0)]
        public static void BuildAndroid() => Build(BuildTarget.Android, "Android", "Sandbox.apk");
        
        private static void Build(BuildTarget buildTarget, string relativePath, string fileName)
        {
            const string startPath = "Builds/Auto";

            var buildDirectory = $"{startPath}/{relativePath}";
            
            if (Directory.Exists(buildDirectory))
            {
                Directory.Delete(buildDirectory, true);
            }
            
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/GameScene.unity" },
                locationPathName = $"{buildDirectory}/{fileName}",
                target = buildTarget,
                options = BuildOptions.None,
            };
            
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build Success.");
            }
            else
            {
                throw new Exception($"Build Failed, status:{report.summary.result}.");
            }
        }
    }

}

#endif
