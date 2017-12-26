using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson8
{

    [TestClass]
    public class Exercise14
    {

        public string URL_Exercise_14 = "http://localhost:8080/litecart/admin/?app=countries&doc=countries/";
        public string URL_Exercise_14_admin = "http://localhost:8080/litecart/admin";
        public void EnterAdminPanel()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl(URL_Exercise_14_admin);
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
        public void Exercise_14()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl(URL_Exercise_14);
            try
            {
                var editCountry = driver.FindElement(By.XPath(".//a[@title='Edit']"));
                editCountry.Click();
                var linkOutside = driver.FindElement(By.XPath(".//strong[text()='Code']/../a"));
               


                string mainWindow = driver.CurrentWindowHandle;
                ICollection<string> oldWindows = driver.WindowHandles;
                linkOutside.Click(); // открывает новое окно
                                     // ожидание появления нового окна,
                                     // идентификатор которого отсутствует в списке oldWindows,
                                     // остаётся в качестве самостоятельного упражнения
                new WebDriverWait(driver,TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(By.XPath(".//a[@class='mw-wiki-logo']")));
               // string newWindow =WebDriverWait.Until(ThereIsWindowOtherThan(oldWindows));
               // driver.SwitchTo().Window(newWindow);

                // ...
                driver.Close();
                driver.SwitchTo().Window(mainWindow);




            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            Driver_Chrome.GetInstance_Chrome.Quit();
        }

        public string GiveUniqEmail()
        {
            return DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "") + DateTime.Now.Millisecond + "@gmail.com";
        }


    }
}
