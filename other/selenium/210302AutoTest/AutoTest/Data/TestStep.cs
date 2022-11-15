using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoTest.Data
{
    [Serializable]
    public class TestStep : TestItemBase, IPersistent
    {
        #region IPersistent

        XElement IPersistent.SaveXml()
        {
            XElement xTestStep = new XElement("TestStep",
                new XAttribute("Id", this.Id),
                new XAttribute("Title", this.Title),
                new XAttribute("Status", (int)this.Status),
                new XAttribute("LogRowIndex", this.LogRowIndex),
                new XElement("BeforeRunning", this.BeforeRunning ?? string.Empty),
                new XElement("AfterRunning", this.AfterRunning ?? string.Empty),
                this.Action == null ? new XElement("Action") : this.Action.SaveXml());
            return xTestStep;
        }

        void IPersistent.LoadXml(XElement node)
        {
            if (!node.IsEmpty)
            {
                _Id = XmlUtility.GetXmlValue(node.Attribute("Id"), Guid.NewGuid().ToString());
                _Title = XmlUtility.GetXmlValue(node.Attribute("Title"), string.Empty);
                this.Status = XmlUtility.GetXmlValue(node.Attribute("Status"), TestStatus.Ready);
                this.LogRowIndex = XmlUtility.GetXmlValue(node.Attribute("LogRowIndex"), 0);
                _BeforeRunning = XmlUtility.GetXmlValue(node.Element("BeforeRunning"), string.Empty);
                _AfterRunning = XmlUtility.GetXmlValue(node.Element("AfterRunning"), string.Empty);
                XElement actionEle = node.Element("Action");
                Type type = !actionEle.HasAttributes ? null : AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes()).Where(e => e.FullName == actionEle.Attribute("Type").Value).FirstOrDefault();
                if (type != null)
                {
                    this.Action = Activator.CreateInstance(type) as ActionBase;
                    this.Action.LoadXml(actionEle);
                }
            }
            _RecordStatus = RecordStatus.Unchanged;
        }

        #endregion

        #region Properties

        private ActionBase _Action;
        public ActionBase Action
        {
            get { return _Action; }
            set
            {
                if (_Action != value)
                {
                    _Action = value;
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }

        [NonSerialized]
        private Assembly _BeforeRunningAssembly;
        private string _BeforeRunning;
        public string BeforeRunning
        {
            get { return _BeforeRunning; }
            set
            {
                string oldValue = _BeforeRunning;
                string newValue = (value ?? "").Trim();
                _BeforeRunning = newValue;
                if (!string.Equals(oldValue, newValue))
                {
                    _BeforeRunningAssembly = null;
                    onPropertyChange("BeforeRunningIndicator");
                    onPropertyChange("BeforeRunning");
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }
        public string BeforeRunningIndicator
        {
            get
            {
                return string.IsNullOrEmpty(_BeforeRunning) ? "" :
                  _BeforeRunningAssembly == null ? "..." : "Compiled";
            }
        }

        [NonSerialized]
        private Assembly _AfterRunningAssembly;
        private string _AfterRunning;
        public string AfterRunning
        {
            get { return _AfterRunning; }
            set
            {
                string oldValue = _AfterRunning;
                string newValue = (value ?? "").Trim();
                _AfterRunning = newValue;
                if (!string.Equals(oldValue, newValue))
                {
                    _AfterRunningAssembly = null;
                    onPropertyChange("AfterRunningIndicator");
                    onPropertyChange("AfterRunning");
                    RecordStatus = RecordStatus.Changed;
                }
            }
        }
        public string AfterRunningIndicator
        {
            get
            {
                return string.IsNullOrEmpty(_AfterRunning) ? "" :
                    _AfterRunningAssembly == null ? "..." : "Compiled";
            }
        }

        bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    onPropertyChange("IsSelected");
                }
            }
        }

        #endregion

        #region Methods

        public Dictionary<string, Value> GetStepParas()
        {
            Dictionary<string, Value> res = Action == null || Action.State == null ? null : Action.State.GetStepParas();
            return res ?? new Dictionary<string, Value>();
        }

        public TestStep Clone()
        {
            TestStep res = new TestStep();
            res.Title = this.Title;
            res._BeforeRunning = _BeforeRunning;
            res._AfterRunning = _AfterRunning;

            res.Action = this.Action == null ? null : this.Action.Clone();
            return res;
        }

        public void ExecuteBeforeRunning(IStepInstance stepInstance)
        {
            if (_BeforeRunningAssembly != null)
            {
                object instance = _BeforeRunningAssembly.CreateInstance("namespaceTemp.classTemp");
                instance.GetType().GetMethod("temp").Invoke(instance, new object[] { stepInstance });
            }
        }
        public void ExecuteAfterRunning(IStepInstance stepInstance)
        {
            if (_AfterRunningAssembly != null)
            {
                object instance = _AfterRunningAssembly.CreateInstance("namespaceTemp.classTemp");
                instance.GetType().GetMethod("temp").Invoke(instance, new object[] { stepInstance });
            }
        }

        public string CompileBeforeRunning()
        {
            string error;
            compile(this, true, out error, out _BeforeRunningAssembly);
            onPropertyChange("BeforeRunningIndicator");
            return error;
        }
        public string CompileAfterRunning()
        {
            string error;
            compile(this, false, out error, out _AfterRunningAssembly);
            onPropertyChange("AfterRunningIndicator");
            return error;
        }
        private static void compile(TestStep step, bool isBefore, out string error, out Assembly ass)
        {
            error = string.Empty;
            ass = isBefore ? step._BeforeRunningAssembly : step._AfterRunningAssembly;
            string text = ((isBefore ? step._BeforeRunning : step._AfterRunning) ?? "").Trim();

            if (string.IsNullOrEmpty(text.Trim()) || ass != null)
                return;

            CompilerResults res = compile(isBefore ? step.BeforeRunning : step.AfterRunning);

            if (res.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Step: {0}", step.Title);
                sb.AppendFormat("\n[{0}] has {1} errors", isBefore ? "Before Running" : "After Running", res.Errors.Count);
                sb.AppendLine();
                foreach (CompilerError err in res.Errors)
                    if (!err.IsWarning)
                        sb.AppendFormat("\nLine {0}: {1}", err.Line - 10, err.ErrorText);
                error = sb.ToString();
            }
            else
                ass = res.CompiledAssembly;
        }
        private static CompilerResults compile(string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                return new CompilerResults(new TempFileCollection());

            string code =
                "using System;" +
                "\nusing System.Collections.Generic;" +
                "\nusing System.Text;" +
                "\nusing AutoTest.Data;" +
                "\nusing AutoTest.Core;" +
                "\nnamespace namespaceTemp {" +
                    "\nclass classTemp" +
                    "\n{" +
                        "\npublic static void temp(StepInstance me)" +
                        "\n{\n" +
                            text +
                        "\n}" +
                    "\n}" +
                "\n}";

            CompilerParameters CompParameters = new CompilerParameters();
            CompParameters.GenerateInMemory = true;

            CompParameters.ReferencedAssemblies.Add("System.dll");
            CompParameters.ReferencedAssemblies.Add("AutoTest.Data.dll");
            CompParameters.ReferencedAssemblies.Add("AutoTest.Core.dll");

            CSharpCodeProvider provider = new CSharpCodeProvider();
            return provider.CompileAssemblyFromSource(CompParameters, code);
        }

        public override void OnVariableNameChanged(string oldValue, string newValue)
        {
            Regex reg = new Regex(@"(?<=(\[""{*)|(""{+))" + oldValue + @"(?=(}*""\])|(}+""))");

            if (!string.IsNullOrWhiteSpace(this.BeforeRunning))
                this.BeforeRunning = reg.Replace(this.BeforeRunning, newValue);

            if (!string.IsNullOrWhiteSpace(this.AfterRunning))
                this.AfterRunning = reg.Replace(this.AfterRunning, newValue);

            if (this.Action != null)
                this.Action.OnVariableNameChanged(oldValue, newValue);

        }

        #endregion
    }
}
