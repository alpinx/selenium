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
            Driver_Chrome.GetInstance_Chrome.Navigate().GoToUrl(URL_Exercise_7);
        }
        public void EnterAdminPanel()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
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
            var driver = Driver_Chrome.GetInstance_Chrome;
            if (driver.Url.Contains("login"))
            {
                EnterAdminPanel();
            }
            try
            {
                var ListMainItems = new List<string>();
                //нашли главные элементы меню сделали список имен
                var menuItems = driver.FindElements(By.XPath(".//*[@id='box-apps-menu']/li"));
                foreach (var item in menuItems)
                {
                    ListMainItems.Add(item.Text);
                }
                //идем по главным элементам меню проверяем H1 
                foreach (var MainItem in ListMainItems)
                {
                    var MainElementClick=driver.FindElement(By.XPath(".//*[@id='box-apps-menu']/li//span[text()='"+MainItem+"']"));
                    MainElementClick.Click();
                    var H1elementMain = driver.FindElements(By.XPath(".//h1"));
                    Assert.IsTrue(H1elementMain.Count > 0 && H1elementMain[0].Text != "");
                    //у каждого главного ищем sub-элементы
                    var subItems = driver.FindElements(By.XPath(".//li[@id='app-']/ul//a"));
                    if (subItems.Count > 0)
                    {
                        var ListSubItems = new List<string>();
                        foreach (var item in subItems)
                        {
                            ListSubItems.Add(item.Text);
                        }
                        //идем по sub-элементам меню и проверяем H1
                        foreach (var SubItem in ListSubItems)
                        {
                            var SubElementClick = driver.FindElement(By.XPath(".//li[@id='app-']/ul//span[text()='" + SubItem + "']"));
                            SubElementClick.Click();
                            var H1elementSub = driver.FindElements(By.XPath(".//h1"));
                            Assert.IsTrue(H1elementSub.Count > 0 && H1elementSub[0].Text != "");
                        }
                    }
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
            Driver_Chrome.GetInstance_Chrome.Quit();
        }
    }
}
