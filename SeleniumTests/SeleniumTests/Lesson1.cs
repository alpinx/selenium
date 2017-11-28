using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestClass]
    public class Lesson1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://yandex.ru");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            driver.Quit();
        }
    }
}
