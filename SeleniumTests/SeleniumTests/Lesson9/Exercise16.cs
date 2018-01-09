using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace SeleniumTests.Lesson9
{
    class Exercise16
    {
        [TestClass]
        public class Exercise15
        {
            //https://www.gridlastic.com/
            //alexey.pigin@firstlinesoftware.com
            //SeleniumTest1
            [TestMethod]
            public void test1()
            {
                RemoteWebDriver driver;
                DesiredCapabilities capability = DesiredCapabilities.Chrome();
                capability.SetCapability("platform", "VISTA");
                capability.SetCapability("version", "latest");
                capability.SetCapability("gridlasticUser", "2mVZ4kYxnRCmFkSCXtsmzGk8bFVhFVTx");
                capability.SetCapability("gridlasticKey", "8Em3Oar1STen4WQRf84FFfDHKpPfbyCa");
                capability.SetCapability("video", "True");
                driver = new RemoteWebDriver(
                    new Uri("http://347BR2LE.gridlastic.com:80/wd/hub"), capability, TimeSpan.FromSeconds(600));// NOTE: connection timeout of 600 seconds or more required for time to launch grid nodes if non are available.
                driver.Manage().Window.Maximize(); // If Linux set window size, max 1920x1080, like driver.Manage().Window.Size = new Size(1920, 1020);
                driver.Navigate().GoToUrl("https://www.google.com/ncr");
                IWebElement query = driver.FindElement(By.Name("q"));
                query.SendKeys("webdriver");
                query.Submit();
                Console.WriteLine("Video: " + "http://s3-eu-west-2.amazonaws.com/a1a394c8-e152-7713-35af-84bba4c6b87t/64e17e97-1b32-b7b2-6297-225cdd33590e/play.html?" + driver.SessionId);
                driver.Quit();

            }
        }
    }
}
