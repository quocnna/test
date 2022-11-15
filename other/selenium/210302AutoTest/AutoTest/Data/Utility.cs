using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public class XmlUtility
    {
        public static T GetXmlValue<T, T2>(T2 attr, T defaultValue)
        {
            if (attr != null)
                if (defaultValue.GetType() == typeof(TestStatus))
                    return (T)Convert.ChangeType(
                    (attr is XElement) ? (TestStatus)Convert.ToInt16((attr as XElement).Value) : (TestStatus)Convert.ToInt16((attr as XAttribute).Value),
                    typeof(T));
                else
                    return (T)Convert.ChangeType(
                        (attr is XElement) ? (attr as XElement).Value : (attr as XAttribute).Value,
                        typeof(T));
            return defaultValue;
        }
    }

    internal static class Utility
    {
        public static bool IsEmpty(this string val)
        {
            return val == null || val.Trim().Length == 0;
        }
        public static bool IsEmpty(this IEnumerable val)
        {
            return val == null || !val.GetEnumerator().MoveNext();
        }

        public static string GetEnvironment(string envName, EnvironmentVariableTarget target)
        {
            string st = Environment.GetEnvironmentVariable(envName, target);
            return getEnvironmentReference(st, target);
        }
        private static string getEnvironmentReference(string envValue, EnvironmentVariableTarget target)
        {
            //Dim nPos, nStart, nEnd, strResult, strTemp
            string result = "";
            int start = envValue.IndexOf('%');
            if (start < 0) return envValue;
            int end = envValue.IndexOf('%', start + 1);
            if (end < start) return envValue;

            string temp = envValue.Substring(start + 1, end - 1);
            temp = GetEnvironment(temp, target);

            result = envValue.Substring(0, start) + temp + (end < envValue.Length - 1 ? envValue.Substring(end + 1) : "");
            return result;
        }
    }
}
