using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_PageObjectPattern.Page_Objects
{
    internal class InsightsPage: BasePage
    {
        public InsightsPage(IWebDriver driver) : base(driver) { }

        //Locators for TC4
        private readonly By insightsBttn = By.XPath("//li[@class='top-navigation__item epam'][3]");
        private readonly By nextSlideBttn = By.XPath("//body/div[@id='wrapper']/main[@id='main']/div[contains(@class,'content-container parsys')]/div[3]/div[1]/div[2]/button[2]//*[name()='svg']");
        private readonly By articleTitle = By.XPath("//div[contains(@class,'owl-item active')]//span[contains(@class,'font-size-44')]");
        private readonly By readMoreBttn = By.CssSelector("body > div:nth-child(5) > main:nth-child(3) > div:nth-child(1) > div:nth-child(3) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(7) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > a:nth-child(1)");
        private readonly By actualTitle = By.CssSelector("span[class='font-size-80-33'] span[class='museo-sans-light']");

        //Methods for TC 4
        public void ClickInsightsBttn()
        {
            Driver.FindElement(insightsBttn).Click();
        }

        public void ClickNextSlideBttn()
        {
            Driver.FindElement(nextSlideBttn).Click();
            WaitForElement(articleTitle);
        }

        public string GetArticleTitle()
        {
            WaitForElement(articleTitle);
            return Driver.FindElement(articleTitle).Text.Trim();
        }

        public void ClickReadMoreBttn()
        {
            Driver.FindElement(readMoreBttn).Click();
        }

        public string GetActuaTitle()
        {
            WaitForElement(actualTitle);
            return Driver.FindElement(actualTitle).Text.Trim();
        }

    }
}
