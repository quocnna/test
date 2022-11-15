using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace AutoTest.DBAction
{
    [Serializable]
    public class DBAction : ActionBase
    {
        public DBAction()
            : base()
        {
            if (_UI == null)
                _UI = new DBActionContent();
        }

        public override string Name
        {
            get { return "DB Command"; }
        }

        private static DBActionContent _UI;
        public override UserControl UI
        {
            get { return _UI; }
        }

        public override void Execute(IStepInstance step)
        {
        }

        public override ActionBase Clone()
        {
            return new DBAction();
        }

        public override XElement SaveXml()
        {
            XElement xElement = new XElement("Action", new XAttribute("Type", typeof(DBAction).FullName)
                , new XElement("Name", this.Name));

            return xElement;
        }

        public override void LoadXml(XElement element)
        {
            //foreach (var state in element.Element("States").Elements("State"))
            //    this.State[state.Attribute("Key").Value] = state.Attribute("Value").Value;
        }

        private SimpleActionState _State = new SimpleActionState();
        public override IActionState State
        {
            get { return _State; }
        }
    }
}
