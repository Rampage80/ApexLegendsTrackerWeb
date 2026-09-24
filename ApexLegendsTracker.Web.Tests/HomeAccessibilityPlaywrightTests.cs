using Microsoft.Playwright;

namespace ApexLegendsTracker.Web.Tests;

public sealed class HomeAccessibilityPlaywrightTests : IAsyncLifetime
{
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL") ?? "http://localhost:5001";
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;
    private bool _serverUnavailable;
    private bool _browserUnavailable;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        try
        {
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        }
        catch (PlaywrightException)
        {
            _browserUnavailable = true;
            return;
        }

        _page = await _browser.NewPageAsync();

        try
        {
            await _page.GotoAsync(_baseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 5000 });
        }
        catch (PlaywrightException)
        {
            _serverUnavailable = true;
        }
    }

    [Fact]
    public async Task Home_has_accessible_navigation_headings_and_form_names()
    {
        SkipWhenServerUnavailable();

        IPage page = _page!;
        await page.Locator("main").WaitForAsync();

        Assert.Equal(1, await page.Locator("h1").CountAsync());
        Assert.Equal("Know the game before you queue.", await page.Locator("h1").InnerTextAsync());
        Assert.Equal(1, await page.GetByRole(AriaRole.Navigation, new() { Name = "Primary navigation" }).CountAsync());
        Assert.Equal(1, await page.GetByLabel("Player name").CountAsync());
        Assert.Equal(1, await page.GetByLabel("Platform").CountAsync());
        Assert.Equal(1, await page.GetByRole(AriaRole.Textbox, new() { Name = "Your Apex Legends question" }).CountAsync());
        Assert.Equal(1, await page.GetByRole(AriaRole.Link, new() { Name = "Skip to main content" }).CountAsync());
    }

    [Fact]
    public async Task Home_has_visible_keyboard_focus_and_chat_is_visible_on_desktop()
    {
        SkipWhenServerUnavailable();

        IPage page = _page!;
        await page.SetViewportSizeAsync(1440, 900);
        await page.GetByLabel("Player name").FocusAsync();

        string outlineStyle = await page.GetByLabel("Player name").EvaluateAsync<string>(
            "element => getComputedStyle(element).outlineStyle");
        string outlineWidth = await page.GetByLabel("Player name").EvaluateAsync<string>(
            "element => getComputedStyle(element).outlineWidth");
        bool chatFitsViewport = await page.Locator(".home-chat-section .chat-panel").EvaluateAsync<bool>(
            "element => element.getBoundingClientRect().bottom <= window.innerHeight");

        Assert.NotEqual("none", outlineStyle);
        Assert.NotEqual("0px", outlineWidth);
        Assert.True(chatFitsViewport);
    }

    [Fact]
    public async Task Home_has_no_horizontal_overflow_on_mobile()
    {
        SkipWhenServerUnavailable();

        IPage page = _page!;
        await page.SetViewportSizeAsync(390, 844);

        bool hasHorizontalOverflow = await page.EvaluateAsync<bool>(
            "() => document.documentElement.scrollWidth > document.documentElement.clientWidth");
        bool searchPanelIsPresent = await page.Locator(".home-lookup-panel").IsVisibleAsync();
        bool chatIsPresent = await page.GetByRole(AriaRole.Heading, new() { Name = "Ask AI about Apex" }).IsVisibleAsync();

        Assert.False(hasHorizontalOverflow);
        Assert.True(searchPanelIsPresent);
        Assert.True(chatIsPresent);
    }

    [Fact]
    public async Task Home_restores_chat_messages_after_refresh()
    {
        SkipWhenServerUnavailable();

        IPage page = _page!;
        await page.EvaluateAsync("""
            () => localStorage.setItem('apexTracker.chatMessages', JSON.stringify([
                { Text: 'Remember my question', IsUser: true }
            ]))
            """);
        await page.ReloadAsync(new PageReloadOptions { WaitUntil = WaitUntilState.NetworkIdle });

        Assert.True(await page.GetByText("Remember my question").IsVisibleAsync());
    }

    public async Task DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    private void SkipWhenServerUnavailable()
    {
        if (_serverUnavailable || _browserUnavailable)
        {
            throw Xunit.Sdk.SkipException.ForSkip(
            $"Install Playwright Chromium and start the Blazor app before running browser tests. Target: {_baseUrl}");
        }
    }
}