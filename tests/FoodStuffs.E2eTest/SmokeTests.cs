using FoodStuffs.E2eTest.Infrastructure;
using Microsoft.Playwright;
using Xunit;

namespace FoodStuffs.E2eTest;

[Collection("E2e")]
public class SmokeTests : IAsyncLifetime
{
    private readonly E2eFixture _fixture;
    private IBrowserContext _context = null!;
    private IPage _page = null!;

    public SmokeTests(E2eFixture fixture) => _fixture = fixture;

    public async Task InitializeAsync()
    {
        _context = await _fixture.Browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task HomePage_DisplaysMainElement()
    {
        await _page.GotoAsync(_fixture.BaseUrl);
        var main = _page.Locator("main#main");
        await Assertions.Expect(main).ToBeVisibleAsync();
    }
}
