using AutoTest.Data;
using AutoTest.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace AutoTest.UserControls
{
    [Serializable]
    internal class CallTestActionState : IActionState
    {
        private string _CallTestCaseId;
        public string CallTestCaseId
        {
            get { return _CallTestCaseId; }
            set { _CallTestCaseId = value; }
        }
        public List<Variable> Variables = new List<Variable>();

        public Dictionary<string, Value> GetStepParas()
        {
            Dictionary<string, Value> res = new Dictionary<string, Value>();
            if (Variables != null)
                foreach (Variable e in Variables)
                    res[e.Name] = new Value(e.Value);

            return res;
        }

        public IActionState Clone()
        {
            CallTestActionState res = new CallTestActionState();
            res.CallTestCaseId = CallTestCaseId;
            if (Variables != null)
            {
                res.Variables = new List<Variable>();
                foreach (Variable e in Variables)
                    res.Variables.Add(e.Clone());
            }
            return res;
        }

        public void Clear()
        {
            CallTestCaseId = null;
            Variables.Clear();
        }

        public XElement SaveXml()
        {
            XElement xCallTestAction = new XElement("Action", new XAttribute("Type", typeof(CallTestAction).FullName));
            xCallTestAction.Add(new XElement("TestCaseId", this.CallTestCaseId));
            XElement xState = new XElement("States");
            xCallTestAction.Add(xState);
            foreach (Variable e in Variables)
                xState.Add((e as IPersistent).SaveXml());
            return xCallTestAction;
        }

        public void LoadXml(XElement parentNode)
        {
            var states = parentNode.Element("States");
            if (states != null)
            {
                this.CallTestCaseId = XmlUtility.GetXmlValue(parentNode.Element("TestCaseId"), string.Empty);
                foreach (var e in states.Elements("Variable"))
                {
                    Variable res = new Variable();
                    (res as IPersistent).LoadXml(e);
                    this.Variables.Add(res);
                }
            }
        }

        public void OnVariableNameChanged(string oldValue, string newValue)
        {
            //Regex reg = new Regex("(?<={+)" + oldValue + "(?=}+)");

            //foreach (Variable v in Variables)
            //{
            //    if (string.Equals(oldValue, v.Name, StringComparison.OrdinalIgnoreCase))
            //        v.Name = newValue;
            //    if (v.Value is string || v.Value is Value)
            //        v.Value = reg.Replace((string)v.Value, newValue);
            //}
        }
    }

    [Serializable]
    public sealed class CallTestAction : ActionBase, ICallTestAction
    {
        public override string Name
        {
            get { return "Call Test Case"; }
        }

        private static CallTestActionUI _UI;
        public override UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new CallTestActionUI();
                _UI.Bind(this);
                return _UI;
            }
        }

        public string CallTestCaseId
        {
            get { return _State.CallTestCaseId; }
            set { _State.CallTestCaseId = value; }
        }
        public override void Execute(IStepInstance step)
        {
            throw new NotImplementedException();
        }

        private CallTestActionState _State = new CallTestActionState();
        public override IActionState State
        {
            get { return _State; }
        }

        public override ActionBase Clone()
        {
            CallTestAction res = new CallTestAction();
            res._State = (CallTestActionState)_State.Clone();
            return res;
        }
    }

    public partial class CallTestActionUI : UserControl
    {
        public CallTestActionUI()
        {
            InitializeComponent();

            comboFunctions.ItemsSource = TestModel.AllFunctions;

            comboFunctions.SelectionChanged += comboFunctionName_SelectionChanged;

            imgGoto.MouseDown += imgGoto_MouseDown;
        }

        #region Fields

        private bool _IsBinding;
        private CallTestActionState _State;

        #endregion

        #region Events

        void comboFunctionName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (_IsBinding)
                return;

            TestCase tc = comboFunctions.SelectedItem as TestCase;
            if (tc != null)
            {
                List<Variable> oldVariables = new List<Variable>(_State.Variables.ToArray());
                _State.Clear();
                _State.CallTestCaseId = tc.Id;
                foreach (Variable v in tc.Data.Where(ele => ele.IsPublic))
                {
                    Variable clone = v.Clone();
                    clone.Value = oldVariables.Select(ele => ele.Value).FirstOrDefault();
                    _State.Variables.Add(clone);
                }
            }

            ucParas.ItemsSource = null;
            ucParas.ItemsSource = _State.Variables;
        }
        void imgGoto_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TestCase tc = comboFunctions.SelectedItem as TestCase;
            if (tc != null)
            {
                TestCase temp = tc;
                while (temp != null)
                {
                    temp.IsExpanded = true;
                    temp = temp.Parent;
                }
                tc.IsSelected = true;
            }
        }

        #endregion

        #region Methods

        internal void Bind(CallTestAction instance)
        {
            _State = (CallTestActionState)instance.State;

            _IsBinding = true;
            comboFunctions.SelectedValue = _State.CallTestCaseId;
            _IsBinding = false;
            ucParas.ItemsSource = null;

            if (_State.CallTestCaseId == null)
                return;

            TestCase tc = TestModel.AllFunctions.FirstOrDefault(e => e.Id == _State.CallTestCaseId);
            if (tc == null)
                return;

            Dictionary<string, object> old = new Dictionary<string, object>();
            foreach (Variable v in _State.Variables)
                old[v.Name] = v.Value;

            _State.Variables.Clear();
            foreach (Variable v in tc.Data.Where(e => e.IsPublic))
            {
                Variable newV = v.Clone();
                newV.Value = old.ContainsKey(v.Name) ? old[v.Name] : v.Value;
                _State.Variables.Add(newV);
            }
            ucParas.ItemsSource = _State.Variables;
        }

        #endregion
    }
}
