using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestClass]
    public class Lesson2
    {
        public static ChromeDriver driver;
        public string URL_Exercise_1 = "http://yandex.ru";
        public string URL_Exercise_3 = "http://localhost:8080/litecart/admin";

        [TestInitialize]
        public void Init()
        {
             driver = new ChromeDriver();
        }
        [TestMethod]
        public void Exercise_1()
        {
            driver.Navigate().GoToUrl(URL_Exercise_1);
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
        [TestMethod]
        public void Exercise_3()
        {
            try
            {
                driver.Navigate().GoToUrl(URL_Exercise_3);
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
                Assert.Fail(e.Message+"\n"+e.InnerException.Message);
            }
            
        }
        [TestCleanup]
        public void Clean()
        {
            driver.Quit();
        }

    }
}
