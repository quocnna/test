using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AutoTest.ExternalFunction
{
    [Serializable]
    internal class ExternalFunctionState : IActionState
    {
        public string AssemblyName;
        public string ClassName;
        public string FunctionName;
        internal bool IsUnitTest;

        public List<Variable> Variables;

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
            ExternalFunctionState res = new ExternalFunctionState();
            res.AssemblyName = this.AssemblyName;
            res.ClassName = this.ClassName;
            res.FunctionName = this.FunctionName;
            if (Variables != null)
            {
                res.Variables = new List<Variable>();
                foreach (Variable e in Variables)
                    res.Variables.Add(e.Clone());
            }
            return res;
        }

        public XElement SaveXml()
        {
            XElement res = new XElement("Action", new XAttribute("Type", typeof(ExternalFunction).FullName));
            XElement xState = new XElement("States");
            res.Add(xState);

            xState.Add(new XElement("Assembly", this.AssemblyName));
            xState.Add(new XElement("Class", this.ClassName));
            xState.Add(new XElement("Function", this.FunctionName));
            xState.Add(new XElement("IsUT", this.IsUnitTest));

            if (Variables != null)
            {
                XElement xVars = new XElement("Variables");
                xState.Add(xVars);
                foreach (Variable e in Variables)
                    xVars.Add((e as IPersistent).SaveXml());
            }

            return res;
        }

        public void LoadXml(XElement parentNode)
        {
            var states = parentNode.Element("States");
            if (states != null)
            {
                this.AssemblyName = XmlUtility.GetXmlValue(states.Element("Assembly"), string.Empty);
                this.ClassName = XmlUtility.GetXmlValue(states.Element("Class"), string.Empty);
                this.FunctionName = XmlUtility.GetXmlValue(states.Element("Function"), string.Empty);
                this.IsUnitTest = XmlUtility.GetXmlValue<bool, XElement>(states.Element("IsUT"), false);
                XElement xVars = states.Element("Variables");
                if (xVars != null)
                {
                    this.Variables = new List<Variable>();
                    foreach (var e in xVars.Elements())
                    {
                        Variable res = new Variable();
                        this.Variables.Add(res);
                        (res as IPersistent).LoadXml(e);
                    }
                }
            }
        }

        public void OnVariableNameChanged(string oldValue, string newValue)
        {
            if (Variables != null)
            {
                Regex reg = new Regex("(?<={+)" + oldValue + "(?=}+)");

                foreach (Variable v in Variables)
                    if (v.Value is string || v.Value is Value)
                        v.Value = reg.Replace((string)v.Value, newValue);
            }
        }
    }

    [Serializable]
    public class ExternalFunction : ActionBase
    {
        private const string Folder = "ExternalFunctions";
        private static Dictionary<string, AssemblyWarp> __AssembliesLib;
        internal static Dictionary<string, AssemblyWarp> AssembliesLib
        {
            get
            {
                if (__AssembliesLib == null)
                    __AssembliesLib = getAssemblies<ExternalLibAttribute>();

                return __AssembliesLib;
            }
        }

        private static Dictionary<string, AssemblyWarp> __AssembliesUnitTest;
        internal static Dictionary<string, AssemblyWarp> AssembliesUnitTest
        {
            get
            {
                if (__AssembliesUnitTest == null)
                    __AssembliesUnitTest = getAssemblies<UnitTestAttribute>();

                return __AssembliesUnitTest;
            }
        }


        private static IStepInstance _CurrentStep;
        public static IStepInstance CurrentStep
        {
            get
            {
                return _CurrentStep;
            }
        }

        public override string Name
        {
            get { return "External Function"; }
        }

        private static ExternalFunctionUI _UI;
        public override UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new ExternalFunctionUI();
                _UI.Bind(this);
                return _UI;
            }
        }

        public override void Execute(IStepInstance step)
        {
            try
            {
                _CurrentStep = step;
                Dictionary<string, AssemblyWarp> assemblies = _State.IsUnitTest ? AssembliesUnitTest : AssembliesLib;
                AssemblyWarp assWarp = assemblies.ContainsKey(_State.AssemblyName) ? assemblies[_State.AssemblyName] : null;
                if (assWarp == null)
                    throw new Exception(string.Format("Assembly [{0}] was not loaded", _State.AssemblyName));

                ClassWarp type = assWarp.Classes.ContainsKey(_State.ClassName) ? assWarp.Classes[_State.ClassName] : null;
                if (type == null)
                    throw new Exception(string.Format("Can not find class [{0}] in assembly [{1}]", _State.ClassName, _State.AssemblyName));

                FunctionWarp func = type.Functions.ContainsKey(_State.FunctionName) ? type.Functions[_State.FunctionName] : null;
                if (func == null)
                    throw new Exception(string.Format("Can not find function [{0}] in class [{1}]", _State.FunctionName, _State.ClassName));

                List<Value> paras = null;
                if (_State.Variables != null && _State.Variables.Count > 0)
                {
                    paras = new List<Value>();
                    foreach (var e in _State.Variables)
                        paras.Add(step.StepData[e.Name]);
                }

                object instance = Activator.CreateInstance(type.Value, null);
                step.Result = func.Value.Invoke(instance, paras == null ? null : paras.ToArray());
            }
            catch (Exception ex)
            {
                step.Fail(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private ExternalFunctionState _State = new ExternalFunctionState();
        public override IActionState State
        {
            get { return _State; }
        }

        public override ActionBase Clone()
        {
            ExternalFunction res = new ExternalFunction();
            res._State = (ExternalFunctionState)_State.Clone();
            return res;
        }

        public static List<TestCase> LoadUnitTests()
        {
            if (AssembliesUnitTest.Count == 0)
                return null;
            List<TestCase> testCases = new List<TestCase>();
            foreach (AssemblyWarp a in AssembliesUnitTest.Values)
            {
                TestCase tcAssembly = new TestCase();
                tcAssembly.Title = a.Name;
                foreach (ClassWarp c in a.Classes.Values)
                {
                    TestCase tcClass = new TestCase();
                    tcClass.Title = c.Name;
                    tcAssembly.Children.Add(tcClass);
                    foreach (FunctionWarp f in c.Functions.Values)
                    {
                        TestStep step = new TestStep();
                        step.Title = f.Name;
                        step.Action = new ExternalFunction();
                        ExternalFunctionState state = step.Action.State as ExternalFunctionState;
                        state.AssemblyName = a.Key;
                        state.ClassName = c.Key;
                        state.FunctionName = f.Key;
                        state.Variables = f.Paras.ConvertAll<Variable>(p => new Variable() { Name = p });
                        state.IsUnitTest = true;
                        tcClass.Steps.Add(step);
                    }
                }
                testCases.Add(tcAssembly);
            }
            return testCases;
        }

        private static Dictionary<string, AssemblyWarp> getAssemblies<T>()  where T : AutoTestAttribute
        {
            Dictionary<string, AssemblyWarp> res = new Dictionary<string, AssemblyWarp>();

            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + Folder);
            string dataDll = Path.GetFileName(typeof(IStepInstance).Assembly.Location);
            string externalFunctionDll = Path.GetFileName(typeof(ExternalLibAttribute).Assembly.Location);
            foreach (string path in files.Where(e => e != dataDll && e != externalFunctionDll))
            {
                string ext = Path.GetExtension(path);
                string fileName = Path.GetFileName(path);

                if (fileName == dataDll || fileName == externalFunctionDll ||
                    (!string.Equals(".exe", ext, StringComparison.OrdinalIgnoreCase) && !string.Equals(".dll", ext, StringComparison.OrdinalIgnoreCase)))
                    continue;

                Assembly assembly = Assembly.LoadFrom(path);

                foreach (Type type in assembly.GetTypes())
                {
                    T att = type.GetCustomAttribute(typeof(T)) as T ;
                    if (att != null)
                    {
                        AssemblyWarp assWarp = res.ContainsKey(assembly.FullName) ? res[assembly.FullName] : null;
                        if (assWarp == null)
                        {
                            assWarp = new AssemblyWarp(assembly);
                            res[assWarp.Key] = assWarp;
                        }

                        ClassWarp classWarp = new ClassWarp(att.FriendlyName, type);
                        assWarp.Classes[classWarp.Key] = classWarp;
                    }
                }
            }

            return res;
        }
    }
}
