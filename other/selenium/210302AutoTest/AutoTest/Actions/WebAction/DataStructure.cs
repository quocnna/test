using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.WebAction
{
    internal enum IOTypes { Mouse, Keyboard, Set, Get, Command }
    internal enum EventType
    {
        Click,
        DoubleClick,
        MouseDown,
        MouseUp,
        MouseMove,
        MouseOut,
        MouseOver,
        Dragdrop,
        KeyDown,
        KeyPress,
        KeyUp,
        SystemKeyDown,
        SystemKeyUp,
        SwitchTo,
        SwitchToDefaultContent,
        TypeText,

        SetText,
        SetCheckBoxValue,
        SetDropDownItem,
        GetText,
        GetElement,
        GetPageTitle,

        LaunchApp,
        Navigate,
        Confirm,
        CloseWindow,
        JavaScript
    }
    internal enum FindBy { Id, Name, TagName, ClassName, CssSelector, XPath, LinkText, PartialLinkText }
    internal enum SystemKey { Ctrl, Shift, Atl, Enter }
    internal class ParaInfo
    {
        private ParaInfo(string label, string toolTip, IEnumerable<object> items = null)
        {
            this.Label = label;
            this.Name = label.Replace(" ", "");
            this.ToolTip = toolTip;

            if (items != null)
            {
                Items = new List<string>();
                foreach (var e in items)
                    Items.Add(e.ToString());
            }
        }

        private ParaInfo(string label, string toolTip, Type enumType)
            : this(label, toolTip, Enum.GetNames(enumType))
        {
        }
        public readonly string Name;
        public readonly string Label;
        public readonly string ToolTip;
        public readonly List<string> Items;

        #region Data

        public static readonly ParaInfo FindBy = new ParaInfo("Find By", "There are some ways to find object, please choose one", typeof(FindBy));
        public static readonly ParaInfo ObjectId = new ParaInfo("Object Identify", "String value to find object");
        public static readonly ParaInfo Condition = new ParaInfo("Condition", "By default application search for displayed elements. Should use this condition for special cases, ex: type='hidden' style='display:block");
        public static readonly ParaInfo XCoord = new ParaInfo("X", "Click at X-Coordination is a number");
        public static readonly ParaInfo YCoord = new ParaInfo("Y", "Click at Y-Coordination is a number");
        public static readonly ParaInfo Button = new ParaInfo("Button", "Mouse button is Left or Right", new List<string> { "Left", "Right" });
        public static readonly ParaInfo StringValue = new ParaInfo("Value", "Value is a string");
        public static readonly ParaInfo ScriptValue = new ParaInfo("Script Content", "Using keywork \"me\" to access current element, ex: me.click();");
        public static readonly ParaInfo SystemKey = new ParaInfo("Key", "Special keyword", typeof(SystemKey));
        public static readonly ParaInfo Browser = new ParaInfo("Browser", "Browser Type", new List<string> { "Firefox", "IE", "Chrome", "Safari" });
        public static readonly ParaInfo Url = new ParaInfo("Url", "full url of appication");
        public static readonly ParaInfo RelativeUrl = new ParaInfo("Relative Url", "Relative url of appication");
        public static readonly ParaInfo FindTargetBy = new ParaInfo("Find Target By", "There are some ways to find object, please choose one", typeof(FindBy));
        public static readonly ParaInfo ObjectTargetId = new ParaInfo("Target Object Identify", "String value to find object");
        public static readonly ParaInfo IsChecked = new ParaInfo("Check", "Check is checked or unchecked", new List<string> { "Checked", "Un-Checked" });
        public static readonly ParaInfo TimeOut = new ParaInfo("Time Out", "the maximum time in SECOND for waiting");

        #endregion
    }

    internal class ActionInfo
    {
        private ActionInfo(IOTypes io, EventType ev, params ParaInfo[] paras)
        {
            IO = io;
            Event = ev;
            if (paras != null)
            {
                Paras = new List<ParaInfo>();
                foreach (ParaInfo e in paras)
                    Paras.Add(e);
            }
        }
        public IOTypes IO { get; private set; }
        public EventType Event { get; private set; }
        public readonly List<ParaInfo> Paras;

        public override string ToString()
        {
            return Event.ToString();
        }

        #region Data

        public static List<ActionInfo> AllActions = new List<ActionInfo>
        {
            new ActionInfo(IOTypes.Mouse, EventType.Click, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.XCoord, ParaInfo.YCoord ),
            new ActionInfo(IOTypes.Mouse, EventType.DoubleClick, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.XCoord, ParaInfo.YCoord ),
            new ActionInfo(IOTypes.Mouse, EventType.MouseDown, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.Button, ParaInfo.XCoord, ParaInfo.YCoord ),
            new ActionInfo(IOTypes.Mouse, EventType.MouseUp, ParaInfo.FindBy,ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.Button, ParaInfo.XCoord, ParaInfo.YCoord ),
            new ActionInfo(IOTypes.Mouse, EventType.MouseMove, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.XCoord, ParaInfo.YCoord ),
            new ActionInfo(IOTypes.Mouse, EventType.MouseOut, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition),
            new ActionInfo(IOTypes.Mouse, EventType.MouseOver, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition),
            new ActionInfo(IOTypes.Mouse, EventType.Dragdrop, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.FindTargetBy, ParaInfo.ObjectTargetId ),

            new ActionInfo(IOTypes.Keyboard, EventType.SystemKeyDown, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.SystemKey ),
            new ActionInfo(IOTypes.Keyboard, EventType.SystemKeyUp, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.SystemKey ),
            new ActionInfo(IOTypes.Keyboard, EventType.TypeText, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.StringValue ),
            new ActionInfo(IOTypes.Keyboard, EventType.SwitchTo, ParaInfo.FindBy, ParaInfo.ObjectId),
            new ActionInfo(IOTypes.Keyboard, EventType.SwitchToDefaultContent),

            new ActionInfo(IOTypes.Set, EventType.SetText, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.StringValue),
            new ActionInfo(IOTypes.Set, EventType.SetDropDownItem, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.StringValue ),
            new ActionInfo(IOTypes.Set, EventType.SetCheckBoxValue, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.IsChecked ),

            new ActionInfo(IOTypes.Get, EventType.GetText, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition ),
            new ActionInfo(IOTypes.Get, EventType.GetElement, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.TimeOut),
            new ActionInfo(IOTypes.Get, EventType.GetPageTitle),

            new ActionInfo(IOTypes.Command, EventType.LaunchApp, ParaInfo.Browser, ParaInfo.Url ),
            new ActionInfo(IOTypes.Command, EventType.Navigate, ParaInfo.Url ),
            new ActionInfo(IOTypes.Command, EventType.Confirm, ParaInfo.FindBy, ParaInfo.ObjectId ),
            new ActionInfo(IOTypes.Command, EventType.CloseWindow),
            new ActionInfo(IOTypes.Command, EventType.JavaScript, ParaInfo.FindBy, ParaInfo.ObjectId, ParaInfo.Condition, ParaInfo.ScriptValue),
        };

        #endregion
    }
}
