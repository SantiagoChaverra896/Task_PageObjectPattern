using Core.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Data;
using log4net.Config;
using log4net;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace Task_TestAutomationFramework
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected IWebDriver driver;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseTest));

        [SetUp]
        public void Setup()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            string logFilePath = Path.GetFullPath("Logs/TestAutomation.log");
            Console.WriteLine($"📂 Expected log file location: {logFilePath}");

            Logger.Info("🔹 log4net initialized before test execution.");

            Logger.Info("Starting Test setup...");
            driver = DriverManager.GetDriver("chrome");
            driver.Navigate().GoToUrl(Data.applicationUrl);
            Logger.Info("WebDriver initialized...");
            
        }

        [TearDown]
        public void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string testName = TestContext.CurrentContext.Test.Name;
                ScreenshotMaker.TakeScreenshot(driver, testName);
            }
            Logger.Info("Closing Browser");
            DriverManager.QuitDriver();
        }
    }
}
