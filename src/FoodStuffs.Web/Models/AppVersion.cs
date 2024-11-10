namespace FoodStuffs.Web.Models;

internal record AppVersion(
    string? Version,
    bool IsPublicRelease,
    bool IsPrerelease,
    string GitCommitId,
    DateTime GitCommitDate,
    string AssemblyConfiguration);
