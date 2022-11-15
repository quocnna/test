using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using AutoTest.Data;
using System.Threading;
using System.Collections.Specialized;
using OpenQA.Selenium.Remote;
using System.Windows.Automation;

namespace AutoTest.WebAction
{
    internal static class GeneralHandler
    {
        public static IWebElement FindElement(IStepInstance step)
        {
            IWebElement res = null;
            try
            {
                res = getElement(step, WebAction.TimeOut);
                if (res != null)
                    step.Log("Find OK");
                else
                    step.Fail("Not Exist Element");
            }
            catch (Exception ex)
            {
                step.Fail(ex.Message);
            }

            return res;
        }
        public static IWebElement GetElement(IStepInstance step)
        {
            int seconds = (int)step.StepData[ParaInfo.TimeOut.Name];
            TimeSpan timeOut = seconds > 0 ? new TimeSpan(0, 0, seconds) : WebAction.TimeOut;
            IWebElement res = null;
            try
            {
                res = getElement(step, timeOut);
                step.Log(res == null ? "Get NULL" : "Get OK");
            }
            catch (Exception)
            {
                step.Log("Get NULL");
            }

            return res;
        }
        private static IWebElement getElement(IStepInstance step, TimeSpan timeout)
        {
            IMemory data = step.StepData;
            Value method = data[ParaInfo.FindBy.Name];
            FindBy findBy = method.IsEmpty() ? FindBy.Id : (FindBy)Enum.Parse(typeof(FindBy), (string)method);

            Dictionary<string, string> attributes = attributeParser((data[ParaInfo.Condition.Name] ?? "").ToString());
            string valueToFind = (string)data[ParaInfo.ObjectId.Name];

            By by = findBy == FindBy.Id ? By.Id(valueToFind) :
                findBy == FindBy.Name ? By.Name(valueToFind) :
                findBy == FindBy.TagName ? By.TagName(valueToFind) :
                findBy == FindBy.ClassName ? By.ClassName(valueToFind) :
                findBy == FindBy.CssSelector ? By.CssSelector(valueToFind) :
                findBy == FindBy.XPath ? By.XPath(valueToFind) :
                findBy == FindBy.LinkText ? By.LinkText(valueToFind) : By.PartialLinkText(valueToFind);

            WebDriverWait wait = new WebDriverWait(WebAction.CurrentWebDriver, timeout);
            IWebElement res = null;
            wait.Until<bool>((driver) =>
            {
                res = driver.FindElement(by);
                bool match = res != null;
                if (attributes.Count == 0)
                {
                    step.Log("Displayed: ", res.Displayed);
                    match = match && res.Displayed;
                }
                else
                {
                    step.Log("Check Condition:");
                    foreach (var attr in attributes)
                        if (match)
                        {
                            if (string.Equals(attr.Key, "text", StringComparison.OrdinalIgnoreCase))
                            {
                                string value = res.Text;
                                match = match && string.Equals(value, attr.Value, StringComparison.OrdinalIgnoreCase);
                                step.Log("Text[", attr.Value, "==", value, "] =>", match);
                            }
                            else if (string.Equals(attr.Key, "style", StringComparison.OrdinalIgnoreCase))
                            {
                                Dictionary<string, string> styles = styleParser(attr.Value);
                                foreach (var style in styles)
                                {
                                    string value = res.GetCssValue(style.Key);
                                    match = match && string.Equals(value, style.Value, StringComparison.OrdinalIgnoreCase);
                                    step.Log(style.Key, "[", style.Value, "==", value, "] =>", match);
                                }
                            }
                            else
                            {
                                string value = res.GetAttribute(attr.Key);
                                match = match && string.Equals(value, attr.Value, StringComparison.OrdinalIgnoreCase);
                                step.Log(attr.Key, "[", attr.Value, "==", value, "] =>", match);
                            }
                        }
                        else
                            break;
                }

                return match;
            });

            return res;
        }
        private static Dictionary<string, string> attributeParser(string strValue)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            StringBuilder sb = new StringBuilder();
            string key = null;

            for (int i = 0; i < strValue.Length; i++)
            {
            Start:
                char ch = strValue[i];
                if (ch == ' ' || ch == '=')
                {
                    if (sb.Length == 0)
                        continue;

                    key = sb.ToString();

                    attributes[key] = null;
                    if (ch == ' ')
                    {
                        while (++i < strValue.Length)
                        {
                            ch = strValue[i];
                            if (ch == ' ')
                                continue;
                            else if (ch == '=')
                                break;
                            else //<input type='checkbox' CHECKED />
                            {
                                sb.Clear();
                                goto Start;
                            }
                        }
                    }

                    if (ch == '=')
                    {
                        while (++i < strValue.Length)
                        {
                            ch = strValue[i];
                            if (ch == ' ')
                                continue;
                            else if (ch == '\'' || ch == '"')
                            {
                                int j = i < strValue.Length - 1 ? strValue.IndexOf(ch, i + 1) : -1;
                                if (j > 0)
                                {
                                    string v = strValue.Substring(i + 1, j - i - 1).Trim();
                                    attributes[key] = v;
                                    i = j + 1;
                                    if (i >= strValue.Length)
                                        return attributes;
                                }
                                sb.Clear();
                                goto Start;
                            }
                            else //<input type=CHECKBOX/>
                            {
                                sb.Clear();
                                do
                                {
                                    ch = strValue[i];
                                    if (ch == ' ')
                                    {
                                        if (sb.Length == 0)
                                            continue;
                                        else
                                            break;
                                    }
                                    else
                                        sb.Append(ch);
                                }
                                while (++i < strValue.Length);
                                attributes[key] = sb.ToString();
                                sb.Clear();
                                goto Start;
                            }
                        }
                    }
                }
                else
                    sb.Append(ch);
            }

            return attributes;
        }
        private static Dictionary<string, string> styleParser(string strValue)
        {
            Dictionary<string, string> styles = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(strValue))
                return styles;

            StringBuilder sb = new StringBuilder();
            string key = null;

            Action CreateElement = () =>
            {
                if (key == null)
                {
                    key = sb.ToString().Trim();
                    styles[key] = null;
                }
                else
                    styles[key] = sb.ToString().Trim();
            };

            for (int i = 0; i < strValue.Length; i++)
            {
                char ch = strValue[i];
                switch (ch)
                {
                    case ':':
                        key = sb.ToString().Trim();
                        styles[key] = null;
                        sb.Clear();
                        break;
                    case ';':
                        CreateElement();
                        key = null;
                        sb.Clear();
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            if (sb.Length > 0)
                CreateElement();

            return styles;
        }

        public static void LauchApp(IStepInstance step, IMemory stepData)
        {
            string browser = (string)stepData[ParaInfo.Browser.Name];
            step.Log(ParaInfo.Browser.Name, " = ", browser);

            string url = (string)stepData[ParaInfo.Url.Name];
            step.Log(ParaInfo.Url.Name, " = ", browser);

            AutomationElementCollection collection1 = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);

            string windowName = "";
            switch (browser)
            {
                case "IE":
                    InternetExplorerOptions options = new InternetExplorerOptions();
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    WebAction.CurrentWebDriver = new InternetExplorerDriver(options);
                    windowName = "Internet Explorer";
                    break;
                case "Chrome":
                     ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    WebAction.CurrentWebDriver = new ChromeDriver(service);
                    windowName = "Chrome";
                    break;
                case "Safari":
                    WebAction.CurrentWebDriver = new SafariDriver();
                    windowName = "Safari";
                    break;
                default:
                    WebAction.CurrentWebDriver = new FirefoxDriver();
                    windowName = "Firefox";
                    break;
            }

            WebAction.CurrentWebDriver.Manage().Timeouts().ImplicitWait =TimeSpan.FromSeconds(20);
            WebAction.CurrentWebDriver.Navigate().GoToUrl(url);

            AutomationElementCollection collection2 = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);
            bool stop = false;
            AutomationElement ele = null;
            
            for (int i2 = 0; i2 < collection2.Count && !stop; i2++)
            {
                ele = collection2[i2];
                try
                {
                    if ((ele.GetCurrentPropertyValue(AutomationElementIdentifiers.NameProperty) as string).Contains(windowName))
                        for (int i1 = 0; i1 < collection1.Count && !stop; i1++)
                            stop = ele != collection1[i1];
                }
                catch { }
            }

            if (ele != null)
                TestModel.MainWindowHandle = new IntPtr(ele.Current.NativeWindowHandle);

        }

        public static string GetText(IWebElement webEle)
        {
            return webEle.Text;
        }

        public static bool AssetText(IWebElement webEle, IMemory stepData)
        {
            string expectedValue = (string)stepData[ParaInfo.StringValue.Name];
            return webEle.Text.Equals(expectedValue);
        }

        public static void SetText(IWebElement webEle, IMemory stepData)
        {
            string value = (string)stepData[ParaInfo.StringValue.Name];
            webEle.Clear();
            webEle.SendKeys(value);
        }

        public static void Navigate(IMemory stepData)
        {
            string url = (string)stepData[ParaInfo.Url.Name];
            WebAction.CurrentWebDriver.Navigate().GoToUrl(url);
        }

        public static void Close()
        {
            WebAction.CurrentWebDriver.Close();
        }

        public static void SetDropDownItem(IWebElement eleDropDown, IMemory stepData)
        {
            string value = (string)stepData[ParaInfo.StringValue.Name];
            SelectElement clickThis = new SelectElement(eleDropDown);
            clickThis.SelectByText(value);
        }

        public static void Confirm(IWebElement webEle)
        {
            webEle.Click();
            IAlert alert = WebAction.CurrentWebDriver.SwitchTo().Alert();
            alert.Accept();
            _ = new WebDriverWait(WebAction.CurrentWebDriver, TimeSpan.FromSeconds(10));
        }

        public static void Open(IStepInstance step, IMemory stepData)
        {
            string relativeUrl = (string)stepData[ParaInfo.RelativeUrl.Name] ?? "";
            step.Log(ParaInfo.RelativeUrl.Name, " = ", relativeUrl);

            string url = WebAction.CurrentWebDriver.Url;
            int i = url.IndexOf('/', 7);
            url = i < 0 ? url : url.Substring(0, i);
            string st = relativeUrl.First() == '/' ? "" : "/";
            url = url + st + relativeUrl;

            step.Log("Full Url = ", url);

            WebAction.CurrentWebDriver.Navigate().GoToUrl(url);
        }

        public static void SetCheckBoxValue(IWebElement webEle, IMemory stepData)
        {
            bool isCheck = (bool)stepData[ParaInfo.IsChecked.Name];
            if (isCheck && !webEle.Selected)
                webEle.Click();
            else if (!isCheck && webEle.Selected)
                webEle.Click();

        }

        public static void JavaScript(IWebElement webEle, IMemory stepData)
        {
            string script = "var me = arguments[0];" + (string)stepData[ParaInfo.ScriptValue.Name] + ";";
            (WebAction.CurrentWebDriver as IJavaScriptExecutor).ExecuteScript(script, webEle);
        }

        public static string GetTitle()
        {
            return WebAction.CurrentWebDriver.Title;
        }

        public static void CloseWindow()
        {
            WebAction.CurrentWebDriver.Close();
        }
    }
}
