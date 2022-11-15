using AutoTest.Core;
using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoTest.UserControls
{
    public partial class VariablesUI : UserControl
    {
        #region Constructors

        public VariablesUI()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += delegate
            {
                if (IsReadOnly)
                {
                    colName.IsReadOnly = true;
                    dataGrid.CanUserAddRows = false;
                    dataGrid.CanUserDeleteRows = false;
                }
            };

            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGrid.CellEditEnding += dataGrid_CellEditEnding;
            dataGrid.RowEditEnding += dataGrid_RowEditEnding;
        }

        void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            string name = (e.Row.Item as Variable).Name ?? "";
            e.Cancel = name.Contains('.');
        }

        void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Name")
            {
                TextBox textBox = (e.EditingElement as TextBox);
                if (textBox.Text.Contains('.') || string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Background = Brushes.LightPink;
                    MessageBox.Show("Please input valid parameter name", "Warning", MessageBoxButton.OK);
                    e.Cancel = true;
                }
            }
        }

        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Fields

        private IEnumerable _Clipboard;

        #endregion

        #region Properties

        public IEnumerable ItemsSource
        {
            get { return dataGrid.ItemsSource; }
            set { dataGrid.ItemsSource = value; }
        }

        public bool IsReadOnly { get; set; }

        public bool ShowColumnPublic
        {
            get { return this.colPublic.Visibility == System.Windows.Visibility.Visible; }
            set { this.colPublic.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
        }

        #endregion

        #region Events

        private void textBoxCellEditing_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            Dispatcher.BeginInvoke((Action)(() => { txt.SelectAll(); }));
        }

        private void dataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer)
                Keyboard.Focus(dataGrid);
        }

        #endregion

        #region Commands

        private void cutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.IsReadOnly)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = !dataGrid.SelectedItems.IsEmpty();
            if (dataGrid.SelectedItems.Count == 1 && !(dataGrid.SelectedItem is Variable))
                e.CanExecute = false;
        }
        private void cutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<object> list = new List<object>();
            foreach (object o in (sender as DataGrid).SelectedItems)
                list.Add(o);
            _Clipboard = list;

            foreach (Variable o in list)
            {
                (ItemsSource as IList<Variable>).Remove(o);
            }
        }

        private void copyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            if (this.IsReadOnly)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = !dataGrid.SelectedItems.IsEmpty();
            if (dataGrid.SelectedItems.Count == 1 && !(dataGrid.SelectedItem is Variable))
                e.CanExecute = false;
        }
        private void copyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<object> list = new List<object>();
            foreach (object o in (sender as DataGrid).SelectedItems)
                list.Add(o);
            _Clipboard = list;
        }
        
        private void pasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.IsReadOnly)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = _Clipboard != null;
        }
        private void pasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable list = _Clipboard as IEnumerable;
            if (list != null)
            {
                IList dataSource = (sender as DataGrid).ItemsSource as IList;
                foreach (object o in list)
                    if (o is Variable)
                        dataSource.Add((o as Variable).Clone());
            }
        }

        private void moveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = !this.IsReadOnly && grid.SelectedItem != null && grid.SelectedItem is Variable;
            e.Handled = true;
        }
        private void moveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            Variable variable = grid.SelectedItem as Variable;
            ObservableCollection<Variable> variables = grid.ItemsSource as ObservableCollection<Variable>;
            int selectedIndex = variables.IndexOf(variable);
            if (selectedIndex > 0)
                variables.Move(selectedIndex, selectedIndex - 1);
            Keyboard.Focus(grid);
        }

        private void moveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = !this.IsReadOnly && grid.SelectedItem != null && grid.SelectedItem is Variable;
            e.Handled = true;
        }
        private void moveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            Variable variable = grid.SelectedItem as Variable;
            ObservableCollection<Variable> variables = grid.ItemsSource as ObservableCollection<Variable>;
            int selectedIndex = variables.IndexOf(variable);
            if (selectedIndex < variables.Count - 1)
                variables.Move(selectedIndex, selectedIndex + 1);
            Keyboard.Focus(grid);
        }

        #endregion
    }

    public class VariableCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Variable variable = item as Variable;
            if (variable != null && variable.Value is IList)
                return TableTemplate;
            return TextTemplate;
        }
    }
}
