using AutoTest.Core;
using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace AutoTest.UserControls
{
    public delegate void TestStepChangedHandler(TestStep e);
    public delegate void TestCaseChangedHandler(TestCase e);
    public delegate void ActionChangedHandler(ActionBase e);

    public partial class TestCaseUI : UserControl
    {
        #region Constructors

        public TestCaseUI()
        {
            initActions();
            InitializeComponent();
        }

        #endregion

        #region Fields

        Point _StartPoint;
        bool _IsDragging;

        object _Clipboard;
        DataGrid _PrevSelectedDataGrid;
        DataGridRow _StartRow;
        DataGridRow _EndRow;
        DataGridRow _CurrentRow;

        #endregion

        #region Properties

        private TestModel _Model;
        public TestModel Model
        {
            get { return _Model; }
            set
            {
                _Clipboard = null;
                _PrevSelectedDataGrid = null;
                _StartRow = null;
                _EndRow = null;
                _CurrentRow = null;
                _IsDragging = false;
                _Model = value;
                if (treeTestCase.SelectedItem != null && treeTestCase.SelectedItem is TestCase)
                    (treeTestCase.SelectedItem as TestCase).IsSelected = false;
                treeTestCase.ItemsSource = _Model.TestCases;
                CurrentTestCase = null;
                CurrentStep = null;
            }
        }

        public bool IsBinding { get; set; }

        private TestStep _CurrentStep;
        public TestStep CurrentStep
        {
            get { return _CurrentStep; }
            set
            {
                if (_CurrentStep != value)
                {
                    _CurrentStep = value;
                    if (OnStepChanged != null)
                        OnStepChanged(value);
                }
            }
        }

        private TestCase _CurrentTestCase;
        public TestCase CurrentTestCase
        {
            get { return _CurrentTestCase; }
            set
            {
                if (_CurrentTestCase != value)
                {
                    _CurrentTestCase = value;
                    if (OnTestCaseChanged != null)
                        OnTestCaseChanged(value);
                }
            }
        }

        #endregion

        #region Commands

        private void cutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null;
        }
        private void cutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TestCase testCase = this.CurrentTestCase;
            if (testCase != null)
            {
                _Clipboard = testCase;
                ObservableCollection<TestCase> cases = this.getContainingCollection(testCase);
                cases.Remove(testCase);

                Keyboard.Focus(treeTestCase);
                TestCase tc = treeTestCase.SelectedItem as TestCase;
                if (tc != null)
                {
                    tc.IsSelected = false;
                    tc.IsSelected = true;
                }
            }
        }
        private void dataGridCutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentStep != null;
            e.Handled = true;
        }
        private void dataGridCutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<object> list = new List<object>();
            foreach (object o in (sender as DataGrid).SelectedItems)
                list.Add(o);
            _Clipboard = list;

            foreach (TestStep o in list)
                o.Parent.Steps.Remove(o);
        }

        private void copyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null;
        }
        private void copyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _Clipboard = this.CurrentTestCase;
        }
        private void dataGridCopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentStep != null;
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
                && this.CurrentTestCase != null
                && ((_Clipboard is TestCase && this.CurrentTestCase.Steps.IsEmpty())
                    || (_Clipboard is IEnumerable && this.CurrentTestCase.Children.IsEmpty()));
        }
        private void pasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_Clipboard is TestCase)
            {
                IList list = this.CurrentTestCase == null || this.CurrentTestCase.Parent == null ? treeTestCase.ItemsSource as IList : this.CurrentTestCase.Parent.Children;
                list.Add((_Clipboard as TestCase).Clone());
            }
            else if (_Clipboard is IEnumerable && this.CurrentTestCase != null)
            {
                bool resetBinding = this.CurrentTestCase.Children.IsEmpty();
                foreach (var o in _Clipboard as IEnumerable)
                    if (o is TestStep)
                        this.CurrentTestCase.Steps.Add((o as TestStep).Clone());

                if (resetBinding)
                    BindingOperations.GetBindingExpressionBase(treeTestCase.Tag as DependencyObject, ItemsControl.ItemsSourceProperty).UpdateTarget();
                this.CurrentTestCase.IsExpanded = true;
            }
        }
        private void dataGridPasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Clipboard is IEnumerable;
            e.Handled = true;
        }
        private void dataGridPasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable list = _Clipboard as IEnumerable;
            if (list != null)
            {
                IList dataSource = (sender as DataGrid).ItemsSource as IList;
                foreach (object o in list)
                    if (o is TestStep)
                        dataSource.Add((o as TestStep).Clone());
            }
        }

        private void deleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null;
        }
        private void deleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TestCase testCase = this.CurrentTestCase;
            if (testCase != null)
            {
                IList cases = this.getContainingCollection(testCase);
                cases.Remove(testCase);
                CurrentTestCase = null;
                Keyboard.Focus(treeTestCase);
                TestCase tc = treeTestCase.SelectedItem as TestCase;
                if (tc != null)
                {
                    tc.IsSelected = false;
                    tc.IsSelected = true;
                }
            }
        }

        private void moveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null;
        }
        private void moveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TestCase testCase = this.CurrentTestCase;
            ObservableCollection<TestCase> cases = this.getContainingCollection(testCase);
            int selectedIndex = cases.IndexOf(testCase);
            if (selectedIndex > 0)
                cases.Move(selectedIndex, selectedIndex - 1);
        }
        private void dataGridMoveUpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is TestStep;
            e.Handled = true;
        }
        private void dataGridMoveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep testStep = grid.SelectedItem as TestStep;
            TestCase testCase = testStep.Parent;
            int selectedIndex = testCase.Steps.IndexOf(testStep);
            if (selectedIndex > 0)
                testCase.Steps.Move(selectedIndex, selectedIndex - 1);
            Keyboard.Focus(grid);
            grid.SelectedItems.Clear();
            grid.SelectedItem = null;
        }

        private void moveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null;
        }
        private void moveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TestCase testCase = this.CurrentTestCase;
            ObservableCollection<TestCase> cases = this.getContainingCollection(testCase);
            int selectedIndex = cases.IndexOf(testCase);
            if (selectedIndex < cases.Count - 1)
                cases.Move(selectedIndex, selectedIndex + 1);
        }
        private void dataGridMoveDownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            e.CanExecute = grid.SelectedItem != null && grid.SelectedItem is TestStep;
            e.Handled = true;
        }
        private void dataGridMoveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep testStep = grid.SelectedItem as TestStep;
            TestCase testCase = testStep.Parent;
            int selectedIndex = testCase.Steps.IndexOf(testStep);
            if (selectedIndex < testCase.Steps.Count - 1)
                testCase.Steps.Move(selectedIndex, selectedIndex + 1);
            Keyboard.Focus(grid);
            grid.SelectedItems.Clear();
            grid.SelectedItem = null;
        }

        private void lock_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null && this.CurrentTestCase.Status != TestStatus.Blocked;
        }
        private void lock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.CurrentTestCase.Status = TestStatus.Blocked;
        }
        private void dataGridLock_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep step = grid.SelectedItem as TestStep;
            e.CanExecute = step != null && step.Status != TestStatus.Blocked;
            e.Handled = true;
        }
        private void dataGridLock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep step = grid.SelectedItem as TestStep;
            step.Status = TestStatus.Blocked;
        }

        private void unlock_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.CurrentTestCase != null && this.CurrentTestCase.Status == TestStatus.Blocked;
        }
        private void unlock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.CurrentTestCase.Status = TestStatus.Ready;
        }
        private void dataGridUnlock_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep step = grid.SelectedItem as TestStep;
            e.CanExecute = step != null && step.Status == TestStatus.Blocked;
            e.Handled = true;
        }
        private void dataGridUnlock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            TestStep step = grid.SelectedItem as TestStep;
            step.Status = TestStatus.Ready;
        }

        private void dataGridAddAction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void dataGridAddAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            IList<TestStep> steps = (grid.DataContext as TestStepsWrapper).Steps;
            TestStep selectedStep = steps.FirstOrDefault(s => s.IsSelected);
            if (selectedStep != null)
                selectedStep.IsSelected = false;

            TestStep step = new TestStep() { Title = "New action..." };
            steps.Add(step);
            step.IsSelected = true;
        }

        public bool CanEditTestCaseCommand
        {
            get
            {
                return this.CurrentTestCase != null;
            }
        }
        public void ExecuteEditTestCaseCommand()
        {
            this.CurrentTestCase.IsEditting = true;
        }

        public bool CanAddTestCaseCommand
        {
            get
            {
                return true;
            }
        }
        public void ExecuteAddTestCaseCommand()
        {
            TestCase newTestCase = new TestCase() { Title = "New Test Case" };
            getContainingCollection(this.CurrentTestCase).Add(newTestCase);

            newTestCase.IsSelected = true;
            treeTestCase.Focus();
        }

        public bool CanAddChildTestCaseCommand
        {
            get
            {
                TestCase testCase = this.CurrentTestCase;
                return testCase == null || testCase.Steps.IsEmpty();
            }
        }
        public void ExecuteAddChildTestCaseCommand()
        {
            TestCase newTestCase = new TestCase() { Title = "New Test Case" };
            if (treeTestCase.SelectedItem != null)
            {
                ICollection<TestCase> children = (treeTestCase.SelectedItem as TestCase).Children;
                bool resetBinding = children.IsEmpty();
                children.Add(newTestCase);
                if (resetBinding)
                    BindingOperations.GetBindingExpressionBase(treeTestCase.Tag as TreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
                (treeTestCase.SelectedItem as TestCase).IsExpanded = true;
            }
            else
                (treeTestCase.ItemsSource as ICollection<TestCase>).Add(newTestCase);
            newTestCase.IsSelected = true;
        }

        public bool CanAddActionCommand
        {
            get
            {
                TestCase testCase = this.CurrentTestCase;
                if (testCase == null || !testCase.Children.IsEmpty())
                    return false;
                return true;
            }
        }
        public void ExecuteAddActionCommand(bool isInsert = false)
        {
            TestCase testCase = this.CurrentTestCase;
            TestStep step = new TestStep() { Title = "New action..." };
            TestStep selectedStep = testCase.Steps.FirstOrDefault(s => s.IsSelected);
            if (selectedStep != null)
                selectedStep.IsSelected = false;

            bool resetBinding = testCase.Steps.IsEmpty();
            if (!isInsert || selectedStep == null)
                testCase.Steps.Add(step);
            else
                testCase.Steps.Insert(testCase.Steps.IndexOf(selectedStep), step);

            if (resetBinding)
                BindingOperations.GetBindingExpressionBase(treeTestCase.Tag as TreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
            testCase.IsExpanded = true;
            step.IsSelected = true;
        }

        public bool CanInsertItemCommand
        {
            get
            {
                return true;
            }
        }
        public void ExecuteInsertItemCommand()
        {
            TestCase testCase = this.CurrentTestCase;
            if (testCase != null && !testCase.Steps.IsEmpty())
                this.ExecuteAddActionCommand(true);
            else
            {
                if (testCase != null)
                    this.ExecuteAddChildTestCaseCommand();
                else
                    this.ExecuteAddTestCaseCommand();
            }
        }

        #endregion

        #region Events

        public event TestStepChangedHandler OnStepChanged;
        public event TestCaseChangedHandler OnTestCaseChanged;
        public event ActionChangedHandler OnActionChanged;

        void treeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid)
                Keyboard.Focus(treeTestCase);
        }

        void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            if (targetTreeViewItem != null)
                _StartPoint = e.GetPosition(null);
        }

        void treeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_IsDragging)
            {
                if (e.OriginalSource is TextBox)
                    return;
                TreeViewItem sourceTreeViewItem = UIHelper.GetAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (sourceTreeViewItem == null || sourceTreeViewItem.DataContext is TestStepsWrapper)
                    return;

                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - _StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    _IsDragging = true;
                    DataObject data = null;
                    if (treeTestCase.SelectedItem is TestCase)
                        data = new DataObject("testCase", new DragDropData(treeTestCase.SelectedItem as TestCase, new WindowInteropHelper(Application.Current.MainWindow).Handle.ToInt32()));
                    if (data != null)
                    {
                        DragDropEffects dde = DragDropEffects.Copy | DragDropEffects.Move;
                        if (e.RightButton == MouseButtonState.Pressed)
                            dde = DragDropEffects.All;
                        _ = DragDrop.DoDragDrop(treeTestCase, data, dde);
                    }
                    _IsDragging = false;
                }
            }
        }

        void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("testCase"))
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            }
            else
            {
                try
                {
                    DragDropData dragData = (DragDropData)e.Data.GetData("testCase");
                    TestCase sourceCase = dragData.TestCase;
                    TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                    TestCase targetCase = targetTreeViewItem != null ? targetTreeViewItem.DataContext as TestCase : null;
                    if (targetCase != null)
                    {
                        if (sourceCase == targetCase || (targetCase == sourceCase.Parent && (e.KeyStates & DragDropKeyStates.ControlKey) != DragDropKeyStates.ControlKey) || !targetCase.Steps.IsEmpty())
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
                        }
                        else
                        {
                            TestCase parent = targetCase.Parent;
                            bool sourceIsParentOfTarget = parent == sourceCase;
                            while (!sourceIsParentOfTarget && parent != null)
                            {
                                parent = parent.Parent;
                                sourceIsParentOfTarget = parent == sourceCase;
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
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        void treeView_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (data.GetDataPresent("testCase"))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                TreeViewItem targetTreeViewItem = UIHelper.GetAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                TestCase targetCase = targetTreeViewItem != null ? targetTreeViewItem.DataContext as TestCase : null;
                try
                {
                    DragDropData dragData = (DragDropData)e.Data.GetData("testCase");
                    TestCase sourceCase = dragData.TestCase;
                    TestCaseCollection targetCollection = targetCase == null ? treeTestCase.ItemsSource as TestCaseCollection : targetCase.Children;

                    bool resetBinding = targetCase != null && targetCase.Children.IsEmpty();

                    bool isDragFromDifferentWindow = dragData.UniqueId != new WindowInteropHelper(Application.Current.MainWindow).Handle.ToInt32();
                    bool isCopy = e.KeyStates == DragDropKeyStates.ControlKey;
                    if (isCopy || isDragFromDifferentWindow)
                        targetCollection.Add(sourceCase = sourceCase.Clone());
                    else
                    {
                        TestCaseCollection sourceCollection = getContainingCollection(sourceCase);
                        sourceCollection.MoveToOtherCollection(sourceCase, targetCollection);
                    }

                    if (targetCase != null)
                    {
                        targetCase.IsExpanded = true;
                        if (resetBinding)
                            BindingOperations.GetBindingExpressionBase(targetTreeViewItem, ItemsControl.ItemsSourceProperty).UpdateTarget();
                    }
                    sourceCase.IsSelected = true;
                    sourceCase.IsExpanded = true;
                }
                catch
                {
                }
            }
        }

        void treeView_ItemSelected(object sender, RoutedEventArgs e)
        {
            if (treeTestCase.SelectedItem is TestStepsWrapper)
            {
                TestStepsWrapper stepsWrapper = treeTestCase.SelectedItem as TestStepsWrapper;
                stepsWrapper.IsSelected = false;
                if (!stepsWrapper.Steps.IsEmpty())
                    stepsWrapper.Steps[0].Parent.IsSelected = true;
            }
            else if (treeTestCase.SelectedItem is TestCase)
            {
                treeTestCase.Tag = e.OriginalSource;
                this.CurrentTestCase = treeTestCase.SelectedItem as TestCase;
            }
        }

        private void stepsDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            CurrentStep = grid.SelectedItem as TestStep;
        }

        private void stepsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (_PrevSelectedDataGrid != null && _PrevSelectedDataGrid != sender)
                ((DataGrid)_PrevSelectedDataGrid).SelectedItem = null;
            _PrevSelectedDataGrid = sender as DataGrid;
            CurrentStep = ((DataGrid)sender).SelectedItem as TestStep;
            _CurrentRow = ((DataGrid)sender).ItemContainerGenerator.ContainerFromItem(CurrentStep) as DataGridRow;
        }

        private void cmbActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            TestStep step = c.DataContext as TestStep;
            if (step != null && c.SelectedItem != null && !object.Equals(step.Action, c.SelectedItem))
            {
                step.Action = Activator.CreateInstance(c.SelectedItem.GetType()) as ActionBase;
                if (OnActionChanged != null)
                    this.OnActionChanged(step.Action);
            }
            e.Handled = true;
        }

        private void editTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            TestCase tc = textBox.DataContext as TestCase;
            if (textBox.Visibility == Visibility.Visible)
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    Keyboard.Focus(textBox);
                    textBox.SelectAll();
                }, DispatcherPriority.Render);

                textBox.Text = tc.Title;
                textBox.KeyDown += editTextBox_KeyDown;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                    tc.Title = textBox.Text;
                textBox.KeyDown -= editTextBox_KeyDown;
            }
        }
        private void editTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            TestCase tc = textBox.DataContext as TestCase;
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                    tc.Title = textBox.Text;
                tc.IsEditting = false;
            }
            else if (e.Key == Key.Escape)
            {
                textBox.Text = tc.Title;
                tc.IsEditting = false;
            }
        }

        #endregion

        #region Methods

        public void MarkCurrentStepAsStart()
        {
            if (_StartRow != null)
            {
                _StartRow.Background = Brushes.White;
                _StartRow = null;
            }
            if (this.CurrentStep != null && _CurrentRow != null)
            {
                _CurrentRow.Background = Brushes.LightBlue;
                _StartRow = _CurrentRow;
                _Model.StartStep = this.CurrentStep;
            }
        }

        public void MarkCurrentStepAsEnd()
        {
            if (_EndRow != null)
            {
                _EndRow.Background = Brushes.White;
                _EndRow = null;
            }
            if (this.CurrentStep != null && _CurrentRow != null)
            {
                _CurrentRow.Background = Brushes.LightPink;
                _EndRow = _CurrentRow;
                _Model.EndStep = this.CurrentStep;
            }
        }

        public void ClearMarkStart()
        {
            if (_StartRow != null)
            {
                _StartRow.Background = Brushes.White;
                _StartRow = null;
            }
            _Model.StartStep = null;
        }

        public void ClearMarkEnd()
        {
            if (_EndRow != null)
            {
                _EndRow.Background = Brushes.White;
                _EndRow = null;
            }
            _Model.EndStep = null;
        }

        private void initActions()
        {
            var list = new ObservableCollection<ActionBase>();
            Resources.Add("ActionList", list);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                list.Add(new AutoTest.WebAction.WebAction());
                list.Add(new CallTestAction());
                list.Add(new AutoTest.ExternalFunction.ExternalFunction());
                list.Add(new AutoTest.DBAction.DBAction());
            }

        }

        private TestCaseCollection getContainingCollection(TestCase testCase)
        {
            return testCase != null && testCase.Parent != null ? testCase.Parent.Children : (TestCaseCollection)treeTestCase.ItemsSource;
        }

        #endregion

        #region Inner class

        [Serializable]
        private class DragDropData
        {
            public DragDropData(TestCase testCase, int uniqueId)
            {
                this.TestCase = testCase;
                this.UniqueId = uniqueId;
            }

            public TestCase TestCase { get; set; }
            public int UniqueId { get; private set; }
        }

        #endregion
    }
}
