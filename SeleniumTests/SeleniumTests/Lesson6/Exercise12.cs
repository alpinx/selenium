using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson6
{

    [TestClass]
    //[DeploymentItem(@"..\Lesson6", "SQL")]
    public class Exercise12
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        public string URL_Exercise_12 = "http://localhost:8080/litecart/admin/?app=catalog&doc=catalog";
        public string URL_Exercise_12_admin = "http://localhost:8080/litecart/admin/?app=countries&doc=countries";
        public void EnterAdminPanel()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl(URL_Exercise_12_admin);
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
        public void Exercise_12()
        {
            try
            {
                var driver = Driver_Chrome.GetInstance_Chrome;
                EnterAdminPanel();
                driver.Navigate().GoToUrl(URL_Exercise_12);
                var UniqNumber = GiveUniqNum();
                var UniqName = "Another Duck №" + UniqNumber;
                var AddProduct = driver.FindElement(By.XPath(".//a[@class='button' and contains(text(),'Add New Product')]"));
                AddProduct.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//div[@class='tabs']")));
                //general
                var status = driver.FindElement(By.XPath(".//input[@name='status' and @value='1']"));
                var name = driver.FindElement(By.XPath(".//strong[text()='Name']/..//input"));
                var Code = driver.FindElement(By.XPath(".//strong[text()='Code']/..//input"));
                var Category = driver.FindElement(By.XPath(".//strong[text()='Categories']/..//input[@data-name='Root']"));
                var ProductGroupUnisex = driver.FindElement(By.XPath(".//strong[text()='Product Groups']/..//td[text()='Unisex']/..//input"));
                var Quantity = driver.FindElement(By.XPath(".//input[@name='quantity']"));
                var UploadImage = driver.FindElement(By.XPath(".//strong[text()='Upload Images']/..//input"));
                var DateValidFrom = driver.FindElement(By.XPath(".//strong[text()='Date Valid From']/..//input"));
                var DateValidTo = driver.FindElement(By.XPath(".//strong[text()='Date Valid To']/..//input"));

                if (!status.Selected) {status.Click();}
                name.SendKeys(UniqName);
                Code.SendKeys(UniqNumber);
                if (!Category.Selected) { Category.Click();}
                if (!ProductGroupUnisex.Selected) { ProductGroupUnisex.Click(); }
                Quantity.Clear();
                Quantity.SendKeys("10");
                string path = Directory.GetCurrentDirectory()+@"\Lesson6\red-duck.png";
                UploadImage.SendKeys(path);
                DateValidFrom.SendKeys("01.12.2017");
                DateValidTo.SendKeys("01.02.2018");
                //information
                var InfoTab = driver.FindElement(By.XPath(".//div[@class='tabs']//li//a[text()='Information']"));
                InfoTab.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//div[@id='tab-information' and @style='display: block;']")));
                var Manufacturer = driver.FindElement(By.XPath(".//select[@name='manufacturer_id']"));
                var Keywords = driver.FindElement(By.XPath(".//input[@name='keywords']"));
                var ShortDescription = driver.FindElement(By.XPath(".//input[@name='short_description[en]']"));
                var Description = driver.FindElement(By.XPath(".//textarea[@name='description[en]']"));
                var HeadTitle = driver.FindElement(By.XPath(".//input[@name='head_title[en]']"));

                var selectManufacturer = new SelectElement(Manufacturer);
                selectManufacturer.SelectByValue("1");
                Keywords.SendKeys("Red;Duck");
                ShortDescription.SendKeys("Another Red Duck");
                Description.SendKeys("Another Red Duck with 10$ price! BUY NOW!");
                HeadTitle.SendKeys("Red Duck");
                //price
                var PriceTab = driver.FindElement(By.XPath(".//div[@class='tabs']//li//a[text()='Prices']"));
                PriceTab.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//div[@id='tab-prices' and @style='display: block;']")));
                var Price = driver.FindElement(By.XPath(".//input[@name='purchase_price']"));
                var Money = driver.FindElement(By.XPath(".//select[@name='purchase_price_currency_code']"));
                var SaveButton = driver.FindElement(By.XPath(".//button[@name='save']"));

                Price.Clear();
                Price.SendKeys("10");
                var SelectMoney=new SelectElement(Money);
                SelectMoney.SelectByValue("USD");

                SaveButton.Click();

                new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                    ExpectedConditions.ElementExists(By.XPath(".//table[@class='dataTable']")));
                var H1elemennt = driver.FindElement(By.XPath(".//h1"));
                Assert.IsTrue(H1elemennt.Text == "Catalog");
                var CreatedProduct = driver.FindElements(By.XPath(".//table[@class='dataTable']//tr[@class='row']//a[text()='"+UniqName+"']"));
                Assert.IsTrue(CreatedProduct.Count>0);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
            }
        }
        public string GiveUniqNum()
        {
            return DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "") + DateTime.Now.Millisecond;
        }

        [TestCleanup]
        public void CleanUp()
        {
            Driver_Chrome.GetInstance_Chrome.Quit();
        }
    }
}
