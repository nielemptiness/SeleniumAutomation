using Core.Base.Elements;
using OpenQA.Selenium;

namespace Tests.PageObjects
{
    public class PcAndLaptopsPage
    {
        private IWebDriver _driver { get; }

        public PcAndLaptopsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public static readonly string LaptopSection = "Ноутбуки";

        private IWebElement Section(string section) => ElementSearch.FindElement(_driver,
            $"//a[@title = '{section}'][contains(@class, 'tile-cats__heading_type_center')]");

        //Mocked
        public object OpenSection(string section)
        {
            Section(section).Click();
            return new object();
        }
        
        public LaptopSelectionPage OpenLaptopSelectionPage()
        {
            Section(LaptopSection).Click();
            return new LaptopSelectionPage(_driver);
        }
            
            
    }
}