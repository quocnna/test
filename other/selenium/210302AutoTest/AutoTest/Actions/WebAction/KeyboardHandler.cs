using AutoTest.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
//using Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.WebAction
{
    internal static class KeyboardHandler
    {
        public static void TypeText(IWebElement webEle, IMemory stepData)
        {
            string value = (string)stepData[ParaInfo.StringValue.Name];
            foreach (char ch in value)
            {
                webEle.SendKeys(ch.ToString());
                Thread.Sleep(50);
            }
        }

        public static void SystemKeyDown(IWebElement webEle, IMemory stepData)
        {
            Value method = stepData[ParaInfo.SystemKey.Name];
             SystemKey sysKey = method.IsEmpty() ? SystemKey.Ctrl : (SystemKey)Enum.Parse(typeof(SystemKey), (string)method);

             Actions action = new Actions(WebAction.CurrentWebDriver);
             switch (sysKey)
             {
                 case SystemKey.Atl:
                     action.KeyDown(webEle, Keys.Alt);
                     break;
                 case SystemKey.Ctrl:
                     action.KeyDown(webEle, Keys.Control);
                     break;
                 case SystemKey.Shift:
                     action.KeyDown(webEle, Keys.Shift);
                     break;
                 case SystemKey.Enter:
                     webEle.SendKeys(Keys.Return);
                     break;
             }
             action.Perform();
        }

        public static void SystemKeyUp(IWebElement webEle, IMemory stepData)
        {
            Value method = stepData[ParaInfo.SystemKey.Name];
            SystemKey sysKey = method.IsEmpty() ? SystemKey.Ctrl : (SystemKey)Enum.Parse(typeof(SystemKey), (string)method);

            Actions action = new Actions(WebAction.CurrentWebDriver);
            switch (sysKey)
            {
                case SystemKey.Atl:
                    action.KeyUp(webEle, Keys.Alt);
                    break;
                case SystemKey.Ctrl:
                    action.KeyUp(webEle, Keys.Control);
                    break;
                case SystemKey.Shift:
                    action.KeyUp(webEle, Keys.Shift);
                    break;
                case SystemKey.Enter:
                    webEle.SendKeys(Keys.Return);
                    break;
            }
            action.Perform();
        }

        public static void SwitchTo(IWebElement webEle)
        {
            WebAction.CurrentWebDriver.SwitchTo().Frame(webEle);
        }

        public static void SwitchToDefaultContent()
        {
            WebAction.CurrentWebDriver.SwitchTo().DefaultContent();
        }
    }
}
