using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_PageObjectPattern.Page_Objects
{
    internal class CareersPage: BasePage
    {
        public CareersPage(IWebDriver driver) : base(driver) { }
                
        //Locators for TC1
        private readonly By careersBttn = By.XPath("//li[@class='top-navigation__item epam'][5]");
        private readonly By keywordsField = By.Id("new_form_job_search-keyword");
        private readonly By locationField = By.XPath("//span[@class='select2-selection select2-selection--single']"); //
        private readonly By remoteCheck = By.XPath("//label[@for='id-93414a92-598f-316d-b965-9eb0dfefa42d-remote']");
        private readonly By findBttn = By.XPath("//button[@type='submit']");
        private readonly By latestItemOnList = By.XPath("//ul[@class='search-result__list']/li[last()]//div//div/a");
        private readonly By requirementsList = By.XPath("//div[@class='vacancy-details-23__content-holder']/ul[2]");
        private readonly By listItem = By.TagName("li");
        
        // Methods for interacting with the Careers Page TC1
        public void ClickCareersBttn()
        {
            Driver.FindElement(careersBttn).Click();
        }

        public void EnterLanguage(string language)
        {
            Driver.FindElement(keywordsField).SendKeys(language);
        }

        public void SelectLocation(string location)
        {
            actions.MoveToElement(Driver.FindElement(locationField))
                    .Click()
                    .SendKeys(location)
                    .SendKeys(Keys.Enter)
                    .Perform();
        }

        public void CheckRemoteOption()
        {
            Driver.FindElement(remoteCheck).Click();
        }

        public void ClickFindBttn()
        {
            Driver.FindElement(findBttn).Click();
        }

        public void AccessLatestJobPosition()
        {
            while (true)
            {
                WaitForElementToDisapear(loaderSpinner);
                actions.ScrollToElement(Driver.FindElement(latestItemOnList)).Perform();
                // Check if the loading animation appears again
                bool isLoadingVisible = Driver.FindElements(loaderSpinner).Any(e => e.Displayed);
                if (!isLoadingVisible)
                {
                    actions.MoveToElement(Driver.FindElement(latestItemOnList))
                           .Click()
                           .Perform();
                    break;
                }
            }
        }

        public bool ReturnProgrammingLanguage(string programmingLanguage)
        {
            var requirements = Driver.FindElement(requirementsList);
            var listItems = requirements.FindElements(listItem);
            bool containsLanguage = listItems.Any(item => item.Text.Contains(programmingLanguage, StringComparison.OrdinalIgnoreCase));
            return containsLanguage;
        }     
    }
}
