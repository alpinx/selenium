using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace SeleniumTests.Lesson9
{
    [TestClass]
    public class Exercise15
    {
        //тут не знаю как показывать что сделал)
        //использовал selenium grid extras
        //добавил папочку с настройками hub там есть конфиги нод
        //тест1 запускается на виртуалке
        //тест2 запускается на локальной ноде
        public RemoteWebDriver Driver;
        [TestMethod]
        public void test1()
        {
            Driver = new RemoteWebDriver(new Uri("http://192.168.52.51:4444/wd/hub"), new InternetExplorerOptions().ToCapabilities());
            Driver.Navigate().GoToUrl("http://ya.ru");
           
        }
        [TestMethod]
        public void test2()
        {
            Driver = new RemoteWebDriver(new Uri("http://192.168.52.51:4444/wd/hub"), new ChromeOptions().ToCapabilities());
            Driver.Navigate().GoToUrl("http://ya.ru");
        }

        [TestCleanup]
        public void clean()
        {
            Driver.Quit();
        }
    }
}
