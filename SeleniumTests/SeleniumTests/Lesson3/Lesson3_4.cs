using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestClass]
    public class Lesson3_4
    {
        public static ChromeDriver ChromeDriver;
        public static FirefoxDriver FirefoxDriver;
        public static InternetExplorerDriver IEDriver;
        
        public string URL_Exercise_4 = "http://localhost:8080/litecart/admin";

        public bool VerifyAdminLogin(IWebDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl(URL_Exercise_4);
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(ExpectedConditions.ElementExists((By.Name("login_form"))));

                IWebElement Login = driver.FindElement(By.Name("username"));
                IWebElement Password = driver.FindElement(By.Name("password"));
                IWebElement LoginButton = driver.FindElement(By.Name("login"));

                Login.SendKeys("admin");
                Password.SendKeys("admin");
                LoginButton.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(ExpectedConditions.ElementExists((By.Id("sidebar"))));
                return true;
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
                return false;
            }
        }

        [TestMethod]
        public void Exercise_4_Chrome()
        {
            ChromeDriver=new ChromeDriver();
            Assert.IsTrue(VerifyAdminLogin(ChromeDriver));
            ChromeDriver.Quit();
        }
        [TestMethod]
        public void Exercise_4_Firefox()
        {
            FirefoxDriver = new FirefoxDriver();
            Assert.IsTrue(VerifyAdminLogin(FirefoxDriver));
            FirefoxDriver.Quit();
        }
        [TestMethod]
        public void Exercise_4_IE()
        {
            IEDriver = new InternetExplorerDriver();
            Assert.IsTrue(VerifyAdminLogin(IEDriver));
            IEDriver.Quit();
        }

       
    }
}
