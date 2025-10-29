using System.Threading.Tasks;
using Microsoft.Playwright;
using FLaUIDemo.Common;

namespace FLaUIDemo.Browser
{
    public class AccountPage
    {
        private readonly IPage _page;

        public AccountPage(IPage page)
        {
            _page = page;
            //_page.WaitForLoadStateAsync(LoadState.Load);
        }

        public ILocator Login => _page.Locator("//a[@href='#login']");
        
        public ILocator Email => _page.Locator("//input[@type='email']");

        public ILocator Password => _page.Locator("//input[@type='password']");

        public ILocator Submit => _page.Locator("//input[@type='submit']");

        public async Task LogIn()
        {
            await _page.ClickAsync("//a[@href='#login']");
            await _page.ClickAsync("//input[@type='email']");
            await _page.Keyboard.TypeAsync(Configuration.AccountEmail);
            await _page.ClickAsync("//input[@type='password']");
            await _page.Keyboard.TypeAsync(Configuration.AccountPassword);
            await _page.ClickAsync("//input[@type='submit']");
            /*
            await Login.ClickAsync();
            await Email.FillAsync(Configuration.AccountEmail);
            await Submit.ClickAsync();
            await Password.FillAsync(Configuration.AccountPassword);
            await Submit.ClickAsync();
            */
        }
    }
}
