using System.Collections.Generic;
using System.Linq;
using Core.Base.Elements;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class LaptopSelectionPage
    {
        private IWebDriver _driver { get; }

        public LaptopSelectionPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private List<IWebElement> AddToCart => ElementSearch.FindElementsList(_driver,
            "//app-buy-button");

        public CartPage AddFirstToCart()
        {
            var button = AddToCart.FirstOrDefault();
            button?.Click();

            return new CartPage(_driver);
        }
    }
}