var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

var isLocalBuild = BuildSystem.IsLocalBuild;
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", BuildSystem.AppVeyor.Environment.Repository.Branch);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    Information(EnvironmentVariable<string>("NUGET_API_KEY", "42"));

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

Task("Test")
    .WithCriteria(isLocalBuild)
    .IsDependentOn("Build")
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