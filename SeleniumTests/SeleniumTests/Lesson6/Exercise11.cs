using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace SeleniumTests.Lesson6
{
    
    [TestClass]
    public class Exercise11
    {

        public string URL_Exercise_11 = "http://localhost:8080/litecart/en/create_account";

        [TestMethod]
        public void Exercise_11()
        {
            try
            {
                var driver = Driver_Chrome.GetInstance_Chrome;
                driver.Navigate().GoToUrl(URL_Exercise_11);
                var First_Name = driver.FindElement(By.XPath(".//input[@name='firstname']"));
                var Last_Name = driver.FindElement(By.XPath(".//input[@name='lastname']"));
                var Address1 = driver.FindElement(By.XPath(".//input[@name='address1']"));
                var Postcode = driver.FindElement(By.XPath(".//input[@name='postcode']"));
                var City = driver.FindElement(By.XPath(".//input[@name='city']"));
                SelectElement Country =
                    new SelectElement(driver.FindElement(By.XPath(".//select[@name='country_code']")));
                SelectElement Zone_code =
                    new SelectElement(driver.FindElement(By.XPath(".//select[@name='zone_code']")));
                var Email = driver.FindElement(By.XPath(".//input[@name='email']"));
                var Phone = driver.FindElement(By.XPath(".//input[@name='phone']"));
                var Password = driver.FindElement(By.XPath(".//input[@name='password']"));
                var PasswordConfirm = driver.FindElement(By.XPath(".//input[@name='confirmed_password']"));
                var CreateButton = driver.FindElement(By.XPath(".//button[@name='create_account']"));

                var UniqEmail = GiveUniqEmail();


                First_Name.SendKeys("Name");
                Last_Name.SendKeys("Last");
                Address1.SendKeys("Address");
                Postcode.SendKeys("12345");
                City.SendKeys("City");
                Country.SelectByText("United States");
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//select[@name='zone_code' and not(@disabled)]")));
                Zone_code.SelectByText("Florida");
                Email.SendKeys(UniqEmail);
                Phone.SendKeys("+10000000000");
                Password.SendKeys("password");
                PasswordConfirm.SendKeys("password");
                CreateButton.Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success']")).Count>0);

                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//div[@id='box-account']//a[text()='Logout']")));
                Logout(driver);
                Login(driver,UniqEmail,"password");
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//div[@id='box-account']//a[text()='Logout']")));
                Logout(driver);

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
        public void Login(IWebDriver driver, string Login, string Password)
        {
            try
            {
                var Email = driver.FindElement(By.XPath(".//input[@name='email']"));
                var Pass = driver.FindElement(By.XPath(".//input[@name='password']"));
                var LoginBtn = driver.FindElement(By.XPath(".//button[@name='login']"));
                
                Email.SendKeys(Login);
                Pass.SendKeys(Password);
                LoginBtn.Click();
                Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success'and contains(text(),' You are now logged in as')]")).Count > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
                throw;
            }
        }

        public void Logout(IWebDriver driver)
        {
            try
            {
                var Logout = driver.FindElement(By.XPath(".//div[@id='box-account']//a[text()='Logout']"));
                Logout.Click();
                Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success'and text()=' You are now logged out.']")).Count > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
                throw;
            }
            
        }

        public string GiveUniqEmail()
        {
            return DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "")+DateTime.Now.Millisecond + "@gmail.com";
        }


    }
}
