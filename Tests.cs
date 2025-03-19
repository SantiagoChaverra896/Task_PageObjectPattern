using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Data.Common;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using static System.Collections.Specialized.BitVector32;
using SeleniumExtras.WaitHelpers;
using static OpenQA.Selenium.BiDi.Modules.Input.Wheel;
using System;
using Task_PageObjectPattern.Page_Objects;
using System.Diagnostics;

namespace LocatorsPractice
{
    public class Tests
    {
        public IWebDriver driver;
        private CareersPage careersPage;
        private SearchPage searchPage;
        private AboutPage aboutPage;
        private InsightsPage insightsPage;
        private string downloadPath;
        private readonly string fileName = "EPAM_Corporate_Overview_Q4FY-2024.pdf"; // used for TC3
        private readonly int numberOfClicks = 2; // used for TC4
        
        [SetUp]
        public void Setup()
        {   //incorporate an option to run tests in headless mode
            bool isHeadless = false;

            //download Path
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Configure ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", downloadPath);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            if (isHeadless)
            {
                options.AddArgument("--headless=new"); // Enable headless mode
                options.AddArgument("--disable-gpu"); // Recommended for Windows
                options.AddArgument("--window-size=1920,1080"); // Set a default window size
            }

            Console.WriteLine($"Headless mode: {isHeadless}");


            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.epam.com");
            driver.Manage().Window.Maximize();

            cookiesManagement(driver);
            // Initialize HomePage
            careersPage = new CareersPage(driver);
            searchPage = new SearchPage(driver);
            aboutPage = new AboutPage(driver);
            insightsPage = new InsightsPage(driver);
        }

        [Test]
        [TestCase("SAP", "Bogota")]
        [TestCase("Python", "All Locations")]
        [TestCase("C#", "Greece")]
        public void Test1(String programmingLanguage, string location)
        {
            try
            {
                careersPage.ClickCareersBttn();
                careersPage.EnterLanguage(programmingLanguage);
                careersPage.SelectLocation(location);
                careersPage.CheckRemoteOption();
                careersPage.ClickFindBttn();
                careersPage.AccessLatestJobPosition();

                bool containsLanguage = careersPage.ReturnProgrammingLanguage(programmingLanguage);

                if (containsLanguage)
                {
                    Console.WriteLine($"Test Passed: Found '{programmingLanguage}' in Requirements description.");
                    Assert.Pass();
                }
                else
                {
                    Console.WriteLine($"Test Failed: '{programmingLanguage}' not found in Requirements description.");
                    Assert.Fail();
                }
            }
            finally
            {

                driver.Quit();
            }
        }

        [Test]
        [TestCase("CLOUD")]
        [TestCase("BLOCKCHAIN")]
        [TestCase("Automation")]
        public void Test2(string searchString)
        {
            try
            {
                searchPage.ClickMagnifierIcon();
                searchPage.EnterSearchedValue(searchString);
                searchPage.ClickSearchBttn();
                searchPage.ClickOnTheLatestItem();

                var linksTexts = searchPage.ExtractAllLinksTexts();
                int containing = searchPage.CountTextsWithSearchedValue(linksTexts, searchString);
                int notContaining = searchPage.CountTextsWithoutSearchedValue(linksTexts, searchString);

                //Validate that all the items contain the searchString
                bool allContainSearchString = linksTexts.All(text => text.Contains(searchString, StringComparison.OrdinalIgnoreCase));

                if (allContainSearchString)
                {
                    Assert.Pass();
                    Console.WriteLine($"All the link´s text contain the search value: {searchString}");
                }
                else
                {
                    Console.WriteLine($"{containing} link´s text contain the {searchString} value");
                    Console.WriteLine($"{notContaining} link´s text do not contain the {searchString} value");
                    Assert.Fail();
                }
            }
            finally
            {
                driver.Quit();
            }
        }

        [Test]
        public void Test3() 
        {
            try 
            {
                aboutPage.ClickAboutBttn();
                aboutPage.ScrolltoSection();
                aboutPage.ClickDownloadBttn();
                Assert.IsTrue(aboutPage.WaitForFileDownload(downloadPath, fileName, 30), $"File {fileName} was not downloaded");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to process TC3", e);
            }
            finally 
            { 
                driver.Quit();
                aboutPage.CloseOpenedPdf();
                aboutPage.DeleteDownloadedFile(downloadPath, fileName);
            }
        
        }

        [Test]
        public void Test4() 
        {
            try
            {
                insightsPage.ClickInsightsBttn();
                
                // Loop to click the button the specified number of times
                for (int i = 0; i < numberOfClicks; i++)
                {
                    insightsPage.ClickNextSlideBttn();
                }

                string articleName = insightsPage.GetArticleTitle();
                insightsPage.ClickReadMoreBttn();
                string actualArticleName = insightsPage.GetActuaTitle();
                Assert.That(actualArticleName, Is.EqualTo(articleName), $"Expected '{articleName}', but found '{actualArticleName}'");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to process TC4", e);
            }
            finally
            {
                driver.Quit();
            }
        }

        public void cookiesManagement(IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement AcceptBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("onetrust-accept-btn-handler")));
                
                Actions actions = new Actions(driver);
                actions.MoveToElement(AcceptBtn).Click().Perform();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("onetrust-banner-sdk")));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cookies banner not found, proceeding...");
            }
        }
        
    }
}