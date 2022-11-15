using AutoTest.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using OpenQA.Selenium.Support.UI;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace AutoTest.WebAction
{
    [Serializable]
    public class WebAction : ActionBase
    {
        internal const string Event = "Event";

        private static IWebDriver _CurrentWebDriver;
        public static IWebDriver CurrentWebDriver
        {
            get
            {
                if (_CurrentWebDriver == null)
                    throw new Exception(string.Format("Browser has not started"));
                return _CurrentWebDriver;
            }
            set
            {
                _CurrentWebDriver = value;
            }
        }
        public static TimeSpan TimeOut = new TimeSpan(0, 1, 0);

        public override string Name
        {
            get { return "WebUI Action"; }
        }

        private static WebActionUI _UI;
        public override UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new WebActionUI();
                _UI.Bind(this);
                return _UI;
            }
        }

        internal SimpleActionState _State = new SimpleActionState();
        public override IActionState State
        {
            get { return _State; }
        }

        public override void Execute(IStepInstance step)
        {
            string stEvent = _State[Event];
            step.Log(stEvent != null ? string.Format("Action: {0}", stEvent) : "Action is empty");
            if (string.IsNullOrWhiteSpace(stEvent))
                return;

            IMemory memory = step.StepData;

            EventType e = (EventType)Enum.Parse(typeof(EventType), stEvent);
            step.Log("Executing: ", e, "...");

            switch (e)
            {
                case EventType.LaunchApp:
                    GeneralHandler.LauchApp(step, memory);
                    break;
                case EventType.GetPageTitle:
                    step.Result = GeneralHandler.GetTitle();
                    break;
                case EventType.Navigate:
                    GeneralHandler.Navigate(memory);
                    break;
                case EventType.CloseWindow:
                    GeneralHandler.CloseWindow();
                    break;
                case EventType.GetElement:
                    step.Result = GeneralHandler.GetElement(step);
                    break;
                case EventType.SwitchToDefaultContent:
                    KeyboardHandler.SwitchToDefaultContent();
                    break;
                default:
                    IWebElement element = GeneralHandler.FindElement(step);
                    switch (e)
                    {
                        case EventType.SwitchTo:
                            KeyboardHandler.SwitchTo(element);
                            break;
                        case EventType.GetText:
                            step.Result = GeneralHandler.GetText(element);
                            break;
                        case EventType.Click:
                            MouseHandler.Click(element, memory);
                            break;
                        case EventType.DoubleClick:
                            MouseHandler.DoubleClick(element);
                            break;
                        case EventType.Dragdrop:
                            MouseHandler.DragAndDrop(element, memory);
                            break;
                        case EventType.MouseOver:
                            MouseHandler.MouseOver(element);
                            break;
                        case EventType.TypeText:
                            KeyboardHandler.TypeText(element, memory);
                            break;
                        case EventType.SystemKeyDown:
                            KeyboardHandler.SystemKeyDown(element, memory);
                            break;
                        case EventType.SetText:
                            GeneralHandler.SetText(element, memory);
                            break;
                        case EventType.SetDropDownItem:
                            GeneralHandler.SetDropDownItem(element, memory);
                            break;
                        case EventType.Confirm:
                            GeneralHandler.Confirm(element);
                            break;
                        case EventType.SetCheckBoxValue:
                            GeneralHandler.SetCheckBoxValue(element, memory);
                            break;
                        case EventType.JavaScript:
                            GeneralHandler.JavaScript(element, memory);
                            break;
                    }
                    break;
            }

            step.Log("Executed");
        }

        public override ActionBase Clone()
        {
            WebAction res = new WebAction();
            res._State = _State.Clone() as SimpleActionState;
            return res;
        }

        public override XElement SaveXml()
        {
            XElement xAction = new XElement("Action", new XAttribute("Type", typeof(WebAction).FullName)
                , new XElement("Name", this.Name));
            XElement xState = new XElement("States");
            xAction.Add(xState);

            foreach (var e in this.State.GetStepParas())
                xState.Add(new XElement("State",
                    new XAttribute("Key", e.Key),
                    new XAttribute("Value", e.Value)));

            return xAction;
        }

        public override void LoadXml(XElement element)
        {
            this.State.LoadXml(element);

            //foreach (var state in element.Element("States").Elements("State"))
            //    this.State.[state.Attribute("Key").Value] = state.Attribute("Value").Value;
        }
    }
}
