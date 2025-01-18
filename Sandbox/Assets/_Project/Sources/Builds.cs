#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace _Project.Sources
{
    public static class Builds
    {
        private static readonly string[] GameScenes = new string[]
        {
            "Assets/_Project/Scenes/GameScene.unity",
        };

        private static readonly string[] ClientScenes = new[]
        {
            "Assets/_Project/Scenes/EntryScene.unity",
        }.Concat(GameScenes).ToArray();
        
        private static readonly string[] ServerScenes = new[]
        {
            "Assets/_Project/Scenes/ServerEntryScene.unity",
        }.Concat(GameScenes).ToArray();
        
        private const string BuildsPath = "Builds/Auto/";

        [MenuItem("Custom/Builds/Build Windows", priority = 1001)]
        public static void BuildWindows() => Build(BuildTarget.StandaloneWindows, ClientScenes,"Windows", "Sandbox.exe");
        
        [MenuItem("Custom/Builds/Build Android", priority = 1002)]
        public static void BuildAndroid() => Build(BuildTarget.Android, ClientScenes, "Android", "Sandbox.apk");

        [MenuItem("Custom/Builds/Build Linux Server", priority = 2001)]
        public static void BuildLinuxServer() => Build(BuildTarget.StandaloneLinux64, ServerScenes,"LinuxServer", "SandboxServer.x86_64");

        [MenuItem("Custom/Builds/Build Windows Server", priority = 2002)]
        public static void BuildWindowsServer() => Build(BuildTarget.StandaloneWindows, ServerScenes,"WindowsServer", "SandboxServer.exe");
        
        [MenuItem("Custom/Builds/Open Builds Folder", priority = 3001)]
        public static void OpenBuildsFolder() => EditorUtility.RevealInFinder(BuildsPath);

        private static void Build(BuildTarget buildTarget, string[] scenes, string relativePath, string fileName)
        {

            var buildDirectory = BuildsPath + relativePath;
            
            if (Directory.Exists(buildDirectory))
            {
                Directory.Delete(buildDirectory, true);
            }
            
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
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
