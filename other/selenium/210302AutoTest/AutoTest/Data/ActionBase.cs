using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public interface IActionState : IPersistent
    {
        void OnVariableNameChanged(string oldValue, string newValue);
        Dictionary<string, Value> GetStepParas();
        IActionState Clone();
    }

    [Serializable]
    public class SimpleActionState : IActionState
    {
        private Dictionary<string, string> _Data = new Dictionary<string, string>();

        public virtual void Clear()
        {
            _Data.Clear();
        }

        public virtual Dictionary<string, Value> GetStepParas()
        {
            Dictionary<string, Value> res = new Dictionary<string,Value>();
            foreach (var e in _Data)
                res[e.Key] = new Value(e.Value);
            return res;
        }

        public virtual XElement SaveXml()
        {
            XElement res = new XElement("States");
            foreach (var e in _Data)
                res.Add(new XElement("State",
                    new XAttribute("Key", e.Key),
                    new XAttribute("Value", e.Value)));
            return res;
        }

        public virtual void LoadXml(XElement parentNode)
        {
            this.Clear();

            foreach (var state in parentNode.Element("States").Elements("State"))
                _Data[state.Attribute("Key").Value] = state.Attribute("Value").Value;
        }

        public virtual string this[string key]
        {
            get { return _Data.ContainsKey(key) ? _Data[key] : ""; }
            set { _Data[key] = value; }
        }

        public IEnumerable<string> Keys
        {
            get { return _Data.Keys; }
        }

        public virtual void OnVariableNameChanged(string oldValue, string newValue)
        {
            foreach (string key in _Data.Keys)
                _Data[key] = replaceVariableNameChanged(_Data[key], oldValue, newValue);
        }

        protected string replaceVariableNameChanged(string text, string oldValue, string newValue)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                Regex reg = new Regex("(?<={+)" + oldValue + "(?=}+)");
                text = reg.Replace(text, newValue);
            }
            return text;
        }

        public virtual IActionState Clone()
        {
            SimpleActionState res = new SimpleActionState();
            foreach (var e in _Data)
                res[e.Key] = e.Value;
            return res;
        }


    }

    [Serializable]
    public abstract class ActionBase
    {
        public abstract string Name { get; }

        public abstract UserControl UI { get; }

        public abstract IActionState State { get; }

        public abstract void Execute(IStepInstance step);

        public abstract ActionBase Clone();
        public virtual XElement SaveXml()
        {
            return State != null ? State.SaveXml() : null;
        }
        public virtual void LoadXml(XElement node)
        {
            if (State != null)
                State.LoadXml(node);
        }

        public void OnVariableNameChanged(string oldValue, string newValue)
        {
            State.OnVariableNameChanged(oldValue, newValue);
        }

        public override string ToString()
        {
            return Name ?? this.GetType().FullName;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj) || (obj is ActionBase ? (string.Compare(this.Name, ((ActionBase)obj).Name, true) == 0) : false);
        }
        public override int GetHashCode()
        {
            return (this.Name ?? "").GetHashCode();
        }
    }
}
