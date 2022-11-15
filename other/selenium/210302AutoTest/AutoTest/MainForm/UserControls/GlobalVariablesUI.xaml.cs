using AutoTest.Core;
using AutoTest.Data;
using AutoTest.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AutoTest.UserControls
{
    public partial class GlobalVariablesUI : UserControl
    {
        #region Constructors

        public GlobalVariablesUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Fields

        object _Clipboard;
        Point _StartPoint;
        bool _IsDragging;

        #endregion

        #region Events

        void treeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid)
                Keyboard.Focus(treeView);
        }

        private void dataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)sender);
            if (treeItem != null)
                treeItem.IsSelected = true;
        }

        private void dataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as DataGrid).SelectedItem = null;
        }

        private void tableData_GotFocus(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)sender);
            if (treeItem != null)
                treeItem.IsSelected = true;
        }

        private void tableData_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TableDataUI).dataGrid.SelectedItem = null;
        }

        void treeView_ItemSelected(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is GlobalVariableGroup)
                treeView.Tag = e.OriginalSource;
        }

        void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is GlobalVariableGroup)
            {
                panelDescription.Visibility = Visibility.Visible;
                Binding bind = new Binding("Description");
                bind.Mode = BindingMode.TwoWay;
                bind.Source = treeView.SelectedItem;
                bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(txtDescription, TextBox.TextProperty, bind);
            }
            else
            {
                panelDescription.Visibility = Visibility.Collapsed;
                BindingOperations.ClearBinding(txtDescription, TextBox.TextProperty);
            }
        }

        void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            if (targetTreeViewItem == null)
            {
                if (treeView.SelectedItem != null && treeView.SelectedItem is GlobalVariableGroup)
                    (treeView.SelectedItem as GlobalVariableGroup).IsSelected = false;
            }
            else
                _StartPoint = e.GetPosition(null);
        }

        void treeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            if (targetTreeViewItem == null)
            {
                if (treeView.SelectedItem != null)
                    (treeView.SelectedItem as GlobalVariableGroup).IsSelected = false;
            }
            else if (targetTreeViewItem.DataContext is GlobalVariableGroup)
                (targetTreeViewItem.DataContext as GlobalVariableGroup).IsSelected = true;
        }

        void treeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_IsDragging)
            {
                if (e.OriginalSource is TextBox)
                    return;
                TreeViewItem sourceTreeViewItem = UIHelper.GetAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (sourceTreeViewItem == null || sourceTreeViewItem.DataContext is GlobalVariablesWrapper || sourceTreeViewItem.DataContext is GlobalDataTableWrapper)
                    return;

                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - _StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    _IsDragging = true;
                    DataObject data = null;
                    if (treeView.SelectedItem is GlobalVariableGroup)
                        data = new DataObject("group", treeView.SelectedItem);
                    if (data != null)
                    {
                        DragDropEffects dde = DragDropEffects.Copy | DragDropEffects.Move;
                        if (e.RightButton == MouseButtonState.Pressed)
                            dde = DragDropEffects.All;
                        _ = DragDrop.DoDragDrop(treeView, data, dde);
                    }
                    _IsDragging = false;
                }
            }
        }

        void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("group"))
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            }
            else
            {
                GlobalVariableGroup sourceGroup = (GlobalVariableGroup)e.Data.GetData("group");
                TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                GlobalVariableGroup targetGroup = targetTreeViewItem != null ? targetTreeViewItem.DataContext as GlobalVariableGroup : null;
                if (targetGroup != null)
                {
                    if (sourceGroup == targetGroup || 
                        (targetGroup == sourceGroup.Parent && (e.KeyStates & DragDropKeyStates.ControlKey) != DragDropKeyStates.ControlKey) || 
                        !targetGroup.Variables.IsEmpty() || 
                        (targetGroup.DataTable != null && targetGroup.DataTable.Count > 0))
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                    }
                    else
                    {
                        GlobalVariableGroup parent = targetGroup.Parent;
                        bool sourceIsParentOfTarget = parent == sourceGroup;
                        while (!sourceIsParentOfTarget && parent != null)
                        {
                            parent = parent.Parent;
                            sourceIsParentOfTarget = parent == sourceGroup;
                        }
                        if (sourceIsParentOfTarget)
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
                        }
                    }
                }
                e.Handled = true;
            }
        }

        void treeView_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (data.GetDataPresent("group"))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                GlobalVariableGroup targetGroup = targetTreeViewItem != null ? targetTreeViewItem.DataContext as GlobalVariableGroup : null;
                try
                {
                    GlobalVariableGroup sourceGroup = (GlobalVariableGroup)e.Data.GetData("group");
                    if (targetGroup != null)
                    {
                        bool resetBinding = targetGroup.Children.IsEmpty();
                        if (e.KeyStates == DragDropKeyStates.ControlKey)
                            targetGroup.Children.Add(sourceGroup = sourceGroup.Clone());
                        else
                        {

                            ObservableCollection<GlobalVariableGroup> groups = this.getContainingCollection(sourceGroup);
                            groups.Remove(sourceGroup);
                            targetGroup.Children.Add(sourceGroup);
                        }

                        if (resetBinding)
                            BindingOperations.GetBindingExpressionBase(targetTreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
                        targetGroup.IsExpanded = true;
                        sourceGroup.IsSelected = true;
                        sourceGroup.IsExpanded = true;
                    }
                    else
                    {
                        ObservableCollection<GlobalVariableGroup> rootGroups = treeView.ItemsSource as ObservableCollection<GlobalVariableGroup>;
                        if (e.KeyStates == DragDropKeyStates.ControlKey)
                            rootGroups.Add(sourceGroup = sourceGroup.Clone());
                        else
                        {
                            ObservableCollection<GlobalVariableGroup> groups = this.getContainingCollection(sourceGroup);
                            groups.Remove(sourceGroup);
                            rootGroups.Add(sourceGroup);
                        }
                        sourceGroup.IsSelected = true;
                        sourceGroup.IsExpanded = true;
                    }
                }
                catch
                {
                }
            }
        }

        private void editTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            GlobalVariableGroup group = textBox.DataContext as GlobalVariableGroup;
            if (group == null)
                return;
            if (textBox.Visibility == Visibility.Visible)
            {
                Window window = Window.GetWindow(this);
                if (window.IsActive)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        Keyboard.Focus(textBox);
                        textBox.SelectAll();
                    }, DispatcherPriority.Render);
                }

                textBox.Background = Brushes.White;
                textBox.Text = group.Title;
                textBox.KeyDown += editTextBox_KeyDown;
                textBox.KeyUp += editTextBox_KeyUp;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text) && textBox.Tag != null && (int)textBox.Tag != 1)
                    group.Title = textBox.Text;
                textBox.KeyDown -= editTextBox_KeyDown;
                textBox.KeyUp -= editTextBox_KeyUp;
            }
        }

        private void editTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Tag = textBox.Text.Contains('.') ? 1 : 0;
            textBox.Background = (int)textBox.Tag == 1 ? Brushes.LightPink : Brushes.White;
        }

        private void editTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            GlobalVariableGroup group = textBox.DataContext as GlobalVariableGroup;
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text) && (int)textBox.Tag != 1)
                    group.Title = textBox.Text;
                else if ((int)textBox.Tag == 1)
                    MessageBox.Show("Name is invalid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                group.IsEditting = false;
            }
            else if (e.Key == Key.Escape)
            {
                textBox.Text = group.Title;
                group.IsEditting = false;
            }
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Name")
            {
                TextBox textBox = (e.EditingElement as TextBox);
                string s = textBox.Text;
                if (s.Contains("\"") || s.Contains("'") || s.Contains("."))
                {
                    textBox.Background = Brushes.LightPink;
                    MessageBox.Show("Please input valid parameter name", "Warning", MessageBoxButton.OK);
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Commands

        private void cutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void cutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            if (group != null)
            {
                _Clipboard = group;
                ObservableCollection<GlobalVariableGroup> groups = this.getContainingCollection(group);
                groups.Remove(group);
            }
        }
        private void dataGridCutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            e.CanExecute = !dataGrid.SelectedItems.IsEmpty();
            if (dataGrid.SelectedItems.Count == 1 && !(dataGrid.SelectedItem is GlobalVariable))
                e.CanExecute = false;
            e.Handled = true;
        }
        private void dataGridCutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<object> list = new List<object>();
            foreach (object o in (sender as DataGrid).SelectedItems)
                list.Add(o);
            _Clipboard = list;

            IList dataSource = (list[0] as GlobalVariable).Parent.Variables;
            foreach (GlobalVariable o in list)
            {
                dataSource.Remove(o);
            }
        }

        private void copyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void copyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _Clipboard = this.CurrentGroup;
        }
        private void dataGridCopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            e.CanExecute = !dataGrid.SelectedItems.IsEmpty();
            if (dataGrid.SelectedItems.Count == 1 && !(dataGrid.SelectedItem is GlobalVariable))
                e.CanExecute = false;
            e.Handled = true;
        }
        private void dataGridCopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<object> list = new List<object>();
            foreach (object o in (sender as DataGrid).SelectedItems)
                list.Add(o);
            _Clipboard = list;
        }

        private void pasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Clipboard != null
                && this.CurrentGroup != null
                && ((clipboardIsGroup() && this.CurrentGroup.Variables.IsEmpty() && this.CurrentGroup.DataTable == null)
                    || (clipboardIsVariables() && this.CurrentGroup.Children.IsEmpty() && this.CurrentGroup.DataTable == null));
        }
        private void pasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (clipboardIsGroup())
            {
                IList list = getContainingCollection(CurrentGroup);
                list.Add((_Clipboard as GlobalVariableGroup).Clone());
            }
            else if (clipboardIsVariables())
            {
                bool resetBinding = this.CurrentGroup.Variables.IsEmpty();
                foreach (var o in _Clipboard as IEnumerable)
                    if (o is GlobalVariable)
                        this.CurrentGroup.Variables.Add((o as GlobalVariable).Clone());

                if (resetBinding)
                    BindingOperations.GetBindingExpressionBase(treeView.Tag as DependencyObject, ItemsControl.ItemsSourceProperty).UpdateTarget();
                this.CurrentGroup.IsExpanded = true;
            }
        }
        private void dataGridPasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Clipboard != null && clipboardIsVariables();
        }
        private void dataGridPasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable list = _Clipboard as IEnumerable;
            if (list != null)
            {
                IList dataSource = (sender as DataGrid).ItemsSource as IList;
                foreach (object o in list)
                    if (o is GlobalVariable)
                        dataSource.Add((o as GlobalVariable).Clone());
            }
        }

        private void deleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void deleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            if (group != null)
            {
                IList groups = this.getContainingCollection(group);
                groups.Remove(group);
                Keyboard.Focus(treeView);
            }
        }

        private void moveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void moveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            ObservableCollection<GlobalVariableGroup> groups = this.getContainingCollection(group);
            int selectedIndex = groups.IndexOf(group);
            if (selectedIndex > 0)
                groups.Move(selectedIndex, selectedIndex - 1);
        }
        private void dataGridMoveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is GlobalVariable;
            e.Handled = true;
        }
        private void dataGridMoveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            GlobalVariable variable = grid.SelectedItem as GlobalVariable;
            GlobalVariableGroup group = variable.Parent;
            int selectedIndex = group.Variables.IndexOf(variable);
            if (selectedIndex > 0)
                group.Variables.Move(selectedIndex, selectedIndex - 1);
            Keyboard.Focus(grid);
        }

        private void moveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void moveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            ObservableCollection<GlobalVariableGroup> groups = this.getContainingCollection(group);
            int selectedIndex = groups.IndexOf(group);
            if (selectedIndex < groups.Count - 1)
                groups.Move(selectedIndex, selectedIndex + 1);
        }
        private void dataGridMoveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is GlobalVariable;
            e.Handled = true;
        }
        private void dataGridMoveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            GlobalVariable variable = grid.SelectedItem as GlobalVariable;
            GlobalVariableGroup group = variable.Parent;
            int selectedIndex = group.Variables.IndexOf(variable);
            if (selectedIndex < group.Variables.Count - 1)
                group.Variables.Move(selectedIndex, selectedIndex + 1);
            Keyboard.Focus(grid);
        }

        private void editGroup_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentGroup != null;
        }
        private void editGroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.CurrentGroup.IsEditting = true;
        }

        private void addGroup_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void addGroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup newGroup = new GlobalVariableGroup() { Title = "NewGroup" };
            GlobalVariableGroup group = this.CurrentGroup;
            if (group != null && group.Parent != null)
                group.Parent.Children.Add(newGroup);
            else
                (treeView.ItemsSource as ICollection<GlobalVariableGroup>).Add(newGroup);
            newGroup.IsSelected = true;
        }

        private void addChildGroup_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            e.CanExecute = (group != null && group.Variables.IsEmpty() && group.DataTable == null);
        }
        private void addChildGroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup newGroup = new GlobalVariableGroup() { Title = "NewGroup" };
            if (treeView.SelectedItem != null)
            {
                ICollection<GlobalVariableGroup> children = (treeView.SelectedItem as GlobalVariableGroup).Children;
                bool resetBinding = children.IsEmpty();
                children.Add(newGroup);
                if (resetBinding)
                    BindingOperations.GetBindingExpressionBase(treeView.Tag as TreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
                (treeView.SelectedItem as GlobalVariableGroup).IsExpanded = true;
            }
            else
                (treeView.ItemsSource as ICollection<GlobalVariableGroup>).Add(newGroup);
            newGroup.IsSelected = true;
        }

        private void addVariable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            e.CanExecute = (group != null && group.Children.IsEmpty() && group.DataTable == null);
        }
        private void addVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            GlobalVariable variable = new GlobalVariable() { Name = "variable" };
            bool resetBinding = group.Variables.IsEmpty();
            group.Variables.Add(variable);
            if (resetBinding)
                BindingOperations.GetBindingExpressionBase(treeView.Tag as TreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
            group.IsExpanded = true;
        }

        private void addTableVariable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            e.CanExecute = (group != null && group.Children.IsEmpty() && group.Variables.IsEmpty() && group.DataTable == null);
        }
        private void addTableVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalVariableGroup group = this.CurrentGroup;
            TableData dataTable = new TableData();
            dataTable.AddColumn();
            dataTable.AddColumn();
            dataTable.AddColumn();
            group.DataTable = dataTable;
            BindingOperations.GetBindingExpressionBase(treeView.Tag as TreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
            group.IsExpanded = true;
        }

        #endregion

        #region Methods

        private bool clipboardIsGroup()
        {
            return _Clipboard is GlobalVariableGroup;
        }

        private bool clipboardIsVariables()
        {
            if (!(_Clipboard is IList))
                return false;
            IList objs = _Clipboard as IList;
            if (objs.Count == 0 || !(objs[0] is GlobalVariable))
                return false;
            return true;
        }

        private ObservableCollection<GlobalVariableGroup> getContainingCollection(GlobalVariableGroup group)
        {
            ObservableCollection<GlobalVariableGroup> cases = group.Parent != null ? group.Parent.Children : (ObservableCollection<GlobalVariableGroup>)treeView.ItemsSource;
            return cases;
        }

        #endregion

        #region Properties

        private IEnumerable _ItemsSource;
        public IEnumerable ItemsSource
        {
            get
            {
                return _ItemsSource;
            }
            set
            {
                _ItemsSource = value;
                treeView.ItemsSource = value;
            }
        }

        private GlobalVariableGroup CurrentGroup
        {
            get
            {
                if (treeView == null)
                    return null;
                return treeView.SelectedItem as GlobalVariableGroup;
            }
        }

        #endregion
    }
}
