using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;

namespace Task_PageObjectPattern.Page_Objects
{
    internal class SearchPage: BasePage
    {
        public SearchPage(IWebDriver driver) : base(driver) { }
        
        // Locators for TC2
        private readonly By magnifierIcon = By.XPath("//button[@class='header-search__button header__icon']");
        private readonly By searchBar = By.Id("new_form_search");
        private readonly By searchBttn = By.CssSelector(".custom-button");
        private readonly By pageFooter = By.XPath("//div[@class='footer-inner']");
        private readonly By viewMoreBttn = By.XPath("//span[contains(text(),'View More')]");
        private readonly By linksTexts = By.XPath("//a[@class='search-results__title-link']");

        //Methods for TC2
        public void ClickMagnifierIcon()
        {
            Driver.FindElement(magnifierIcon).Click();
        }

        public void EnterSearchedValue(string value)
        {
            WaitForElement(searchBar);
            Driver.FindElement(searchBar).SendKeys(value);

        }

        public void ClickSearchBttn()
        {
            Driver.FindElement(searchBttn).Click();
        }

        public void ClickOnTheLatestItem()
        {
            WaitForElementToDisapear(loaderSpinner);
            actions.ScrollToElement(Driver.FindElement(pageFooter))
                   .Perform();
            WaitForElement(viewMoreBttn);
            actions.MoveToElement(Driver.FindElement(viewMoreBttn))
                   .Perform();

            while (Driver.FindElement(viewMoreBttn).Displayed)
            {
                Driver.FindElement(viewMoreBttn).Click();
                WaitForElementToDisapear(loaderSpinner);
                if (Driver.FindElement(viewMoreBttn).Displayed)
                {
                    actions.MoveToElement(Driver.FindElement(viewMoreBttn))
                           .Perform();
                }
                else
                {
                    break;
                }
            }
        }

        public List<string> ExtractAllLinksTexts()
        {
            List<string> texts = Driver.FindElements(linksTexts).Select(e => e.Text).ToList();
            return texts;
        }

        public int CountTextsWithSearchedValue(List<string> texts, string searchString)
        {
            int containing = texts.Count(text => text.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            return (containing);
        }

        public int CountTextsWithoutSearchedValue(List<string> texts, string searchString)
        {
            int notContaining = texts.Count(text => !text.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            return (notContaining);
        }

    }
}
