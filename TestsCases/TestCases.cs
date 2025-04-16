using Business.Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Data;
using Core.Core;

namespace Task_TestAutomationFramework.TestsCases
{
    internal class TestCases: BaseTest
    {
        [Test]
        [TestCase("SAP", "Bogota")]
        [TestCase("Python", "All Locations")]
        [TestCase("C#", "Greece")]
        public void Test_01(string programmingLanguage, string location) 
        {
            try 
            {
                var careersWorkflow = new CareersWorkflow(driver);
                bool languageContained = careersWorkflow.PerformCareerSearch(programmingLanguage, location);

                Assert.IsTrue(languageContained);

                if (languageContained)
                {
                    Logger.Info($"Test 01 for programming language {programmingLanguage} and location {location} Passed");
                }
                else
                {
                    Logger.Info($"Test 01 for programming language {programmingLanguage} and location {location} Failed");
                }

                Logger.Info("TC01 Executed Succcessfully");
            }
            catch(Exception ex) 
            {
                Logger.Error("TC01 failed!", ex);
                throw;
            }
            
            
        }

        [Test]
        [TestCase("CLOUD")]
        [TestCase("BLOCKCHAIN")]
        [TestCase("Automation")]
        public void Test_02(string searchString) 
        {
            try
            {
                var searchWorkflow = new SearchWorkflow(driver);

                List<string> TextList = searchWorkflow.PerformSearch(searchString);
                bool ValidateContainingItems = searchWorkflow.ValidateAllItems(TextList, searchString);
                int ItemsContainingSearchedValue = searchWorkflow.CountTextsWithSearchedValue(TextList, searchString);
                int ItemsNotContainingSearchedValue = searchWorkflow.CountTextsWithoutSearchedValue(TextList, searchString);

                Assert.IsTrue(ValidateContainingItems);

                if (ValidateContainingItems)
                {
                    Logger.Info($"{ItemsContainingSearchedValue} links text contain the {searchString} value ");
                }
                else
                {
                    Logger.Info($"{ItemsContainingSearchedValue} links text contain the {searchString} value ");
                    Logger.Info($"{ItemsNotContainingSearchedValue} links text do not contain the {searchString} value ");
                }
            }
            catch (Exception ex) 
            {
                Logger.Error("TC02 Failed!", ex);
                throw;  
            }        
        }

        [Test]
        public void Test_03() 
        {
            var aboutWorkflow = new AboutWorkflow(driver);

            try
            {
                bool downloadedFile = aboutWorkflow.DownloadFile();
                Assert.IsTrue(downloadedFile, $"File {Data.fileName} was not downloaded");
                Logger.Info("TC03 Executed Succcessfully");
            }
            catch(Exception ex) 
            {
                Logger.Error("TC03 Failed!", ex);
                throw;
            }
            finally 
            {
                aboutWorkflow.CloseOpenedPdf();
                aboutWorkflow.DeleteDownloadedFile(Data.downloadPath,Data.fileName);
            }         

        }

        [Test]
        public void Test_04() 
        {
            try 
            {
                var insightsWorkflow = new InsightsWorkflow(driver);
                var title = insightsWorkflow.GetArticleTitle();
                var actualTitle = insightsWorkflow.GetActualArticleTitle();
                Assert.That(actualTitle, Is.EqualTo(title), $"Expected '{title}', but found '{actualTitle}'");
                Logger.Info("TC04 Executed Succcessfully");
            }
            catch(Exception ex) 
            {
                Logger.Error("TC04 Failed!", ex);
                throw;
            }

        }

    }
}
