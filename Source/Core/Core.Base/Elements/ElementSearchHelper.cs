using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Base.Elements
{
    public static class ElementSearchHelper
    {
        private static WebDriverWait wait;
        private static readonly int defWaitTime = 5;
        private static void InitWait(IWebDriver driver, int timespan)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timespan));
        } 
        
        public static By WaitClickable(IWebDriver driver, string locator)
        {
            var xpathLocator = By.XPath(locator);
            InitWait(driver, defWaitTime);
            wait.Until(ExpectedConditions.ElementToBeClickable(xpathLocator));
            return xpathLocator;
        }

        public static By WaitExists(IWebDriver driver, string locator)
        {
            var xpathLocator = By.XPath(locator);
            InitWait(driver, defWaitTime);
            wait.Until(ExpectedConditions.ElementExists(xpathLocator));
            return xpathLocator;
        }
    }
}