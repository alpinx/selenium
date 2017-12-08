using System;
using System.Collections.Generic;
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
        [TestMethod]
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
                var Menu = driver.FindElement(By.Id("box-apps-menu"));
                var menuItems = driver.FindElements(By.XPath("//*[@id='box-apps-menu']/li"));

                //TODO: пока не сделано, скоро будет
             
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
