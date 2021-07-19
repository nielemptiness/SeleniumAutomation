using Core.Base.Elements;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class CartPage
    {
        private IWebDriver _driver { get; }

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement ConfirmOrderButton => ElementSearch.FindElement(_driver,
            "//div[@class = 'cart-receipt ng-star-inserted']//a");

        public CheckOutPage GoToCheckOut()
        {
            ConfirmOrderButton.Click();
            return new CheckOutPage(_driver);
        }
    }
}