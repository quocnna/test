using AutoTest.Data;
using AutoTest.ExternalFunction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutoTest.ExternalFunctionIO
{
    [ExternalLib]
    public class Brower
    {
        [ExternalLib]
        public static void SetIEBrowserMode(Value mode)
        {
            if (TestModel.MainWindowHandle == IntPtr.Zero)
                return;

            Func<AutomationElement, System.Windows.Automation.Condition, int, AutomationElement> findElement = (e, con, waitInSeconds) =>
            {
                AutomationElement res = null;
                TimeSpan timeOut = new TimeSpan(0, 0, waitInSeconds);
                DateTime dt = DateTime.Now;
                do
                {
                    res = e.FindFirst(TreeScope.Descendants, con);
                }
                while (res == null && DateTime.Now.Subtract(dt) < timeOut);

                return res;
            };

            AutomationElement browser = AutomationElement.FromHandle(TestModel.MainWindowHandle);
            browser.SetFocus();

            Condition condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                 new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 10"),
                 Automation.ControlViewCondition);
            AutomationElement ele = findElement(browser, condition, 1);
            if (ele != null)
            {
                InvokePattern pattern = ele.GetCurrentPattern(InvokePatternIdentifiers.Pattern) as InvokePattern;
                ele.SetFocus();
                pattern.Invoke();
                Thread.Sleep(200);
            }
            else
            {
                browser.SetFocus();
                System.Windows.Forms.SendKeys.SendWait("{F12}");
                Thread.Sleep(200);

                condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                     new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 10"),
                     Automation.ControlViewCondition);
                ele = findElement(browser, condition, 2);
                InvokePattern pattern = ele.GetCurrentPattern(InvokePatternIdentifiers.Pattern) as InvokePattern;
                ele.SetFocus();
                pattern.Invoke();
                Thread.Sleep(200);
            }

            ele = findElement(AutomationElement.RootElement, condition, 2);
            ele.SetFocus();
            System.Windows.Forms.SendKeys.SendWait("{DOWN}");
            System.Windows.Forms.SendKeys.SendWait("{DOWN}");
            System.Windows.Forms.SendKeys.SendWait("{DOWN}");
            System.Windows.Forms.SendKeys.SendWait("~");
            Thread.Sleep(200);

            browser.SetFocus();
            System.Windows.Forms.SendKeys.SendWait("{F12}");
        }

        [ExternalLib]
        public static void SetFileFromOpenFileDialog(Value browser, Value path)
        {
            Func<AutomationElement, Condition, AutomationElement> findElement = (e, con) =>
            {
                AutomationElement res = null;
                TimeSpan timeOut = new TimeSpan(0, 1, 0);
                DateTime dt = DateTime.Now;
                do
                {
                    res = e.FindFirst(TreeScope.Descendants, con);
                }
                while (res == null && DateTime.Now.Subtract(dt) < timeOut);

                return res;
            };

            string dialogName = browser.ToString().ToLower();
            dialogName = dialogName == "chrome" ? "Open" :
                dialogName == "IE" ? "Choose File to Upload" : "File Upload";

            AndCondition condition = new AndCondition(
                 new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                 new PropertyCondition(AutomationElement.NameProperty, dialogName),
                 new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "Dialog"),
                 new PropertyCondition(WindowPatternIdentifiers.WindowInteractionStateProperty, WindowInteractionState.ReadyForUserInteraction),
                 Automation.ControlViewCondition);

            AutomationElement ele = findElement(AutomationElement.RootElement, condition);

            condition = new AndCondition(
                 new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                 new PropertyCondition(AutomationElement.AutomationIdProperty, "1148"),
                Automation.ControlViewCondition);

            ele = findElement(ele, condition);
            ele.SetFocus();

            Thread.Sleep(1000);

            ValuePattern pattern = ele.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            if (!File.Exists(path.ToString()))
                throw new Exception("not exist file " + path);
            pattern.SetValue(path.ToString());
            Thread.Sleep(1000);

            System.Windows.Forms.SendKeys.SendWait("~");
            Thread.Sleep(1000);
        }
    }
}
