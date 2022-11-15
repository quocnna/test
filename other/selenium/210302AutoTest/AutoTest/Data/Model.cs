using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public class TestModel : IPersistent
    {
        #region Inner

        private class TestCaseFunctionCollection : ObservableCollection<TestCase>
        {
            public override event NotifyCollectionChangedEventHandler CollectionChanged;
            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                var eh = CollectionChanged;
                if (eh != null)
                {
                    Dispatcher dispatcher = (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                                             let dpo = nh.Target as DispatcherObject
                                             where dpo != null
                                             select dpo.Dispatcher).FirstOrDefault();
                    if (dispatcher != null && dispatcher.CheckAccess() == false)
                        dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => OnCollectionChanged(e)));
                    else
                        foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                            nh.Invoke(this, e);
                }
            }

            private HashSet<string> _HashId = new HashSet<string>();
            protected override void InsertItem(int index, TestCase item)
            {
                if (_HashId.Add(item.Id))
                    base.InsertItem(index, item);
            }
            protected override void RemoveItem(int index)
            {
                if (index < this.Count)
                {
                    _HashId.Remove(this[index].Id);
                    base.RemoveItem(index);
                }
            }
            protected override void ClearItems()
            {
                _HashId.Clear();
                base.ClearItems();
            }
        }

        #endregion

        #region Constructors

        public TestModel()
        {
            AllFunctions.Clear();
            TestCases = new TestCaseCollection(null);
            GlobalVariables = new ObservableCollection<GlobalVariableGroup>();
            Users = new List<User>();
            CheckPoints = new ObservableCollection<CheckPoint>();

            try
            {
                string temp = Utility.GetEnvironment("Temp", EnvironmentVariableTarget.User);
                temp = temp.IsEmpty() ? AppDomain.CurrentDomain.BaseDirectory : temp;
                temp = temp + (temp.EndsWith("\\") ? "" : "\\") + "AutoTestCheckPoints";

                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);
                this.CheckPointsFolder = temp;
            }
            catch { }

        }

        #endregion

        #region Fields

        public static readonly ObservableCollection<TestCase> AllFunctions = new TestCaseFunctionCollection();
        public static IntPtr MainWindowHandle = IntPtr.Zero;

        #endregion

        #region Properties

        public string FileName { get; set; }
        public TestStep StartStep { get; set; }
        public TestStep EndStep { get; set; }
        public string CheckPointsFolder { get; private set; }

        public ObservableCollection<GlobalVariableGroup> GlobalVariables { get; private set; }
        public ObservableCollection<CheckPoint> CheckPoints { get; private set; }

        public List<User> Users { get; private set; }

        public ObservableCollection<TestCase> TestCases { get; private set; }

        #endregion

        #region IPersistent

        XElement IPersistent.SaveXml()
        {
            XElement node = new XElement("NeogovTest");

            node.Add(new XElement("Mark",
                        new XAttribute("Start", StartStep != null ? StartStep.Id : string.Empty),
                        new XAttribute("End", EndStep != null ? EndStep.Id : string.Empty)));
            _ = new List<TestCase>();
            foreach (IPersistent e in TestCases)
                node.Add(e.SaveXml());
            foreach (IPersistent e in GlobalVariables)
                node.Add(e.SaveXml());
            foreach (IPersistent e in CheckPoints)
                node.Add(e.SaveXml());

            return node;
        }

        void IPersistent.LoadXml(XElement node)
        {
            AllFunctions.Clear();

            string startId = string.Empty, endId = string.Empty;
            if (node.Element("Mark") != null)
            {
                startId = XmlUtility.GetXmlValue(node.Element("Mark").Attribute("Start"), string.Empty);
                endId = XmlUtility.GetXmlValue(node.Element("Mark").Attribute("End"), string.Empty);
            }

            foreach (var e in node.Elements("TestCase"))
            {
                TestCase tc = new TestCase();
                TestCases.Add(tc);
                (tc as IPersistent).LoadXml(e);
            }

            foreach (var e in node.Elements("Global"))
            {
                GlobalVariableGroup g = new GlobalVariableGroup();
                GlobalVariables.Add(g);
                (g as IPersistent).LoadXml(e);
            }

            foreach (var e in node.Elements("CheckPoint"))
            {
                CheckPoint c = new CheckPoint();
                CheckPoints.Add(c);
                (c as IPersistent).LoadXml(e);
            }

            Func<string, IList<TestCase>, TestStep> findStep = null;
            findStep = (id, nodes) =>
                {
                    foreach (TestCase tc in nodes)
                    {
                        foreach (TestStep e in tc.Steps)
                            if (e.Id == id)
                                return e;

                        TestStep step = findStep(id, tc.Children);
                        if (step != null)
                            return step;
                    }

                    return null;
                };

            StartStep = findStep(startId, TestCases);
            EndStep = findStep(endId, TestCases);
        }

        #endregion
    }
}
