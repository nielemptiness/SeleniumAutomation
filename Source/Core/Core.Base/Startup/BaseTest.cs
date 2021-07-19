using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Serilog;

namespace Core.Base.Startup
{
    public class BaseTest
    {
        #region VARS
        private DRIVERS drivers { get; }
        protected IWebDriver driver;
        #endregion
        
        protected BaseTest(DRIVERS driverType)
        {
            drivers = driverType;
            Log.Information($"Current environment is '{Config.Environment}'");
        }
        
        #region Start
        [OneTimeSetUp]
        protected void OpenDriver()
        {
            try
            {
                driver = new BrowserFactory().GetDriver(driverToReceive: drivers, ifHeadLess: Config.ifHeadless, ifIncognito: true).Value;
            }
            catch (WebDriverException e)
            {
                Log.Fatal($"Unable to create instance of \'{drivers}\' at {e.StackTrace}");
                throw;
            }
        }
        #endregion
        
        #region Finish
        [TearDown]
        protected void End()
        {
            try
            {
                MakeSnapshotOnFailure();
            }
            finally
            {
                driver.Manage().Cookies.DeleteAllCookies();
            }
        }
        
        [OneTimeTearDown]
        protected void CloseDriver()
        {
            driver.Quit();
        }
        #endregion

        #region BaseHelpers
        private static string GetFailureTimeUtc() => DateTime.UtcNow.ToString("dd.MM.yyyy.HH.mm.ss");

        private void MakeSnapshotOnFailure()
        {
            if ((TestContext.CurrentContext.Result.Outcome != ResultState.Error) &&
                (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)) return;

            var url = driver.Url;
            
            Log.Error($"Test failed on {url}, taking screenshot...");
           
            var onFailureSnapshot = driver.TakeScreenshot();
            var neededPath = Path.Combine(TestContext.CurrentContext.WorkDirectory,
                $"failedTest_{TestContext.CurrentContext.Test.MethodName}_{GetFailureTimeUtc()}.png");
            onFailureSnapshot.SaveAsFile(neededPath, ScreenshotImageFormat.Png);
            
            TestContext.AddTestAttachment(neededPath, "OnFailureAction - made screenshot!");
            Log.Error(neededPath, onFailureSnapshot);
        }
        #endregion
    }
}