using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Lesson_11;

namespace SeleniumTests.Lesson7
{

    [TestClass]
    public class Exercise19
    {

        [TestMethod]
        public void Exercise_19()
        {
            var app = new Application();
            var mainPage = app.OpenMainPage();
            var cartCount = Int32.Parse(mainPage.GetCartItemsCount());
            while (cartCount < 3)
            {
                var product = mainPage.OpenProduct("random");
                product.Add2Cart();
                mainPage = app.OpenMainPage();
                cartCount++;
            }
            var cart = mainPage.OpenCart();
            cart.DeleteAllProducts();
            Assert.IsTrue(cart.isCartEmpty());
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
