var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

var isLocalBuild = BuildSystem.IsLocalBuild;
var isMainBranch = StringComparer.OrdinalIgnoreCase.Equals("main", BuildSystem.AppVeyor.Environment.Repository.Branch);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./Ffsti.Library/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./Ffsti.Library.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Publish")
    .WithCriteria(c=>HasArgument("publish"))
    .IsDependentOn("Build")
    .Does(() => {

   var rootAbsoluteDir = MakeAbsolute(Directory("./"));

    var dotnetPackSettings = new DotNetPackSettings{
        OutputDirectory = rootAbsoluteDir + @"\artifacts\",
        Configuration = configuration
    };

    var dotNetNuGetPushSettings = new DotNetNuGetPushSettings {
        ApiKey =EnvironmentVariable<string>("NUGET_API_KEY", "42"),
        Source="https://api.nuget.org/v3/index.json"
    };

    DotNetPack("./Ffsti.Library/Ffsti.Library.csproj", dotnetPackSettings);
    DotNetPack("./Ffsti.Library.Database/Ffsti.Library.Database.csproj", dotnetPackSettings);
    DotNetNuGetPush(rootAbsoluteDir + @"\artifacts\Ffsti.Library.2.0.0.nupkg", dotNetNuGetPushSettings);
    DotNetNuGetPush(rootAbsoluteDir + @"\artifacts\Ffsti.Library.Database.2.0.0.nupkg", dotNetNuGetPushSettings);

});

Task("Test")
    .WithCriteria(isLocalBuild)
    .IsDependentOn("Publish")
    .Does(() =>
{
    DotNetTest("./Ffsti.Library.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);