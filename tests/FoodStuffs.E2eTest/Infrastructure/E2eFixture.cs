using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Playwright;
using Xunit;

namespace FoodStuffs.E2eTest.Infrastructure;

public class E2eFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    private IPlaywright? _playwright;
    private string? _searchIndexFolder;

    public IBrowser Browser { get; private set; } = null!;
    public string BaseUrl { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _searchIndexFolder = Path.Combine(Path.GetTempPath(), "FoodStuffs_E2eTest", "Lucene");

        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.FoodStuffs_AppHost>();

        _app = await builder.BuildAsync();
        var notificationService = _app.ResourceNotifications;
        await _app.StartAsync();

        await notificationService.WaitForResourceAsync("web", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromMinutes(5));

        BaseUrl = _app.GetEndpoint("web", "http").ToString().TrimEnd('/');

        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync();
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright?.Dispose();

        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }

        if (_searchIndexFolder is not null && Directory.Exists(_searchIndexFolder))
        {
            try
            {
                Directory.Delete(_searchIndexFolder, recursive: true);
            }
            catch
            {
                // Best-effort cleanup
            }
        }
    }
}
