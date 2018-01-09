using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson10
{
    [TestClass]
    public class Exercise17
    {
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
        public void CheckCatalog()
        {
            //******************************
            /*
            чтобы проверить можно на время теста переименовать папку
            C:\xampp\htdocs\litecart\ext\trumbowyg  -> C:\xampp\htdocs\litecart\ext\trumbowyg2
            тогда будут валиться сообщения при входе на страницу продукта
            */
            //******************************
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin");
            if (driver.Url.Contains("login"))
            {
                EnterAdminPanel();
            }
            try
            {
                var logmessages = new Dictionary<string, IReadOnlyCollection<LogEntry>>();
                driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=catalog&doc=catalog&category_id=1");
                var listOfProducts = driver.FindElements(
                    By.XPath(".//table[@class='dataTable']//td/img/../a[contains(@href,'product_id')]")).Select(x => x.Text).ToList();
                foreach (var product in listOfProducts)
                {
                    driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=catalog&doc=catalog&category_id=1");
                    driver.FindElement(By.XPath(".//table[@class='dataTable']//td/img/../a[contains(text(),'" + product + "')]")).Click();
                    var logs = driver.Manage().Logs.GetLog("browser");
                    if (logs.Count > 0)
                    {
                        logmessages.Add(product, logs);
                    }
                }
                if (logmessages.Count > 0)
                {
                    foreach (var log in logmessages)
                    {
                        Console.WriteLine(log.Key);
                        foreach (var logMessage in log.Value)
                        {
                            Console.WriteLine(logMessage.Level + "\t" + logMessage.Message);
                        }
                    }
                    Assert.Fail("There are some messages in logs!");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestCleanup]
        public void Clean()
        {
            Driver_Chrome.GetInstance_Chrome.Quit();
        }
    }
}
