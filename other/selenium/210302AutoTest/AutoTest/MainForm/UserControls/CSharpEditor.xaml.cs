using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CSharp;
using AutoTest.Core;
using AutoTest.Data;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.ComponentModel;

namespace AutoTest.UserControls
{
    public partial class CSharpEditor : UserControl
    {
        #region Inner class

        private class CompletionData : ICompletionData
        {
            public CompletionData(string text)
            {
                this.Text = text;
            }

            #region Fields

            private static Dictionary<string, List<ICompletionData>> _Items = new Dictionary<string, List<ICompletionData>>();
            private static HashSet<string> _NonExposeFunctions = new HashSet<string> { "ToString", "Equals", "ReferenceEquals", "GetHashCode", "GetType" };

            #endregion

            #region ICompleteData

            private System.Windows.Media.ImageSource _Image = null;
            public System.Windows.Media.ImageSource Image
            {
                get { return _Image; }
                set { _Image = value; }
            }
            public string Text { get; set; }
            public object Description { get; set; }
            public object Content
            {
                get { return this.Text; }
            }
            public double Priority { get { return 0; } }
            public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
            {
                textArea.Document.Replace(completionSegment, this.Text);
            }

            #endregion

            public static List<ICompletionData> GetCompleteData(Type type)
            {
                List<ICompletionData> res = _Items.ContainsKey(type.FullName) ? _Items[type.FullName] : null;
                if (res == null)
                {
                    res = new List<ICompletionData>();

                    foreach (PropertyInfo e in type.GetProperties())
                    {
                        CompletionData item = new CompletionData(e.Name);
                        item.Image = new BitmapImage(new Uri("pack://application:,,,/Images/iconProperty.gif"));
                        item.Description = string.Format("{0} {1}.{2} {{ get; {3}}}",
                            getTypeName(e.PropertyType),
                            getTypeName(e.DeclaringType),
                            e.Name,
                            e.SetMethod != null && e.SetMethod.IsPublic ? "set; " : "");
                        res.Add(item);
                    }

                    foreach (MethodInfo e in type.GetMethods())
                        if (!e.Name.StartsWith("get_") && !e.Name.StartsWith("set_") && !_NonExposeFunctions.Contains(e.Name))
                        {
                            CompletionData item = new CompletionData(e.Name);
                            StringBuilder sb = new StringBuilder();
                            foreach (ParameterInfo p in e.GetParameters())
                                sb.AppendFormat(", {0} {1}", getTypeName(p.ParameterType), p.Name);
                            if (sb.Length > 0)
                                sb.Remove(0, 2);
                            item.Image = new BitmapImage(new Uri("pack://application:,,,/Images/iconMethod.gif"));
                            item.Description = string.Format("{0} {1}({2})",
                                getTypeName(e.ReturnType),
                                e.Name,
                                sb);
                            res.Add(item);
                        }

                    _Items[type.FullName] = res = res.OrderBy(e => e.Text).ToList();
                }
                return res;
            }
            public static List<ICompletionData> GetCompleteData(List<string> words)
            {
                List<ICompletionData> res = new List<ICompletionData>();
                if (!words.IsEmpty())
                    foreach (var word in words)
                        res.Add(new CompletionData(word));
                return res;
            }
            public static List<ICompletionData> GetCompleteData(TestStep step)
            {
                List<ICompletionData> res = new List<ICompletionData>();
                Dictionary<string, Value> dic = step == null ? null : step.GetStepParas();
                if (dic != null)
                    foreach (string key in dic.Keys)
                        res.Add(new CompletionData("\"" + key + "\""));
                return res;
            }
            public static List<ICompletionData> GetCompleteData(TestCase tc, IEnumerable<GlobalVariableGroup> globalVariables)
            {
                List<ICompletionData> res = new List<ICompletionData>();
                HashSet<string> hash = new HashSet<string>();
                while (tc != null)
                {
                    foreach (var e in tc.Data)
                        if (hash.Add(e.Name))
                            res.Add(new CompletionData("\"" + e.Name + "\""));
                    tc = tc.Parent;
                }

                Action<GlobalVariableGroup, string> buildData = null;
                buildData = (g, path) =>
                {
                    if (g.Children != null)
                        foreach (var child in g.Children)
                            buildData(child, path + "." + child.Title);

                    if (g.Variables != null)
                        foreach (GlobalVariable e in g.Variables)
                            res.Add(new CompletionData("\"" + path + "." + e.Name + "\""));

                    if (g.DataTable != null)
                        foreach (TableDataColumn e in g.DataTable.Columns)
                            res.Add(new CompletionData("\"" + path + "." + e.Header + "\""));
                };

                foreach (GlobalVariableGroup e in globalVariables)
                    buildData(e, e.Title);

                return res;
            }
            public static List<ICompletionData> GetCompleteData(IEnumerable<GlobalVariableGroup> globalVariables)
            {
                List<ICompletionData> res = new List<ICompletionData>();
                Action<GlobalVariableGroup, string> buildData = null;
                buildData = (g, path) =>
                {
                    if (g.DataTable != null && !g.DataTable.Columns.IsEmpty())
                        res.Add(new CompletionData("\"" + path + "\""));

                    if (g.Children != null)
                        foreach (var child in g.Children)
                            buildData(child, path + "." + child.Title);
                };

                foreach (GlobalVariableGroup e in globalVariables)
                    buildData(e, e.Title);

                return res;
            }

            private static string getTypeName(Type type)
            {
                if (type.IsGenericType)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Type t in type.GetGenericArguments())
                        sb.AppendFormat(", {0}", getTypeName(t));

                    if (sb.Length > 0)
                        sb.Remove(0, 2);

                    return string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), sb);
                }
                return type.Name;
            }
        }

        #endregion

        #region Constructors

        public CSharpEditor()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            IHighlightingDefinition customHighlighting;
            using (Stream s = typeof(AutoTest.Main).Assembly.GetManifestResourceStream("AutoTest.UserControls.CSharpEditorInfo.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new string[] { }, customHighlighting);

            txtEditor.TextArea.TextEntering += TextArea_TextEntering;
            txtEditor.TextArea.TextEntered += TextArea_TextEntered;
            txtEditor.SyntaxHighlighting = customHighlighting;

            loadKeyWords();

            btnCompile.Click += btnCompile_Click;
            txtEditor.TextChanged += txtEditor_TextChanged;
        }

        #endregion

        #region Fields

        private CompletionWindow completionWindow;
        private HashSet<char> terminalChars = new HashSet<char> { ' ', ';', (char)9, (char)10, (char)13, '(', ')', '}', '{', '=' };
        private bool typing;

        #endregion

        #region Properties

        private TestStep _Step;
        public virtual TestStep Step
        {
            get { return _Step; }
            set
            {
                if (_Step != null)
                {
                    _Step.PropertyChanged -= step_PropertyChanged;
                }
                _Step = value;
                txtEditor.Text = value == null ? "" : TextValue;
                if (_Step != null)
                {
                    _Step.PropertyChanged += step_PropertyChanged;
                }
            }
        }

        private void step_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == (IsBindingToBeforeRunning ? "BeforeRunning" : "AfterRunning") && !typing)
            {
                txtEditor.Text = TextValue;
            }
        }

        public TestCase TestCase { get; set; }

        public bool IsBindingToBeforeRunning { get; set; }

        public IEnumerable<GlobalVariableGroup> GlobalVariables { get; set; }

        private string TextValue
        {
            get
            {
                return Step == null ? "" : (IsBindingToBeforeRunning ? Step.BeforeRunning : Step.AfterRunning);
            }
            set
            {
                if (Step != null)
                {
                    if (IsBindingToBeforeRunning)
                        Step.BeforeRunning = value;
                    else
                        Step.AfterRunning = value;
                }
            }
        }

        private List<string> _KeyWords { get; set; }

        #endregion

        #region Events

        void txtEditor_TextChanged(object sender, EventArgs e)
        {
            typing = true;
            TextValue = txtEditor.Text;
            typing = false;
        }

        void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                string token = getCurrentToken(txtEditor.CaretOffset - 2);
                if (token == "me")
                    showCompletion(CompletionData.GetCompleteData(typeof(AutoTest.Data.IStepInstance)));
                else if (token == "me.Data")
                    showCompletion(CompletionData.GetCompleteData(typeof(AutoTest.Data.ITableMemory)));
            }
            else if (e.Text == "[")
            {
                string token = getCurrentToken(txtEditor.CaretOffset - 2);
                if (token == "me.Data")
                    showCompletion(CompletionData.GetCompleteData(this.TestCase, this.GlobalVariables));
                else if (token == "me.StepData")
                    showCompletion(CompletionData.GetCompleteData(this.Step));
            }
            else if (e.Text == "(")
            {
                string token = getCurrentToken(txtEditor.CaretOffset - 2);
                if (token == "me.Data.MoveNext" || token == "me.Data.MoveFirst" 
                    || token == "me.Data.MoveLast" || token == "me.Data.FindRow"
                    || token == "me.Data.GetCurrentRowIndex" || token == "me.Data.GetTotalRecords")
                    showCompletion(CompletionData.GetCompleteData(this.GlobalVariables));
            }
            //else if (e.Text == "\"")
            //{
            //    string token = getCurrentToken(txtEditor.CaretOffset - 2);
            //    if (token == "me.Data[")
            //        showCompletion(CompletionData.GetCompleteData(this.GlobalVariables));
            //}
        }
        void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]) && e.Text != "\"")
                {
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            //else if (isLoadKeyWordsMenu(e.Text))
            //{
            //    showCompletion(CompletionData.GetCompleteData(this._KeyWords));
            //}
        }

        private void showCompletion(List<ICompletionData> data)
        {
            if (completionWindow == null)
            {
                completionWindow = new CompletionWindow(txtEditor.TextArea);
                TextOptions.SetTextFormattingMode(completionWindow, TextFormattingMode.Display);

                completionWindow.CompletionList.CompletionData.Clear();
                foreach (ICompletionData item in data)
                    completionWindow.CompletionList.CompletionData.Add(item);

                completionWindow.Show();
                completionWindow.Closing += delegate
                {
                    completionWindow = null;
                };
            }
        }
        private string getCurrentToken(int position)
        {
            if (txtEditor.Text.Length > 0 && (position < txtEditor.Text.Length - 1 || position >= 0))
            {
                StringBuilder sb = new StringBuilder();
                while (position >= 0 && !terminalChars.Contains(txtEditor.Text[position]))
                    sb.Insert(0, txtEditor.Text[position--]);
                return sb.ToString();
            }

            return "";
        }

        private bool isLoadKeyWordsMenu(string text)
        {
            if (text == ".")
                return false;
            var position = txtEditor.CaretOffset;
            StringBuilder textCurrent = new StringBuilder();
            StringBuilder textPrevious = new StringBuilder();
            var allText = string.Format("{0}{1}", txtEditor.Text, text);
            if (position < allText.Length - 1 || position >= 0)
            {
                while (position >= 0 && !terminalChars.Contains(allText[position]))
                    textCurrent.Insert(0, allText[position--]);
                position--;
                while (position >= 0 && !terminalChars.Contains(allText[position]))
                    textPrevious.Insert(0, allText[position--]);
            }

            if (_KeyWords.Contains(textCurrent.ToString()) || _KeyWords.Contains(textPrevious.ToString()) || terminalChars.Contains(Convert.ToChar(text)))
                return false;
            return true;
        }
        private void loadKeyWords()
        {
            _KeyWords = new List<string>();
            IList<HighlightingRule> list = txtEditor.SyntaxHighlighting.MainRuleSet.Rules;
            foreach (var rule in list)
            {
                List<string> temp = rule.Regex.ToString().Replace("\\b(?>", "").Replace(")\\b", "").Split('|').ToList();
                _KeyWords.AddRange(temp);
            }
            _KeyWords.OrderBy(e => e);
        }

        private void btnCompile_Click(object sender, RoutedEventArgs e)
        {
            if (Step != null)
            {
                string error = IsBindingToBeforeRunning ? Step.CompileBeforeRunning() : Step.CompileAfterRunning();
                MessageBox.Show(error.IsEmpty() ? "Compile successfully!" : error, "Compile fail!");
            }
        }

        #endregion
    }
}
