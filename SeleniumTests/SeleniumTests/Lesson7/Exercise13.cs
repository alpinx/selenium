using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Lesson7
{

    [TestClass]
    public class Exercise13
    {

        public string URL_Exercise_13 = "http://localhost:8080/litecart/en/";

        [TestMethod]
        public void Exercise_13()
        {
            var driver = Driver_Chrome.GetInstance_Chrome;
            driver.Navigate().GoToUrl(URL_Exercise_13);
            try
            {
                var cartCounter = Int32.Parse(driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText"));
                while (cartCounter < 3)
                {
                    driver.Navigate().GoToUrl(URL_Exercise_13);
                    var ProductItem =
                        driver.FindElement(By.XPath(".//li[contains(@class,'product column')]//a[@class='link']"));
                    ProductItem.Click();
                    if (driver.FindElements(By.XPath(".//select[@name='options[Size]']")).Count > 0)//если есть поле "размер" которое обязательно для заполнения
                    {
                        new SelectElement(driver.FindElement(By.XPath(".//select[@name='options[Size]']"))).SelectByValue("Small");
                    }
                    var add2CartButton = driver.FindElement(By.XPath(".//button[@name='add_cart_product']"));
                    var quantityBefore = driver.FindElement(By.XPath(".//span[@class='quantity']"))
                        .GetAttribute("innerText");
                    add2CartButton.Click();
                    new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until((IWebDriver d)=>d.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText")!= quantityBefore);
                    Assert.IsTrue(Int32.Parse(driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText")) -
                                  cartCounter == 1);
                    cartCounter++;
                }

                var checkoutLink = driver.FindElement(By.XPath(".//div[@id='cart-wrapper']//a[@class='link']"));
                checkoutLink.Click();

                var checkoutListCount = driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count;
                for (; checkoutListCount > 0; checkoutListCount--)
                {
                    var removeButton = driver.FindElement(By.XPath(".//button[@name='remove_cart_item']"));
                    removeButton.Click();
                    new WebDriverWait(driver,TimeSpan.FromSeconds(10)).Until(
                        d=>d.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count < checkoutListCount);
                    Assert.IsTrue(
                        checkoutListCount - driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count == 1);

                }
                Assert.IsTrue(
                    driver.FindElements(By.XPath(".//em[text()='There are no items in your cart.']")).Count > 0);
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
