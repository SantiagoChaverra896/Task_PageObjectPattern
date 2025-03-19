using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_PageObjectPattern.Page_Objects
{
    internal class AboutPage: BasePage
    {
        public AboutPage(IWebDriver driver) : base(driver) { }
        //Locators for TC3
        private readonly By aboutBttn = By.XPath("//li[@class='top-navigation__item epam'][4]");
        private readonly By epamAAGSec = By.XPath("//span[contains(text(),'EPAM at')]");
        private readonly By downloadBttn = By.XPath("//span[@class='button__content button__content--desktop'][normalize-space()='DOWNLOAD']");

        //Methods for TC3
        public void ClickAboutBttn()
        {
            Driver.FindElement(aboutBttn).Click();
        }

        public void ScrolltoSection()
        {
            actions.MoveToElement(Driver.FindElement(epamAAGSec))
                   .Perform();
        }

        public void ClickDownloadBttn()
        {
            Driver.FindElement(downloadBttn).Click();
        }

        public bool WaitForFileDownload(string directory, string fileName, int timeoutInSeconds)
        {
            string filePath = Path.Combine(directory, fileName);
            int elapsed = 0;

            while (elapsed < timeoutInSeconds)
            {
                if (File.Exists(filePath))
                    return true;  //File is found, exit early

                Task.Delay(1000).Wait();  //Non-blocking wait
                elapsed++;
            }

            return false;  //File was not downloaded in time
        }

        public void CloseOpenedPdf()
        {
            string[] pdfViewers = { "AcroRd32", "Acrobat" }; // Common PDF viewers

            foreach (var processName in pdfViewers)
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(); // Ensure it fully closes
                        Console.WriteLine($"Closed {processName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not close {processName}: {ex.Message}");
                    }
                }
            }
        }

        public void DeleteDownloadedFile(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"✅ File {fileName} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Could not delete {fileName}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"⚠️ File {fileName} does not exist.");
            }
        }

    }
}
