using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace SeleniumTests
{
    public sealed class Driver_Chrome
    {
        private static readonly IWebDriver Instance_Chrome = new ChromeDriver();
        private Driver_Chrome()
        {}
        private static readonly Lazy<IWebDriver> lazy_Chrome = new Lazy<IWebDriver>(() => Instance_Chrome);
        public static IWebDriver GetInstance_Chrome { get { return lazy_Chrome.Value; } }
        public static bool IsRunning => Instance_Chrome==null;
    }
    public sealed class Driver_Firefox
    {
        private static readonly IWebDriver Instance_Firefox = new FirefoxDriver();
        private Driver_Firefox()
        {}
        private static readonly Lazy<IWebDriver> lazy_Firefox = new Lazy<IWebDriver>(() => Instance_Firefox);
        public static IWebDriver GetInstance_Firefox { get { return lazy_Firefox.Value; } }
        public static bool IsRunning => Instance_Firefox == null;
    }
    public sealed class Driver_IE
    {
        private static readonly IWebDriver Instance_IE = new InternetExplorerDriver();
        private Driver_IE()
        {}
        private static readonly Lazy<IWebDriver> lazy_IE = new Lazy<IWebDriver>(() => Instance_IE);
        public static IWebDriver GetInstance_IE { get { return lazy_IE.Value; } }
        public static bool IsRunning => ((RemoteWebDriver)Instance_IE).SessionId != null;
    }
}
