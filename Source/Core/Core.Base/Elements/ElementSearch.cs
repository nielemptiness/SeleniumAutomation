using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Serilog;

namespace Core.Base.Elements
{
    public class ElementSearch
    {
        public static IWebElement FindElement(IWebDriver driver, string locator)
        {
            Log.Information($"Searching for el.  with {locator}");
            try
            {
                var xpathLocator = ElementSearchHelper.WaitClickable(driver, locator);
                var elementToReturn = driver.FindElement(xpathLocator);
                Log.Information("Found!");
                return elementToReturn;
            }
            catch (WebDriverTimeoutException e) when (e.InnerException != null)
            {
                Log.Error($"Unable to find element with xpath \'{locator}\'");
                throw;
            }
            catch (WebDriverException)
            {
                Log.Error($"TimedOut searching for \'{locator}\'");
                throw;
            }
        }
        
        public static List<IWebElement> FindElementsList(IWebDriver driver, string locator)
        {
            Log.Information($"Searching for list of el-s. with {locator}");
            try
            {
                var xpathLocator =  ElementSearchHelper.WaitClickable(driver, locator);
                var elementToReturn = driver.FindElements(xpathLocator).ToList();
                Log.Information("Found!");
                return elementToReturn;
            }
            catch (WebDriverTimeoutException e) when (e.InnerException != null)
            {
                Log.Error($"Unable to find elements list with xpath \'{locator}\'");
                throw;
            }
            catch (WebDriverException)
            {
                Log.Error($"TimedOut searching for \'{locator}\'");
                throw;
            }
        }

        public static IWebElement FindNotClickableEl(IWebDriver driver, string locator)
        {
            Log.Information($"Searching for not interactable el.  with {locator}");
            try
            {
                var xpathLocator =  ElementSearchHelper.WaitExists(driver, locator);
                var elementToReturn = driver.FindElement(xpathLocator);
                Log.Information("Found!");
                return elementToReturn;
            }
            catch (WebDriverTimeoutException e) when (e.InnerException != null)
            {
                Log.Error($"Unable to find checkbox with xpath \'{locator}\' ");
                throw;
            }
            catch (WebDriverException)
            {
                Log.Error($"TimedOut searching for \'{locator}\'");
                throw;
            }
        }
    }
}