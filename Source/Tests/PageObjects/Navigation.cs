using Core.Base;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class Navigation
    {
        private IWebDriver _driver { get; }

        public Navigation(IWebDriver driver)
        {
            _driver = driver;
        }
        
        public MainPage GoToMainPage()
        {
            _driver.Navigate().GoToUrl(Config.MainPageLink);
            return new MainPage(_driver);
        }
    }
}