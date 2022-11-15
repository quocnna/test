using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Windows.Controls;

namespace AutoTest.Core
{
    public static class Log
    {
        private static TextBox _Writer;

        private static string _Padding = string.Empty;
        private static int _PaddingLevel;
        public static int PaddingLevel
        {
            get { return _PaddingLevel; }
            set
            {
                _PaddingLevel = value;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < value; i++)
                    sb.Append("  ");
                _Padding = sb.ToString();
            }
        }


        public static int LineCount
        {
            get;
            private set;
        }

        public static void WriteSeparate()
        {
            Write("_________________________________________________________");
        }

        public static void WriteEmptyLine()
        {
            LineCount++;
            _Writer.Dispatcher.Invoke(() => _Writer.AppendText("\n"));
        }
        public static void Write(params object[] paras)
        {
            StringBuilder sb = new StringBuilder();
            buildString(sb, paras);

            _Writer.Dispatcher.Invoke(() =>
                {
                    LineCount++;
                    _Writer.AppendText("\n" + DateTime.Now.ToShortTimeString() + ": ");
                    _Writer.AppendText(_Padding);
                    _Writer.AppendText(sb.ToString());
                    _Writer.ScrollToEnd();
                });
        }
        public static void buildString(StringBuilder sb, params object[] paras)
        {
            if (paras != null)
                foreach (object o in paras)
                    if (o is string)
                    {
                        sb.Append(o as string);
                        foreach (char ch in o as string)
                            if (ch == '\n')
                                LineCount++;
                    }
                    else if (o is IEnumerable)
                        foreach (var e in (IEnumerable)o)
                            buildString(sb, e);
                    else if (o != null)
                    {
                        string val = o.ToString(); 
                        sb.Append(val);
                        foreach (char ch in val as string)
                            if (ch == '\n')
                                LineCount++;
                    }
        }

        public static void Init(TextBox writer)
        {
            _Writer = writer;
            _Writer.Dispatcher.Invoke(() => _Writer.Text = "");
            LineCount = 0;
            PaddingLevel = 0;
        }
    }
}
