using Microsoft.Playwright;
using System.Threading.Tasks;
using FLaUIDemo.Common;

namespace FLaUIDemo.Browser
{
    public class WebBrowser
    {
        protected IBrowser browser;
        protected IBrowserContext context;
        protected IPlaywright playwright;

        public async Task ConnectToWebView2()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.ConnectOverCDPAsync($"http://localhost:{Configuration.WebView2DebuggingPort}");
        }

        public AccountPage AccountPage => new AccountPage(browser.Contexts[0].Pages[0]);

    }
}
