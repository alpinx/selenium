using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumTests.Lesson_11
{
    class Application
    {
        public static IWebDriver Driver;

        public Application()
        {
            Driver = Driver_Chrome.GetInstance_Chrome;
        }
        public MainPage OpenMainPage()
        {
            Driver.Navigate().GoToUrl("http://localhost:8080/litecart/en/");
            return new MainPage();
        }
    }
}
