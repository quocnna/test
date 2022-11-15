using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public delegate void VariableNameChangedHandler(string oldValue, string newValue);

    [Serializable]
    public class Variable : INotifyPropertyChanged, IPersistent
    {
        public event VariableNameChangedHandler OnVariableNameChanged;

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                string oldVal = _Name;
                _Name = String.IsNullOrWhiteSpace(value) ? _Name : value;

                if (!string.IsNullOrWhiteSpace(oldVal) && oldVal != value)
                {
                    if (OnVariableNameChanged != null)
                        OnVariableNameChanged(oldVal, value);
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private object _Value;
        public object Value {
            get { return _Value; }
            set
            {
                if (!object.Equals(_Value, value))
                {
                    _Value = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        public bool IsPublic { get; set; }
        public string Description { get; set; }

        public Variable Clone()
        {
            return new Variable() { _Name = this.Name, Value = this.Value, IsPublic = this.IsPublic, Description = this.Description };
        }

        XElement IPersistent.SaveXml()
        {
            XElement xVairable = new XElement("Variable",
                new XAttribute("Name", this.Name ?? string.Empty),
                new XAttribute("Value", this.Value ?? string.Empty),
                new XAttribute("IsPublic", this.IsPublic),
                new XAttribute("Description", this.Description ?? string.Empty));
            return xVairable;
        }

        void IPersistent.LoadXml(XElement node)
        {
            Name = XmlUtility.GetXmlValue(node.Attribute("Name"), string.Empty);
            Value = XmlUtility.GetXmlValue(node.Attribute("Value"), string.Empty);
            IsPublic = XmlUtility.GetXmlValue(node.Attribute("IsPublic"), false);
            Description = XmlUtility.GetXmlValue(node.Attribute("Description"), string.Empty);
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
