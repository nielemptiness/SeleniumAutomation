using Core.Base.Elements;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class MainPage
    {
        private IWebDriver _driver { get; }

        public MainPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement ProductSection(string productName) => ElementSearch.FindElement(_driver, "");
    }
}