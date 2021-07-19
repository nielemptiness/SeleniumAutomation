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

        public bool IsCheckoutPage()
        {
            return _driver.Url.Equals(PageLink);
        }
    }
}