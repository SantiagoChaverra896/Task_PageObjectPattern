using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace Task_PageObjectPattern.Page_Objects
{
    internal class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait wait;
        protected Actions actions;

        // Constructor
        public BasePage(IWebDriver driver)
        {
            this.Driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            this.actions = new Actions(driver);
        }

        // Common Methods - Wait Helpers
        protected IWebElement WaitForElement(By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        protected void WaitForElementToDisapear(By locator)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        // Common Elements
        protected By loaderSpinner = By.CssSelector(".preloader");

    }
}
