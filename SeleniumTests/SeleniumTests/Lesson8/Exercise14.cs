using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson8
{

    [TestClass]
    public class Exercise14
    {

        public string URL_Exercise_14 = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
        public string URL_Exercise_14_admin = "http://localhost:8080/litecart/admin";
        public void EnterAdminPanel()
        {
            var driver = Driver_Firefox.GetInstance_Firefox;
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
            
            var driver = Driver_Firefox.GetInstance_Firefox;
            EnterAdminPanel();
            driver.Navigate().GoToUrl(URL_Exercise_14);
            try
            {
                var editCountry = driver.FindElement(By.XPath(".//a[@title='Edit']"));
                editCountry.Click();
                var linkOutside = driver.FindElements(By.XPath(".//strong/../a/i"));
               


                string mainWindow = driver.CurrentWindowHandle;
                ICollection<string> oldWindows = driver.WindowHandles;

                foreach (var link in linkOutside)
                {
                    link.Click();
                    new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d => d.WindowHandles.Count > 1 && driver.WindowHandles.Contains(mainWindow));
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementIsVisible(By.XPath(".//body")));
                    var urlofOutsidepage = driver.Url;
                    driver.Close();
                    Assert.IsTrue(driver.WindowHandles.Count == 1);
                    driver.SwitchTo().Window(mainWindow);
                    Assert.IsTrue(driver.Url!=urlofOutsidepage);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            Driver_Firefox.GetInstance_Firefox.Quit();
        }

        public string GiveUniqEmail()
        {
            return DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "") + DateTime.Now.Millisecond + "@gmail.com";
        }


    }
}
