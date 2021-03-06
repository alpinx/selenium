﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson5
{
    [TestClass]
    public class Exercise9
    {
        public string URL_Exercise_9_admin = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
        public string URL_Exercise_9_1 = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
        public string URL_Exercise_9_2 = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones";

        [TestInitialize]
        public void SetupTest()
        {
            Driver_Chrome.GetInstance_Chrome.Navigate().GoToUrl(URL_Exercise_9_admin);
        }
        public void EnterAdminPanel()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl(URL_Exercise_9_1);
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
        public void Exercise9_1()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            if (driver.Url.Contains("login"))
            {
                EnterAdminPanel();
            }
            try
            {
                var countriesListElements = driver.FindElements(By.XPath(".//tbody//td/a[not(i)]"));
                //проверяем порядок
                Assert.IsTrue(VerifyOrder(driver, countriesListElements));
                //ищем не 0 зоны
                var zonesCounter = driver.FindElements(By.XPath(".//table[@class='dataTable']/tbody/tr/td[last()-1]"));
                var listOFnotNullZones = new Dictionary<string,string>();
                foreach (var zone in zonesCounter)
                {
                    if (zone.Text != "0")
                    {//передаем еще и количество зон на странице Countries чтобы проверить с реальным количеством 
                        listOFnotNullZones.Add(zone.FindElement(By.XPath(".//../td/a")).GetAttribute("href"), zone.Text);
                    }
                }

                foreach (var url in listOFnotNullZones.Keys)
                {
                    
                    driver.Navigate().GoToUrl(url);
                    var ListOfZones = driver.FindElements(By.XPath(
                        ".//table[@id='table-zones']//tbody//td/input[contains(@name,'name') and @value!='']"));
                    Assert.IsTrue(ListOfZones.Count == Int32.Parse(listOFnotNullZones[url]));
                    Assert.IsTrue(VerifyOrder(driver, ListOfZones));
                }

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }

        [TestMethod]
        public void Exercise9_2()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            if (driver.Url.Contains("login"))
            {
                EnterAdminPanel();
            }
            try
            {
                driver.Navigate().GoToUrl(URL_Exercise_9_2);
                var countryItems =
                    driver.FindElements(By.XPath(".//table[@class='dataTable']/tbody/tr/td/a[text()!='']"));
                var countryItemsString= new List<string>();
                foreach (var country in countryItems)
                {
                    countryItemsString.Add(country.GetAttribute("href"));
                }

                foreach (var eachCountry in countryItemsString)
                {
                    driver.Navigate().GoToUrl(eachCountry);
                    var countriesListElements = driver.FindElements(By.XPath(
                        ".//select[contains(@name,'zone_code')]/option[@selected='selected']"));
                    Assert.IsTrue(VerifyOrder(driver, countriesListElements));
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        
        }

        public bool VerifyOrder(IWebDriver driver, IReadOnlyCollection<IWebElement> elements)
        {
            var StringList = new List<string>();
            foreach (var oneElement in elements)
            {
                StringList.Add(oneElement.Text);
            }
            var expectedList = StringList.OrderBy(s => s);
            return expectedList.SequenceEqual(StringList);
        }

        [ClassCleanup]
        public static void ShutDownTest()
        {
            Driver_Chrome.GetInstance_Chrome.Quit();
        }
    }
}
