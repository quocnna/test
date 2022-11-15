using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Linq;

namespace AutoTest.Data
{
    [Serializable]
    public class GlobalVariableGroup : NotifyPropertyChangedBase, IPersistent
    {
        public GlobalVariableGroup()
        {
            Children = new ObservableCollection<GlobalVariableGroup>();
            Variables = new ObservableCollection<GlobalVariable>();
        }

        #region IPersistent

        System.Xml.Linq.XElement IPersistent.SaveXml()
        {
            XElement res = new XElement("Global", new XAttribute("Title", this.Title));
            if (!string.IsNullOrWhiteSpace(this.Description))
                res.SetAttributeValue("Description", this.Description);
            if (this.Children != null && this.Children.Count > 0)
            {
                XElement xChildren = new XElement("Children");
                foreach (IPersistent e in this.Children)
                    xChildren.Add(e.SaveXml());
                res.Add(xChildren);
            }
            if (this.Variables != null && this.Variables.Count > 0)
            {
                XElement xVariable = new XElement("Variables");
                foreach (IPersistent e in this.Variables)
                    xVariable.Add(e.SaveXml());
                res.Add(xVariable);
            }
            if (this.DataTable != null)
                res.Add((this.DataTable as IPersistent).SaveXml());
            return res;
        }

        void IPersistent.LoadXml(XElement node)
        {
            _Title = XmlUtility.GetXmlValue(node.Attribute("Title"), string.Empty);
            Description = XmlUtility.GetXmlValue(node.Attribute("Description"), string.Empty);
            if (node.Elements("Children").Elements("Global").Count() > 0)
                foreach (var e in node.Elements("Children").Elements("Global"))
                {
                    GlobalVariableGroup o = new GlobalVariableGroup();
                    (o as IPersistent).LoadXml(e);
                    Children.Add(o);
                }

            if (node.Elements("Variables").Elements("Variable").Count() > 0)
                foreach (var e in node.Elements("Variables").Elements("Variable"))
                {
                    GlobalVariable o = new GlobalVariable();
                    (o as IPersistent).LoadXml(e);
                    Variables.Add(o);
                }

            if (node.Element("TableData") != null)
            {
                TableData dataTable = new TableData();
                (dataTable as IPersistent).LoadXml(node.Element("TableData"));
                this.DataTable = dataTable;
            }
        }

        #endregion

        string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    onPropertyChange("Title");
                }
            }
        }

        public string Description { get; set; }

        public GlobalVariableGroup Parent { get; set; }

        private ObservableCollection<GlobalVariableGroup> _Children;
        public ObservableCollection<GlobalVariableGroup> Children
        {
            get
            {
                return _Children;
            }
            private set
            {
                if (_Children != value)
                {
                    _Children = value;
                    if (_Children != null)
                    {
                        foreach (GlobalVariableGroup ele in _Children)
                            ele.Parent = this;
                        _Children.CollectionChanged += (sender, e) =>
                        {
                            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                            {
                                foreach (GlobalVariableGroup ele in e.NewItems)
                                    ele.Parent = this;
                            }
                            else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                            {
                                foreach (GlobalVariableGroup ele in e.OldItems)
                                    ele.Parent = null;
                            }
                        };
                    }
                }
            }
        }

        private ObservableCollection<GlobalVariable> _Variables;
        public ObservableCollection<GlobalVariable> Variables
        {
            get
            {
                return _Variables;
            }
            set
            {
                if (_Variables != value)
                {
                    _Variables = value;
                    if (_Variables != null)
                    {
                        foreach (GlobalVariable ele in _Variables)
                            ele.Parent = this;
                        _Variables.CollectionChanged += (sender, e) =>
                        {
                            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                            {
                                foreach (GlobalVariable ele in e.NewItems)
                                    ele.Parent = this;
                            }
                            else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                            {
                                foreach (GlobalVariable ele in e.OldItems)
                                    ele.Parent = null;
                            }
                        };
                    }
                }
            }
        }

        public TableData DataTable { get; set; }

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
                    if (!_IsSelected)
                        this.IsEditting = false;
                }
            }
        }

        bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;
                    onPropertyChange("IsExpanded");
                }
            }
        }

        bool _IsEditting;
        public bool IsEditting
        {
            get
            {
                return _IsEditting;
            }
            set
            {
                if (_IsEditting != value)
                {
                    _IsEditting = value;
                    onPropertyChange("IsEditting");
                }
            }
        }

        public GlobalVariableGroup Clone()
        {
            GlobalVariableGroup group = new GlobalVariableGroup();
            group.Title = this.Title;
            if (_Children != null)
            {
                foreach (var e in _Children)
                    group.Children.Add(e.Clone());
            }

            if (_Variables != null)
            {
                foreach (var e in _Variables)
                    group.Variables.Add(e.Clone());
            }

            if (this.DataTable != null)
                group.DataTable = this.DataTable.Clone();
            return group;
        }
    }

    [Serializable]
    public class GlobalVariable : NotifyPropertyChangedBase, IPersistent
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public GlobalVariableGroup Parent { get; set; }

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

        public GlobalVariable Clone()
        {
            GlobalVariable variable = new GlobalVariable();
            variable.Name = this.Name;
            variable.Value = this.Value;
            return variable;
        }

        #region IPersistent

        XElement IPersistent.SaveXml()
        {
            XElement res = new XElement("Variable", 
                new XAttribute("Name", this.Name),
                new XAttribute("Value", this.Value ?? ""));
            if (!string.IsNullOrWhiteSpace(this.Description))
                res.SetAttributeValue("Description", this.Description);
            return res;
        }

        void IPersistent.LoadXml(XElement node)
        {
            Name = XmlUtility.GetXmlValue(node.Attribute("Name"), string.Empty);
            Value = XmlUtility.GetXmlValue(node.Attribute("Value"), string.Empty);
        }

        #endregion
    }
}
