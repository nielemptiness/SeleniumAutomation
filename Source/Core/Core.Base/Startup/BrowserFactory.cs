using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Serilog;

namespace Core.Base.Startup
{
    public class BrowserFactory
    {
        public Dictionary<DRIVERS, ThreadLocal<IWebDriver>> drivers = new()
        {
            { DRIVERS.ChromeDriver, null },
            { DRIVERS.FirefoxDriver, null }
        };
        private static ChromeOptions GetChromeOptions(bool ifHeadless, bool ifIncognito)
        {
            var chromeOptions = new ChromeOptions();
            
            if (ifHeadless)
            {
                chromeOptions.AddArgument("headless");
                chromeOptions.AddArgument("window-size=1920,1080");
                chromeOptions.AddArgument("disable-gpu");
                chromeOptions.AddArgument("privileged");
                Log.Information("Current browser is in headless mode.");
            }
            if (ifIncognito)
            {
                chromeOptions.AddArgument("incognito");
            }
            
            chromeOptions.AddArgument("start-maximized");
            chromeOptions.AddArgument("allow-running-insecure-content");
            chromeOptions.AddArgument("no-sandbox");
            chromeOptions.AddArgument("ignore-certificate-errors");
            chromeOptions.AddArgument("disable-extensions");
            chromeOptions.AddArgument("proxy-server='direct://'");
            chromeOptions.AddArgument("proxy-bypass-list=*");
            chromeOptions.AddArgument("disable-extensions");
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);

            return chromeOptions;
        }

        private static FirefoxOptions GetFirefoxOptions(bool ifHeadless, bool ifIncognito)
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            if (ifHeadless)
            {
                firefoxOptions.AddArgument("--headless");
                firefoxOptions.AddArgument("--width=1920");
                firefoxOptions.AddArgument("--height=1080");
                Log.Information("Current browser is in headless mode.");
            }
            if (ifIncognito)
            {
                firefoxOptions.AddArgument("--private");
            }
            firefoxOptions.SetPreference("download.prompt_for_download", false);
            firefoxOptions.SetPreference("download.directory_upgrade", true);
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk","application / vnd.microsoft.portable - executable");

            return firefoxOptions;

        }
        
        private static string GetRemoteHub()
        {
            var hub = Environment.GetEnvironmentVariable("SELENIUM_HUB");
            var host = hub ?? "localhost";
            Log.Information($"Current WebDriver host is {host}");
            return "http://" + host + ":4444/wd/hub";
        }
        private void InitDriver(DRIVERS driverType, bool ifHeadless, bool ifIncognito)
        {
            switch (driverType)
            {
                case DRIVERS.ChromeDriver:
                    if (drivers[DRIVERS.ChromeDriver] == null)
                    {
                        if (Config.Environment != "development")
                        {
                            var address = GetRemoteHub();
                            var remoteOptions = GetChromeOptions(ifHeadless, ifIncognito); ;
                            drivers[DRIVERS.ChromeDriver] = new ThreadLocal<IWebDriver>(() => new RemoteWebDriver(new Uri(address),
                                remoteOptions));
                        }
                        else
                        {
                            var options = GetChromeOptions(ifHeadless, ifIncognito);
                            drivers[DRIVERS.ChromeDriver] = new ThreadLocal<IWebDriver>(() => 
                                new ChromeDriver(ChromeDriverService.CreateDefaultService(),
                                options, TimeSpan.FromSeconds(120)));
                        }
                    }
                    break;
                
                case DRIVERS.FirefoxDriver:
                    if (drivers[DRIVERS.FirefoxDriver] == null)
                    {
                        if (Config.Environment != "development")
                        {
                            var address = GetRemoteHub();
                            var remoteOptions = GetFirefoxOptions(ifHeadless, ifIncognito);

                            drivers[DRIVERS.FirefoxDriver] = new ThreadLocal<IWebDriver>(() =>
                                new RemoteWebDriver(new Uri(address),
                                remoteOptions));
                        }
                        else
                        {
                            var options = GetFirefoxOptions(ifHeadless, ifIncognito);

                            drivers[DRIVERS.FirefoxDriver] = new ThreadLocal<IWebDriver>(() => 
                                new FirefoxDriver(FirefoxDriverService.CreateDefaultService(),
                                options, TimeSpan.FromSeconds(120)));
                        }
                        drivers[DRIVERS.FirefoxDriver].Value.Manage().Window.Size = new Size(1920, 1080);
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(driverType), driverType, "No such driver!");
            }
        }
        
        public ThreadLocal<IWebDriver> GetDriver(DRIVERS driverToReceive = DRIVERS.ChromeDriver,
            bool ifHeadLess = true, bool ifIncognito = false, int globalWait = 5)
        {
            Log.Information($"Creating driver of type '{driverToReceive}'");
            InitDriver(driverToReceive, ifHeadLess, ifIncognito);
            var driver = drivers[driverToReceive];
            driver.Value.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(globalWait));
            return driver;
        }
    }
    public enum DRIVERS
    {
        ChromeDriver,
        FirefoxDriver
    }
}
