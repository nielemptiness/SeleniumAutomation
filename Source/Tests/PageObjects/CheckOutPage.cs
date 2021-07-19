using System.Threading;
using System.Threading.Tasks;
using Core.Base.Elements;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class CheckOutPage
    {
        private IWebDriver _driver { get; }

        public CheckOutPage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        private static readonly string PageLink = "https://rozetka.com.ua/checkout/";

        private IWebElement PageHeader => ElementSearch.FindNotClickableEl(_driver,
            "//h1[contains(@class, 'checkout-heading')]");

        public bool IsCheckoutPage()
        {
            var header = PageHeader.Displayed;
            var page = _driver.Url.Equals(PageLink);
            return page && header;
        }
    }
}