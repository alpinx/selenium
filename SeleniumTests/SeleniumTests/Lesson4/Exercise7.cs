using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson4
{
    [TestClass]
    public class Exercise7
    {
        public string URL_Exercise_7 = "http://localhost:8080/litecart/admin";

        [TestInitialize]
        public void SetupTest()
        {
            Driver.GetInstance.Navigate().GoToUrl(URL_Exercise_7);
        }
        public void EnterAdminPanel()
        {
            var driver = Driver.GetInstance;
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(ExpectedConditions.ElementExists((By.Name("login_form"))));

                IWebElement Login = driver.FindElement(By.Name("username"));
                IWebElement Password = driver.FindElement(By.Name("password"));
                IWebElement LoginButton = driver.FindElement(By.Name("login"));

                Login.SendKeys("admin");
                Password.SendKeys("admin");
                LoginButton.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(ExpectedConditions.ElementExists((By.Id("sidebar"))));

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }

        [TestMethod]
        public void CheckMenuItems()
        {
            var driver = Driver.GetInstance;
            if (driver.Url.Contains("login"))
            {
                EnterAdminPanel();
            }
            try
            {
                var ListMainItems = new List<string>();
                var ListSubItems = new List<string>();

                //нашли ссылки на главные элементы меню
                var menuItems = driver.FindElements(By.XPath(".//*[@id='box-apps-menu']/li"));
                foreach (var item in menuItems)
                {
                    ListMainItems.Add(item.FindElement(By.XPath(".//a")).GetAttribute("href"));
                }
                //пробежались по главным элементам меню проверили H1 и добавили ссылки на sub-элементы меню
                foreach (var MainItem in ListMainItems)
                {
                    driver.Navigate().GoToUrl(MainItem);
                    var H1elemennt = driver.FindElements(By.XPath(".//h1"));
                    Assert.IsTrue(H1elemennt.Count > 0 && H1elemennt[0].Text != "");

                    var subItems = driver.FindElements(By.XPath(".//li[@id='app-']/ul//a"));
                    if (subItems.Count > 0)
                    {
                        foreach (var item in subItems)
                        {
                            ListSubItems.Add(item.GetAttribute("href"));
                        }
                    }
                }
                //пробежались по sub-элементам меню
                foreach (var SubItem in ListSubItems)
                {
                    driver.Navigate().GoToUrl(SubItem);
                    var H1elemennt = driver.FindElements(By.XPath(".//h1"));
                    Assert.IsTrue(H1elemennt.Count > 0 && H1elemennt[0].Text != "");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }

        [ClassCleanup]
        public static void ShutDownTest()
        {
            Driver.GetInstance.Quit();
        }
    }
}
