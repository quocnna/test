using AutoTest.Core;
using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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

namespace AutoTest.UserControls
{
    public partial class TableDataUI : UserControl
    {
        #region Constructors

        public TableDataUI()
        {
            InitializeComponent();

            btnAddColumn.Click += btnAddColumn_Click;
            btnDeleteColumn.Click += btnDeleteColumn_Click;
        }

        #endregion

        #region Fields

        private int _ReorderingColumnIndex;
        DataGridColumn _SelectedGridColumn;

        #endregion

        #region Properties

        public TableData DataSource
        {
            get
            {
                return (TableData)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(
                "DataSource",
                typeof(TableData),
                typeof(TableDataUI),
                new PropertyMetadata(default(TableData), OnDataSourcePropertyChanged));
        private static void OnDataSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TableDataUI dataUI = d as TableDataUI;
            dataUI.dataGrid.ItemsSource = null;
            ObservableCollection<ExpandoObject> dataSource = e.NewValue != null ? e.NewValue as TableData : null;
            dataUI.buildUI();
            if (e.NewValue != null)
                dataUI.dataGrid.ItemsSource = dataSource;
        }

        #endregion

        #region Events

        void btnAddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (DataSource == null)
                return;
            TableDataColumn col = DataSource.AddColumn();
            addGridColumn(col);
        }

        void btnDeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            if (_SelectedGridColumn != null && dataGrid.Columns.Contains(_SelectedGridColumn))
            {
                int i = dataGrid.Columns.IndexOf(_SelectedGridColumn);
                int j = dataGrid.Columns.OrderBy(c => c.DisplayIndex).ToList().IndexOf(_SelectedGridColumn);
                dataGrid.Columns.RemoveAt(i);
                if (DataSource != null)
                    DataSource.RemoveColumn(j);
                if (dataGrid.Columns.Count == 0)
                    dataGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            _SelectedGridColumn = null;
        }

        void dataGrid_ColumnReordering(object sender, DataGridColumnReorderingEventArgs e)
        {
            _ReorderingColumnIndex = e.Column.DisplayIndex;
        }

        void dataGrid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            DataSource.MoveColumn(_ReorderingColumnIndex, e.Column.DisplayIndex);
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.Column != null)
                _SelectedGridColumn = dataGrid.CurrentCell.Column;
        }

        #endregion

        #region Commands

        private void dataGridCopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }
        private void dataGridMoveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is ExpandoObject;
            e.Handled = true;
        }
        private void dataGridMoveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TableData dataTable = grid.ItemsSource as TableData;
            int index = dataTable.IndexOf(grid.SelectedItem as ExpandoObject);
            if (index < dataTable.Count - 1)
                dataTable.Move(index, index + 1);
            Keyboard.Focus(grid);
        }

        private void dataGridMoveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is ExpandoObject;
            e.Handled = true;
        }
        private void dataGridMoveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TableData dataTable = grid.ItemsSource as TableData;
            int index = dataTable.IndexOf(grid.SelectedItem as ExpandoObject);
            if (index > 0)
                dataTable.Move(index, index - 1);
            Keyboard.Focus(grid);
        }

        #endregion

        #region Methods

        private void buildUI()
        {
            dataGrid.Columns.Clear();
            foreach (TableDataColumn col in DataSource.Columns)
                addGridColumn(col);
        }

        private void addGridColumn(TableDataColumn col)
        {
            DataGridTextColumn gridColumn = new DataGridTextColumn();
            TextBox txtHeader = new TextBox();
            txtHeader.BorderThickness = new Thickness(0);
            txtHeader.Background = Brushes.Transparent;
            txtHeader.MinWidth = 30;
            txtHeader.GotFocus += delegate
            {
                _SelectedGridColumn = gridColumn;
            };

            Binding binding = new Binding("Header");
            binding.Mode = BindingMode.TwoWay;
            binding.Source = col;
            BindingOperations.SetBinding(txtHeader, TextBox.TextProperty, binding);

            gridColumn.Header = txtHeader;
            gridColumn.Binding = new Binding(col.UniqueId);
            gridColumn.CanUserSort = false;
            gridColumn.MinWidth = 72;


            dataGrid.Columns.Add(gridColumn);
            if (dataGrid.Visibility != System.Windows.Visibility.Visible)
                dataGrid.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion
    }
}
