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

        private static readonly string LaptopSection = "Ноутбуки и компьютеры";
        private IWebElement ProductSection(string productName) => ElementSearch.FindNotClickableEl(_driver, 
            $"(//ul//li[contains(@class, 'menu-categories__item')]//a[text() = '{productName}'])[2]");
        

        public MainPage OpenProductSection(string productName)
        {
            ProductSection(productName).Click();
            return new MainPage(_driver);
        }
        
        public PcAndLaptopsPage OpenLaptopsSection()
        {
            ProductSection(LaptopSection).Click();
            return new PcAndLaptopsPage(_driver);
        }
    }
}