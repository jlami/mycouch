public class BuildConfig
{
    private const string Version = "7.1.0";

    public readonly string SrcDir = "./source/";
    public readonly string ArtifactsDir = "./artifacts/";    
    public readonly string TestResultsDir = "./testresults/";

    public string Target { get; private set; }
    public string SemVer { get; private set; }
    public string BuildVersion { get; private set; }
    public string BuildProfile { get; private set; }

    public static BuildConfig Create(
        ICakeContext context,
        BuildSystem buildSystem)
    {
        if (context == null)
            throw new ArgumentNullException("context");

        var buildRevision = context.EnvironmentVariable("Build.BuildNumber", "0");
        var isFork = context.EnvironmentVariable("System.PullRequest.IsFork", false);
        var branchName = context.EnvironmentVariable("Build.SourceBranchName", string.Empty).ToLowerInvariant();
        var isPreRelease = branchName != "master" || isFork;

        return new BuildConfig
        {
            Target = context.Argument("target", "Default"),
            SemVer = Version + (isPreRelease ? $"-pre{buildRevision}" : string.Empty),
            BuildVersion = $"{Version}.{buildRevision}",
            BuildProfile = context.Argument("configuration", "Release")
        };
    }
}
