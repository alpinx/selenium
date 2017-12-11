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
        private static readonly IWebDriver Instance=new ChromeDriver();

        private Driver()
        {
            
        }

        private static readonly Lazy<IWebDriver> lazy = new Lazy<IWebDriver>(() => Instance);

        public static IWebDriver GetInstance { get { return lazy.Value; } }
    }
}
