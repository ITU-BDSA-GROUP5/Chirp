namespace Chirp.Web.End2EndTests;

// [Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CreateCheepTests : PageTest
{
    bool isSetup = false;

    [SetUp]
    public async Task SetUp()
    {
        if (isSetup) return;

        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7102/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.WaitForURLAsync("https://localhost:7102/", new PageWaitForURLOptions() { Timeout = 0 });

        await context.StorageStateAsync(new()
        {
            Path = "state.json"
        });

        isSetup = true;
    }

    [Test]
    public async Task AuthenticatedUserCanCreateCheepFromPublicTimeline()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            StorageStatePath = "state.json"
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7102/");

        await page.WaitForURLAsync("https://localhost:7102/", new PageWaitForURLOptions() { Timeout = 0 });

        await page.Locator("#CheepMessage").ClickAsync();

        await page.Locator("#CheepMessage").FillAsync("Testing public timeline");

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Testing public timeline");
    }

    [Test]
    public async Task AuthenticatedUserCanCreateCheepFromPrivateTimeline()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            StorageStatePath = "state.json"
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7102/");

        //await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.WaitForURLAsync("https://localhost:7102/", new PageWaitForURLOptions() { Timeout = 0 });

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.Locator("#CheepMessage").ClickAsync();

        await page.Locator("#CheepMessage").FillAsync("Testing private timeline");

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Testing private timeline");
    }
}
