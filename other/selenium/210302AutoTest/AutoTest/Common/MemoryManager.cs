using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoTest.Core
{
    #region MemoryBase

    public abstract class MemoryBase : IMemory
    {
        public MemoryBase(Dictionary<string, Value> data)
        {
            Data = data;
        }

        protected readonly Dictionary<string, Value> Data;
        public abstract Value this[string variableName] { get; set; }

        IEnumerator<KeyValuePair<string, Value>> IEnumerable<KeyValuePair<string, Value>>.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }

    #endregion

    #region TableMemoryBase

    internal abstract class TableMemoryBase : MemoryBase, ITableMemory
    {
        public TableMemoryBase(Dictionary<string, Value> data) : base(data)
        {
        }

        public abstract Value this[string variableName, int rowIndex] { get; set; }

        protected string getLookupValue(string text)
        {
            if (text.IsEmpty())
                return text;

            StringBuilder res = new StringBuilder(text);
            Stack<int> stack = new Stack<int>();
            for (int i = 0; i < res.Length; i++)
            {
                char ch = res[i];

                if (ch == '\\')
                    res.Remove(i, 1);
                else if (ch == '{')
                    stack.Push(i);
                else if ((ch == '}' || ch == ',') && stack.Count > 0)
                {
                    int index = stack.Pop();
                    string token = res.ToString(index + 1, i - index - 1).Trim();
                    _ = "";
                    Value value;
                    if (ch == ',')
                    {
                        StringBuilder sb = new StringBuilder();
                        while (++i < res.Length && res[i] != '}')
                            sb.Append(res[i]);
                        int rowIndex = int.TryParse(sb.ToString(), out rowIndex) ? rowIndex : -1;
                        value = rowIndex < 0 ? this[token] : this[token, rowIndex];
                    }
                    else
                        value = this[token];

                    res.Remove(index, i - index + 1);
                    res.Insert(index, (value ?? "").ToString());
                    i = index;
                }
            }

            return res.ToString();
        }

        public bool MoveNext(string variableName)
        {
            Value value = this[variableName];
            return value != null && value.RawData is TableData ? (value.RawData as TableData).MoveNext() : false;
        }
        public void MoveFirst(string variableName)
        {
            Value value = this[variableName];
            if (value != null && value.RawData is TableData)
                (value.RawData as TableData).MoveFirst();
        }
        public void MoveLast(string variableName)
        {
            Value value = this[variableName];
            if (value != null && value.RawData is TableData)
                (value.RawData as TableData).MoveLast();
        }

        public int GetCurrentRowIndex(string variableName)
        {
            Value value = this[variableName];
            int res = value != null && value.RawData is TableData ? (value.RawData as TableData).CurrentRowIndex : -1;

            Log.Write("GetCurrentRowIndex=", res);
            return res;
        }
        public int GetTotalRecords(string variableName)
        {
            Value value = this[variableName];
            int res = value != null && value.RawData is TableData ? (value.RawData as TableData).Count : -1;

            Log.Write("GetTotalRecords=", res);
            return res;
        }
    }

    #endregion

    #region GlobalMemory

    internal class GlobalMemory : TableMemoryBase
    {
        #region Inner Class

        private class TableDataCell
        {
            public string ColumnHeader;
            public TableData Table;
        }

        #endregion

        public GlobalMemory(IEnumerable<GlobalVariableGroup> groups)
            : base(new Dictionary<string, Value>(StringComparer.OrdinalIgnoreCase))
        {
            if (groups == null)
                return;

            Action<GlobalVariableGroup, string> buildData = null;
            buildData = (g, path) =>
                {
                    if (g.Children != null)
                        foreach (var child in g.Children)
                            buildData(child, path + "." + child.Title);

                    if (g.Variables != null)
                        foreach (GlobalVariable e in g.Variables)
                            Data[path + "." + e.Name] = new Value(e.Value);

                    if (g.DataTable != null && !g.DataTable.Columns.IsEmpty())
                    {
                        Data[path] = new Value(g.DataTable);
                        foreach (TableDataColumn col in g.DataTable.Columns)
                            Data[path + "." + col.Header] = new Value(new TableDataCell() { ColumnHeader = col.Header, Table = g.DataTable });
                    }
                };

            foreach (GlobalVariableGroup e in groups)
                buildData(e, e.Title);
        }

        public override Value this[string variableName]
        {
            get { return this[variableName, -1]; }
            set { this[variableName, -1] = variableName; }
        }

        public override Value this[string variableName, int rowIndex]
        {
            get
            {
                variableName = getLookupValue(variableName);

                Value value = Data.ContainsKey(variableName) ? Data[variableName] : new Value(null);

                if (value.RawData is TableDataCell)
                {
                    TableDataCell cell = value.RawData as TableDataCell;
                    value = cell.Table[cell.ColumnHeader, rowIndex];
                }

                if (value.RawData is string)
                {
                    string st = getLookupValue(value.RawData as string);
                    value = new Value(st);
                }

                Log.Write(variableName, rowIndex < 0 ? "" : "[" + rowIndex + "]", "=", value);
                return value;
            }
            set
            {
                variableName = getLookupValue(variableName);

                Value val = Data.ContainsKey(variableName) ? Data[variableName] : new Value(null);
                if (val.RawData is TableDataCell)
                {
                    TableDataCell cell = value.RawData as TableDataCell;
                    cell.Table[cell.ColumnHeader, rowIndex] = value;
                }
            }
        }
    }

    #endregion

    #region MemoryManager

    internal class MemoryManager : TableMemoryBase
    {
        public MemoryManager(GlobalMemory globalMemory)
            : base(new Dictionary<string, Value>(StringComparer.OrdinalIgnoreCase))
        {
            GlobalData = globalMemory;
        }

        #region Fields

        private readonly Stack<object> _Stack = new Stack<object>();
        public readonly GlobalMemory GlobalData;

        #endregion

        #region IMemory

        public override Value this[string variableName]
        {
            get
            {
                variableName = getLookupValue(variableName);

                bool isNotGlobal = Data.ContainsKey(variableName);
                Value value = isNotGlobal ? Data[variableName] : GlobalData[variableName];
                if (value.RawData is string)
                {
                    string st = getLookupValue(value.RawData as string);
                    value = new Value(st);
                }

                if (isNotGlobal)
                    Log.Write(variableName, "=", value);
                return value;
            }
            set
            {
                variableName = getLookupValue(variableName);

                if (Data.ContainsKey(variableName))
                    Data[variableName] = value;
                else
                    GlobalData[variableName] = value;
            }
        }

        public override Value this[string variableName, int rowIndex]
        {
            get { return GlobalData[variableName, rowIndex]; }
            set { GlobalData[variableName, rowIndex] = variableName; }
        }

        #endregion

        #region Methods

        public void Push(TestCase tc)
        {
            _Stack.Push(tc);
            if (tc != null)
            {
                if (tc != null && tc.Data != null)
                    foreach (Variable e in tc.Data)
                    {
                        if (!e.IsPublic || !Data.ContainsKey(e.Name))
                        {
                            if (Data.ContainsKey(e.Name))
                                _Stack.Push(new KeyValuePair<string, Value>(e.Name, Data[e.Name]));
                            Data[e.Name] = new Value(e.Value);
                        }

                    }
            }
        }
        public void PushBackToRoot(TestCase tc)
        {
            TestCase temp = tc;
            while (temp != null)
            {
                if (!temp.Data.IsEmpty())
                    foreach (Variable e in temp.Data)
                        if (!Data.ContainsKey(e.Name))
                            Data[e.Name] = new Value(e.Value);
                temp = temp.Parent;
            }
        }

        public TestCase Pop()
        {
            object o = null;
            while (!((o = _Stack.Pop()) is TestCase))
            {
                KeyValuePair<string, Value> e = (KeyValuePair<string, Value>)o;
                Data[e.Key] = e.Value;
            }

            return o as TestCase;
        }

        #endregion
    }

    #endregion

    #region StepDataMemory

    internal class StepDataMemory : MemoryBase, IMemory
    {
        public StepDataMemory(Dictionary<string, Value> data, MemoryManager memory) : base(data)
        {
            Memory = memory;
        }

        public readonly MemoryManager Memory;

        public override Value this[string variableName]
        {
            get
            {
                variableName = getLookupValue(variableName);
                Value value = Data.ContainsKey(variableName) ? Data[variableName] : new Value(null);
                if (value.RawData is string)
                {
                    string st = getLookupValue(value.RawData as string);
                    value = new Value(st);
                }

                Log.Write(variableName, "=", value);
                return value;
            }
            set
            {
                variableName = getLookupValue(variableName);
                if (Data.ContainsKey(variableName))
                    Data[variableName] = value;
            }
        }

        protected virtual string getLookupValue(string text)
        {
            if (text.IsEmpty())
                return text;

            StringBuilder res = new StringBuilder(text);
            Stack<int> stack = new Stack<int>();
            for (int i = 0; i < res.Length; i++)
            {
                char ch = res[i];

                if (ch == '\\')
                    res.Remove(i, 1);
                else if (ch == '{')
                    stack.Push(i);
                else if (ch == '}' && stack.Count > 0)
                {
                    int index = stack.Pop();
                    string token = res.ToString(index + 1, i - index - 1).Trim();
                    Value value = Memory[token];
                    res.Remove(index, i - index + 1);
                    res.Insert(index, (value ?? "").ToString());
                    i = index;
                }
            }

            return res.ToString();
        }
    }

    #endregion
}
