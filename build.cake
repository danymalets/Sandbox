#addin nuget:?package=Cake.Unity&version=0.9.0

var target = Argument("target", "Build-Windows");

void Build(BuildTarget buildTarget, string methodName, string logFile){

    UnityEditor("C:/Program Files/Unity/Hub/Editor/6000.0.32f1/Editor/Unity.exe",
        new UnityEditorArguments{
            ProjectPath = "./Sandbox",
            BatchMode = true,
            Quit = true,
            ExecuteMethod = methodName,
            BuildTarget = buildTarget,
            LogFile = logFile,
        },
        new UnityEditorSettings
        {
            RealTimeLog = true,
        }
    );
}

Task("Build-Windows")
    .Does(() =>
{
    Build(BuildTarget.Win64, "_Project.Sources.Builds.BuildWindows", "./Logs/BuildWindowsLog.log");
    
});

Task("Build-Android")
    .Does(() =>
{
    Build(BuildTarget.Android, "_Project.Sources.Builds.BuildAndroid", "./Logs/BuildAndroidLog.log");
    
});

RunTarget(target);