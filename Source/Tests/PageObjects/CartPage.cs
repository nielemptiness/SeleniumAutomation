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

        private IWebElement OpenCartButton => ElementSearch.FindElement(_driver,
            "//li//rz-cart//button");

        public CheckOutPage GoToCheckOut()
        {
            OpenCartButton.Click();
            ConfirmOrderButton.Click();
            return new CheckOutPage(_driver);
        }
    }
}