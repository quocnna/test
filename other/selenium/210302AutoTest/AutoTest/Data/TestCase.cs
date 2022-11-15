using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AutoTest.Data
{
    [Serializable]
    public class TestItemCollection<T> : ObservableCollection<T> where T : TestItemBase
    {
        public TestItemCollection(TestCase parent)
        {
            this.Parent = parent;
        }

        public readonly TestCase Parent;

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            this[oldIndex].Index = newIndex;
            this[newIndex].Index = oldIndex;
            base.MoveItem(oldIndex, newIndex);
        }
        protected override void InsertItem(int index, T item)
        {
            item.Parent = this.Parent;
            item.Index = index;
            base.InsertItem(index, item);
            for (int i = index + 1; i < this.Count; i++)
                this[i].Index = i;
        }
        protected override void RemoveItem(int index)
        {
            if (index >= this.Count)
                return;

            this[index].Parent = null;
            base.RemoveItem(index);
            for (int i = index; i < this.Count; i++)
                this[i].Index = i;
        }
        protected override void SetItem(int index, T item)
        {
            item.Parent = this.Parent;
            base.SetItem(index, item);
        }
    }

    [Serializable]
    public class TestCaseCollection : TestItemCollection<TestCase>
    {
        public TestCaseCollection(TestCase parent) : base(parent) { }

        private bool _IsMoving;

        public void MoveToOtherCollection(TestCase item, TestCaseCollection newParentCollection)
        {
            _IsMoving = newParentCollection._IsMoving = true;

            try
            {
                this.Remove(item);
                newParentCollection.Add(item);
            }
            finally
            {
                _IsMoving = newParentCollection._IsMoving = false;
            }
        }
        protected override void InsertItem(int index, TestCase item)
        {
            updateTestFunctions(item, true);
            base.InsertItem(index, item);
        }
        protected override void RemoveItem(int index)
        {
            updateTestFunctions(this[index], false);
            base.RemoveItem(index);
        }
        private void updateTestFunctions(TestCase testCase, bool isAdded)
        {
            if (_IsMoving)
                return;

            if (testCase.IsFunction)
            {
                if (isAdded)
                    TestModel.AllFunctions.Add(testCase);
                else
                    TestModel.AllFunctions.Remove(testCase);
            }

            foreach (TestCase child in testCase.Children)
                updateTestFunctions(child, isAdded);
        }
    }

    [Serializable]
    public class TestCase : TestItemBase, IPersistent
    {
        #region Inner

        [Serializable]
        private class VariableCollection : ObservableCollection<Variable>
        {
            public VariableCollection(TestCase testcase)
            {
                _Parent = testcase;
            }

            private readonly TestCase _Parent;

            protected override void InsertItem(int index, Variable item)
            {
                item.OnVariableNameChanged += item_OnVariableNameChanged;
                base.InsertItem(index, item);
            }

            void item_OnVariableNameChanged(string oldValue, string newValue)
            {
                _Parent.OnVariableNameChanged(oldValue, newValue);
            }
        }

        #endregion

        public TestCase()
        {
            Data = new VariableCollection(this);
            Status = TestStatus.Ready;
        }

        #region IPersistent

        XElement IPersistent.SaveXml()
        {
            XElement xTestCase = new XElement("TestCase",
                new XAttribute("Id", this.Id),
                new XAttribute("Title", this.Title),
                new XAttribute("IsFunction", this.IsFunction),
                new XAttribute("Status", (int)this.Status),
                new XAttribute("LogRowIndex", this.LogRowIndex));

            if (this.Data.Count > 0)
            {
                XElement xParams = new XElement("TestCaseParams");
                foreach (IPersistent e in this.Data)
                    xParams.Add(e.SaveXml());
                xTestCase.Add(xParams);
            }

            if (this.Children != null && this.Children.Count > 0)
            {
                XElement xChildren = new XElement("Children");
                foreach (IPersistent e in this.Children)
                    xChildren.Add(e.SaveXml());
                xTestCase.Add(xChildren);
            }

            if (this.Steps != null && this.Steps.Count > 0)
            {
                XElement xStep = new XElement("Steps");
                foreach (IPersistent e in this.Steps)
                    xStep.Add(e.SaveXml());
                xTestCase.Add(xStep);
            }

            return xTestCase;
        }

        void IPersistent.LoadXml(XElement node)
        {
            _Id = XmlUtility.GetXmlValue(node.Attribute("Id"), string.Empty);
            _Title = XmlUtility.GetXmlValue(node.Attribute("Title"), string.Empty);
            _IsFunction = XmlUtility.GetXmlValue(node.Attribute("IsFunction"), false);
            this.Status = XmlUtility.GetXmlValue(node.Attribute("Status"), TestStatus.Ready);
            this.LogRowIndex = XmlUtility.GetXmlValue(node.Attribute("LogRowIndex"), 0);

            if (_IsFunction)
                TestModel.AllFunctions.Add(this);

            if (node.Elements("TestCaseParams").Elements("Variable").Count() > 0)
                foreach (var e in node.Elements("TestCaseParams").Elements("Variable"))
                {
                    Variable o = new Variable();
                    Data.Add(o);
                    (o as IPersistent).LoadXml(e);
                }

            if (node.Elements("Children").Elements("TestCase").Count() > 0)
                foreach (var e in node.Elements("Children").Elements("TestCase"))
                {
                    TestCase o = new TestCase();
                    Children.Add(o);
                    (o as IPersistent).LoadXml(e);
                }

            if (node.Elements("Steps").Elements("TestStep").Count() > 0)
                foreach (var e in node.Elements("Steps").Elements("TestStep"))
                {
                    TestStep o = new TestStep();
                    Steps.Add(o);
                    (o as IPersistent).LoadXml(e);
                }

            _RecordStatus = RecordStatus.Unchanged;
        }

        #endregion

        protected bool _IsFunction;
        public bool IsFunction
        {
            get { return _IsFunction; }
            set
            {
                _IsFunction = value;
                onPropertyChange("IsFunction");
                if (value)
                    TestModel.AllFunctions.Add(this);
                else
                    TestModel.AllFunctions.Remove(this);

                RecordStatus = RecordStatus.Changed;
            }
        }

        public ObservableCollection<Variable> Data { get; set; }

        private TestCaseCollection _Children;
        public virtual TestCaseCollection Children
        {
            get
            {
                if (_Children == null)
                    _Children = new TestCaseCollection(this);
                return _Children;
            }
        }

        private ObservableCollection<TestStep> _Steps;
        public virtual ObservableCollection<TestStep> Steps
        {
            get
            {
                if (_Steps == null)
                    _Steps = new TestItemCollection<TestStep>(this);
                return _Steps;
            }
        }

        [NonSerialized]
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

        [NonSerialized]
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

        [NonSerialized]
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

        public TestCase Clone()
        {
            TestCase res = new TestCase();
            res.Title = this.Title;
            res._IsFunction = _IsFunction;
            foreach (var e in Children)
                res.Children.Add(e.Clone());

            foreach (var e in Steps)
                res.Steps.Add(e.Clone());

            foreach (var e in this.Data)
                res.Data.Add(e.Clone());

            return res;
        }

        public override void OnVariableNameChanged(string oldValue, string newValue)
        {
            Regex reg = new Regex("(?<={+)" + oldValue + "(?=}+)");

            foreach (Variable v in this.Data)
                if (v.Value is string || v.Value is Value)
                    v.Value = reg.Replace((string)v.Value, newValue);

            foreach (TestCase child in this.Children)
                child.OnVariableNameChanged(oldValue, newValue);

            foreach (TestStep step in this.Steps)
                step.OnVariableNameChanged(oldValue, newValue);
        }
    }
}
