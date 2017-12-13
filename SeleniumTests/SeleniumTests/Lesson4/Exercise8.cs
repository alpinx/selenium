using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson4
{
    [TestClass]
    public class Exercise8
    {
        public string URL_Exercise_8 = "http://localhost:8080/litecart/en/";

        [TestInitialize]
        public void SetupTest()
        {
            Driver_Chrome.GetInstance_Chrome.Navigate().GoToUrl(URL_Exercise_8);
        }
        
        [TestMethod]
        public void CheckMainPageStikers()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            try
            {
                var ListItemsWithoutStickers = new List<String>();
                var ProductItems = driver.FindElements(By.XPath(".//li[contains(@class,'product column')]"));
                foreach (var Item in ProductItems)
                {
                    if (Item.FindElements(By.XPath(".//div[contains(@class,'sticker')]")).Count == 0)
                    {
                        ListItemsWithoutStickers.Add(Item.FindElement(By.XPath(".//a")).GetAttribute("href"));
                    }
                }
                if (ListItemsWithoutStickers.Count > 0)
                {
                    Assert.Fail("There is no stickers on:\n" + ListItemsWithoutStickers.Aggregate((i, j) => i + "\n" + j));
                }
                
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }


        }

        [ClassCleanup]
        public static void ShutDownTest()
        {
            Driver_Chrome.GetInstance_Chrome.Quit();
        }
    }
}
