namespace Chirp.Web.End2EndTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CreateCheepTests : PageTest
{
    bool isSetup = false;

    BrowserTypeLaunchOptions browserTypeLaunchOptions = new BrowserTypeLaunchOptions
    {
        Headless = false,
    };

    BrowserNewContextOptions browserNewContextOptions = new BrowserNewContextOptions
    {
        IgnoreHTTPSErrors = true,
        StorageStatePath = "state.json"
    };

    [SetUp]
    public async Task SetUp()
    {
        if (isSetup) return;

        await using var browser = await Playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

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
    [Category("End2End")]
    public async Task AuthenticatedUserCanCreateCheepFromPublicAndPrivateTimeline()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

        var context = await browser.NewContextAsync(browserNewContextOptions);

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7102/");

        await page.Locator("#CheepMessage").ClickAsync();

        await page.Locator("#CheepMessage").FillAsync("Testing public timeline");

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Testing public timeline");

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.Locator("#CheepMessage").ClickAsync();

        await page.Locator("#CheepMessage").FillAsync("Testing private timeline");

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Testing private timeline");
    }

    [Test]
    [Category("End2End")]
    public async Task AuthenticatedUserCanFollowAndUnfollowUser()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

        var context = await browser.NewContextAsync(browserNewContextOptions);

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7102/");

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine — 2023-08-01 13:17:39 Follow Starbuck now is what we hear" }).GetByRole(AriaRole.Button).First.ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Jacqualine Gilcoine Unfollow Starbuck now is what we hear the worst. ❤️ 1 like — 2023-08-01 13:17:39");

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Unfollow Starbuck now is what we hear the worst. ❤️ 1 like" }).GetByRole(AriaRole.Button).First.ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Follow");

    }
}
