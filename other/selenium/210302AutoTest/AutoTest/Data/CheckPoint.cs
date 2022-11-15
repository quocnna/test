using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public class CheckPoint : IPersistent
    {
        public string Title { get; set; }
        public string LogData { get; set; }
        public string TestCaseImageFileName { get; set; }
        public string ApplicationImageFileName { get; set; }

        XElement IPersistent.SaveXml()
        {
            XElement xCheckPoint = new XElement("CheckPoint",
                new XAttribute("Title", this.Title ?? string.Empty),
                new XAttribute("LogData", this.LogData ?? string.Empty),
                new XAttribute("TestCaseImageFileName", this.TestCaseImageFileName ?? string.Empty),
                new XAttribute("ApplicationImageFileName", this.ApplicationImageFileName ?? string.Empty));
            return xCheckPoint;
        }

        void IPersistent.LoadXml(XElement node)
        {
            Title = XmlUtility.GetXmlValue(node.Attribute("Title"), string.Empty);
            LogData = XmlUtility.GetXmlValue(node.Attribute("LogData"), string.Empty);
            TestCaseImageFileName = XmlUtility.GetXmlValue(node.Attribute("TestCaseImageFileName"), string.Empty);
            ApplicationImageFileName = XmlUtility.GetXmlValue(node.Attribute("ApplicationImageFileName"), string.Empty);
        }
    }
}
