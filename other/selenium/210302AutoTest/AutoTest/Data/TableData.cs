using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoTest.Data
{
    public interface ITableDataRow
    {
        Value this[string header] { get; set; }
    }

    [Serializable]
    public class TableDataColumn : INotifyPropertyChanged
    {
        public TableDataColumn(string uniqueId, string header)
        {
            this.UniqueId = uniqueId;
            this.Header = header;
        }

        public readonly string UniqueId;

        private string _Header;
        public string Header
        {
            get
            {
                return _Header;
            }
            set
            {
                if (_Header != value)
                {
                    _Header = value;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Header"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Serializable]
    public class TableData : ObservableCollection<ExpandoObject>, IPersistent
    {
        #region Inner

        [Serializable]
        private class TableDataRow : ITableDataRow
        {
            public TableDataRow(TableData table, ExpandoObject row)
            {
                _Table = table;
                _Row = row as IDictionary<string, object>;
            }

            private TableData _Table;
            private IDictionary<string, object> _Row;

            public Value this[string header]
            {
                get
                {
                    TableDataColumn col = _Table.getColumnByHeader(header);
                    return col == null ? new Value(null) : _Row[col.UniqueId] is Value ? (Value)_Row[col.UniqueId] : new Value(_Row[col.UniqueId]);
                }
                set
                {
                    TableDataColumn col = _Table.getColumnByHeader(header);
                    if (col != null)
                        _Row[col.UniqueId] = value ?? new Value(null);
                }
            }
        }

        #endregion

        #region IPersistent

        XElement IPersistent.SaveXml()
        {
            XElement res = new XElement("TableData");
            XElement cols = new XElement("Cols");
            res.Add(cols);
            foreach (var col in Columns)
                cols.Add(new XElement("Col",
                    new XAttribute("Header", col.Header)));

            if (_Columns.Count > 0 && this.Count > 0)
            {
                XElement xRows = new XElement("Rows");
                foreach (var item in this)
                {
                    IDictionary<string, object> row = item as IDictionary<string, object>;
                    List<XAttribute> attrs = new List<XAttribute>();

                    int i = 0;
                    foreach (TableDataColumn col in Columns)
                    {
                        string val = row.ContainsKey(col.UniqueId) ? (row[col.UniqueId] ?? "").ToString() : "";
                        if (!string.IsNullOrEmpty(val))
                            attrs.Add(new XAttribute("_" + i, val));
                        i++;
                    }
                    XElement xRow = new XElement("Row", attrs);
                    xRows.Add(xRow);
                }
                res.Add(xRows);
            }
            return res;
        }

        void IPersistent.LoadXml(XElement node)
        {
            if (node.Elements("Cols").Elements("Col").Count() > 0)
            {
                int i = 0;
                foreach (var e in node.Elements("Cols").Elements("Col"))
                {
                    string header = e.Attribute("Header").Value;
                    TableDataColumn o = new TableDataColumn(i.ToString(), header);
                    _Columns.Add(o);
                    i++;
                }
                if (node.Elements("Rows").Elements("Row").Count() > 0)
                {
                    foreach (var e in node.Elements("Rows").Elements("Row"))
                    {
                        dynamic o = new ExpandoObject();
                        IDictionary<string, object> p = o as IDictionary<string, object>;
                        foreach (var a in e.Attributes())
                        {
                            int colIndex = int.Parse(a.Name.ToString().Substring(1));
                            p[colIndex.ToString()] = a.Value ?? "";
                        }

                        foreach (TableDataColumn col in _Columns)
                            if (!p.ContainsKey(col.UniqueId))
                                p[col.UniqueId] = "";
                        this.Add(o);
                    }
                }
            }
        }

        #endregion

        public bool MoveNext()
        {
            if (CurrentRowIndex >= this.Count - 1)
                return false;

            CurrentRowIndex++;
            return true;
        }
        public void MoveFirst()
        {
            CurrentRowIndex = 0;
        }
        public void MoveLast()
        {
            CurrentRowIndex = Math.Max(0, this.Count - 1);
        }

        public int CurrentRowIndex { get; private set; }

        public Value this[string header]
        {
            get { return this[header, -1]; }
            set { this[header, -1] = value; }
        }
        public Value this[string header, int rowIndex = -1]
        {
            get
            {
                TableDataColumn col = getColumnByHeader(header);
                if (col == null)
                    return new Value(null);

                rowIndex = rowIndex < 0 ? CurrentRowIndex : rowIndex;
                IDictionary<string, object> row = this[rowIndex] as IDictionary<string, object>;
                return new Value(row.ContainsKey(col.UniqueId) ? row[col.UniqueId] : null);
            }
            set
            {
                TableDataColumn col = getColumnByHeader(header);
                if (col != null)
                {
                    IDictionary<string, object> row = this[rowIndex] as IDictionary<string, object>;
                    if (row.ContainsKey(col.UniqueId))
                        row[col.UniqueId] = value ?? new Value(null);
                }
            }
        }

        #region Properties

        private Dictionary<string, TableDataColumn> _HeaderIndexes;
        private readonly List<TableDataColumn> _Columns = new List<TableDataColumn>();
        public IEnumerable<TableDataColumn> Columns
        {
            get { return _Columns; }
        }

        #endregion

        #region Methods

        public TableDataColumn AddColumn(string header = null)
        {
            _HeaderIndexes = null;

            int i = 1;
            header = header ?? "Field";
            while (Columns.Any(e => string.Equals(e.Header, header, StringComparison.OrdinalIgnoreCase)))
                header = "Field" + (i++);

            i = 0;
            while (Columns.Any(e => string.Equals(e.UniqueId, i.ToString(), StringComparison.OrdinalIgnoreCase)))
                i++;

            TableDataColumn col = new TableDataColumn(i.ToString(), header);
            col.PropertyChanged += (s, ev) =>
            {
                if (ev.PropertyName == "Header")
                    _HeaderIndexes = null;
            };
            _Columns.Add(col);

            foreach (IDictionary<string, object> row in this)
                row[col.UniqueId] = new Value(null);

            return col;
        }
        public ITableDataRow AddRow()
        {
            ExpandoObject row = new ExpandoObject();
            this.Add(row);

            IDictionary<string, object> dic = row as IDictionary<string, object>;
            foreach (TableDataColumn col in this.Columns)
                dic[col.UniqueId] = "";


            return new TableDataRow(this, row);
        }

        public void RemoveColumn(int index)
        {
            _HeaderIndexes = null;

            TableDataColumn col = _Columns[index];
            foreach (IDictionary<string, object> item in this)
                item.Remove(col.UniqueId);

            _Columns.RemoveAt(index);
        }
        public void MoveColumn(int index, int newIndex)
        {
            TableDataColumn column = _Columns[index];
            _Columns.Remove(column);
            _Columns.Insert(newIndex, column);
        }

        private TableDataColumn getColumnByHeader(string header)
        {
            if (_HeaderIndexes == null)
            {
                _HeaderIndexes = new Dictionary<string, TableDataColumn>();
                foreach (TableDataColumn col in Columns)
                    _HeaderIndexes[col.Header] = col;
            }

            return _HeaderIndexes.ContainsKey(header) ? _HeaderIndexes[header] : null;
        }

        public TableData Clone()
        {
            TableData res = new TableData();
            foreach (TableDataColumn col in this.Columns)
                res.AddColumn(col.Header);
            for (int i = 0; i < this.Count; i++)
            {
                ExpandoObject item = new ExpandoObject();
                IDictionary<string, object> row = item as IDictionary<string, object>;
                res.Add(item);
                IDictionary<string, object> oldRow = this[i] as IDictionary<string, object>;
                foreach (TableDataColumn col in this.Columns)
                    row[col.UniqueId] = oldRow.ContainsKey(col.UniqueId) ? oldRow[col.UniqueId] : null;
            }

            return res;
        }

        #endregion
    }
}
