using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(NuGetApiKey) },
    InvokedTargets = new[] { nameof(Publish) })]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    [Parameter] [Secret] readonly string NuGetApiKey;
    [Parameter] readonly string NuGetSource;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    static AbsolutePath ArtifactDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Description("Cleans the project")
        .Executes(() =>
        {
            DotNetClean(_ => _
                .SetProject(Solution)
                .SetConfiguration(Configuration));
        });

    Target Restore => _ => _
        .Description("Restores project dependencies")
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .Description("Builds the project")
        .DependsOn(Clean, Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .Description("Tests the project")
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .Description("Generates the project package")
        .DependsOn(Compile)
        .After(Compile)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactDirectory);

            DotNetPack(_ => _
                .SetProject(Solution.ShopSharp_Core_Domain)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(ArtifactDirectory)
                .SetVersion(GitVersion.NuGetVersionV2)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Publish => _ => _
        .Description("Publishes the generated project package")
        .DependsOn(Test, Pack)
        .Requires(() => NuGetSource, () => NuGetApiKey)
        .OnlyWhenStatic(() => Configuration == Configuration.Release && GitRepository.IsOnMainBranch())
        .Executes(() =>
        {
            var packages = GlobFiles(ArtifactDirectory, "*.nupkg");

            DotNetNuGetPush(_ => _
                .SetSource(NuGetSource)
                .SetApiKey(NuGetApiKey)
                .CombineWith(packages, (_, package) => _
                    .SetTargetPath(package)));
        });

    public static int Main() => Execute<Build>(x => x.Test);
}
