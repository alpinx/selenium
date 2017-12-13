using System;
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
    public class Price
    {
        public long Amount { get; set; }
        public List<int> Color;
        public int Size { get; set; }
        public string Style { get; set; }
    }

    public class Duck
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public Price Price { get; set; }
        public Price SalePrice { get; set; }
    }

    [TestClass]
    public class Exercise10
    {

        public string URL_Exercise_10 = "http://localhost:8080/litecart/";
        public string URL_Exercise_10_2 = "http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones";

        [TestInitialize]
        public void SetupTest()
        {
            Driver.GetInstance.Navigate().GoToUrl(URL_Exercise_10);
        }
        [TestMethod]
        public void EnterAdminPanel()
        {
            var driver = Driver.GetInstance;
            try
            {
                var listofProducts = new List<Duck>();
                var productPages = driver.FindElements(By.XPath(".//li[contains(@class,'product column')]"));
                foreach (var product in productPages)
                {
                    var url = product.FindElement(By.XPath(".//a"));
                    var name = product.FindElement(By.XPath(".//div[@class='name']"));
                    var manufacturer = product.FindElement(By.XPath(".//div[@class='manufacturer']"));
                    var price = product.FindElements(By.XPath(".//span[contains(@class,'price')]")).Count == 0 ?
                        product.FindElement(By.XPath(".//s[contains(@class,'price')]")) :
                        product.FindElement(By.XPath(".//span[contains(@class,'price')]"));
                    var saleprice = product.FindElements(By.XPath(".//strong[contains(@class,'price')]")).Count == 0 ?
                        null :
                        product.FindElement(By.XPath(".//strong[contains(@class,'price')]"));

                    listofProducts.Add(new Duck()
                    {
                        URL = url.GetAttribute("href"),
                        Name = name.Text,
                        Manufacturer = manufacturer.Text,
                        Price = new Price()
                        {
                            Amount = Int64.Parse(price.Text.Replace("$", "")),
                            Color = price.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToArray().Select(x => Int32.Parse(x)).ToList(),
                            Size = Int32.Parse(price.GetCssValue("font-size").Remove(2)),
                            Style = price.TagName
                        },
                        SalePrice = saleprice == null ? null :
                        new Price()
                        {
                            Amount = Int64.Parse(saleprice.Text.Replace("$", "")),
                            Color = saleprice.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToArray().Select(x => Int32.Parse(x)).ToList(),
                            Size = Int32.Parse(saleprice.GetCssValue("font-size").Remove(2)),
                            Style = saleprice.TagName
                        }
                    });

                   
                   
                    Assert.IsTrue(listofProducts.Last().Price.Color[0] == listofProducts.Last().Price.Color[1]
                         && listofProducts.Last().Price.Color[1] == listofProducts.Last().Price.Color[2],
                         "Wrong price color");
                    if (listofProducts.Last().SalePrice != null)
                    {
                        Assert.IsTrue(listofProducts.Last().Price.Amount > listofProducts.Last().SalePrice.Amount);
                        Assert.IsTrue(listofProducts.Last().Price.Size < listofProducts.Last().SalePrice.Size);
                        Assert.IsTrue(listofProducts.Last().SalePrice.Color[0] > 100 && 
                            listofProducts.Last().SalePrice.Color[1] == listofProducts.Last().SalePrice.Color[2] && 
                            listofProducts.Last().SalePrice.Color[1] == 0,
                            "Wrong sale price color");
                    }
                }
                foreach (var productOnMain in listofProducts)
                {
                    driver.Navigate().GoToUrl(productOnMain.URL);
                    var name = driver.FindElement(By.XPath(".//h1"));
                    var manufacturer = driver.FindElement(By.XPath(".//div[@class='manufacturer']//img"));
                    var price = driver.FindElements(By.XPath(".//div[@class='information']//span[contains(@class,'price')]")).Count == 0 ?
                        driver.FindElement(By.XPath(".//div[@class='information']//s[contains(@class,'price')]")) :
                        driver.FindElement(By.XPath(".//div[@class='information']//span[contains(@class,'price')]"));
                    var saleprice = driver.FindElements(By.XPath(".//div[@class='information']//strong[contains(@class,'price')]")).Count == 0 ?
                            null :
                            driver.FindElement(By.XPath(".//div[@class='information']//strong[contains(@class,'price')]"));

                    Assert.IsTrue(name.Text == productOnMain.Name, "Wrong  name!");
                    Assert.IsTrue(manufacturer.GetAttribute("Title") == productOnMain.Manufacturer, "Wrong manufacturer");

                    Assert.IsTrue(Int64.Parse(price.Text.Replace("$", "")) == productOnMain.Price.Amount, "Wrong price");
                    Assert.IsTrue(price.TagName == productOnMain.Price.Style, "Wrong price text style");
                    var color = price.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToList()
                        .Select(x => Int32.Parse(x)).ToList();
                    Assert.IsTrue(color[0] == color[1] && color[1] == color[2], "Wrong price color");

                    if (saleprice != null && productOnMain.SalePrice != null)
                    {
                        Assert.IsTrue(Int64.Parse(saleprice.Text.Replace("$", "")) == productOnMain.SalePrice.Amount);
                        Assert.IsTrue(saleprice.TagName == productOnMain.SalePrice.Style, "Wrong price text style");
                        var salecolor = saleprice.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToList()
                            .Select(x => Int32.Parse(x)).ToList();
                        Assert.IsTrue(salecolor[0] > 100 && salecolor[1] == salecolor[2] && salecolor[1] == 0, "Wrong sale price color");

                        Assert.IsTrue(Int32.Parse(saleprice.GetCssValue("font-size").Remove(2)) > Int32.Parse(price.GetCssValue("font-size").Remove(2)), "Wrong sale price size");
                        Assert.IsTrue(price.TagName == "s", "Wrong regular price text style");
                        Assert.IsTrue(Int64.Parse(price.Text.Replace("$", "")) > Int64.Parse(saleprice.Text.Replace("$", "")));
                    }
                }
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
