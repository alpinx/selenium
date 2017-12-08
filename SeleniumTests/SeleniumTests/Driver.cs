using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public sealed class Driver
    {
        private static IWebDriver instance;

        private Driver()
        {
            
        }

        private static readonly Lazy<IWebDriver> lazy = new Lazy<IWebDriver>(() => new ChromeDriver());

        public static IWebDriver GetInstance { get { return lazy.Value; } }
    }
}
