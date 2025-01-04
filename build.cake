#addin nuget:?package=Cake.Unity&version=0.9.0

var target = Argument("target", "Build-Windows");

Task("Clean-Artifacts")
    .Does(() =>
{
    CleanDirectory($"./Builds/Windows");
});

Task("Build-Windows")
    .IsDependentOn("Clean-Artifacts")
    .Does(() =>
{
    
    Console.WriteLine("Hello World!");
    UnityEditor(2022, 3, 
        new UnityEditorArguments{
            ProjectPath = ".",
            BatchMode = true,
            Quit = true,
            ExecuteMethod = "Builds.WindowsBuild",
            BuildTarget = BuildTarget.Android,
            LogFile = "./Logs/BuildLog.log",
        },
        new UnityEditorSettings
        {
            RealTimeLog = true,
        }
    );
    
});

// todo: build directory as argument

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);