using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fiddler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using Proxy = OpenQA.Selenium.Proxy;


namespace SeleniumTests.Lesson_10
{
    [TestClass]
    public class Exercise18
    {
        [TestMethod]
        public void test()
        {
            var seleniumProxy = new Proxy { HttpProxy = "localhost:8877", SslProxy = "localhost:8877" };
            var option = new FirefoxOptions {Proxy = seleniumProxy};
            var slenium = new FirefoxDriver(option);

            const FiddlerCoreStartupFlags fiddlerStartUpFlags = FiddlerCoreStartupFlags.DecryptSSL & FiddlerCoreStartupFlags.AllowRemoteClients & FiddlerCoreStartupFlags.CaptureFTP & FiddlerCoreStartupFlags.ChainToUpstreamGateway & FiddlerCoreStartupFlags.MonitorAllConnections & FiddlerCoreStartupFlags.CaptureLocalhostTraffic;
            FiddlerApplication.Prefs.SetStringPref("fiddler.config.path.makecert", @"d:\..\Makecert.exe");//To define the MakeCert.exe path manually.
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);//Abort session when client abort
            FiddlerApplication.Startup(8877, fiddlerStartUpFlags);
            FiddlerApplication.AfterSessionComplete += delegate (Session targetSession)
            {
                Console.WriteLine("{0}\t{1}",targetSession.responseCode,targetSession.fullUrl);
            };
            slenium.Navigate().GoToUrl("http://google.com");
            FiddlerApplication.oProxy.PurgeServerPipePool();//Run this between tests to make sure the new test will start "clean"
            slenium.Quit();
        }
    }
}
