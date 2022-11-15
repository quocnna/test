using AutoTest.Data;
using AutoTest.UserControls;
using AutoTest.Core;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Net;
using System.Collections;
using System.ComponentModel;
using System.Windows.Shell;
using System.Windows.Interop;

namespace AutoTest
{
    public partial class Main : Window
    {
        private enum RunType { Run, RunCurrentStep, RunCurrentTestCase }

        #region Constructors

        public Main()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            this.setFileNameTitle(null);

            mnuFile_Exit.Click += mnuFile_Exit_Click;
            mnuStart_MarkStart.Click += mnuStart_MarkStart_Click;
            mnuStart_MarkEnd.Click += mnuStart_MarkEnd_Click;
            btnMarkStart.Click += mnuStart_MarkStart_Click;
            btnMarkEnd.Click += mnuStart_MarkEnd_Click;
            btnClearEndStep.Click += btnClearEndStep_Click;
            btnClearStarStep.Click += btnClearStarStep_Click;

            mnuAddServer.Click += mnuAddServer_Click;
            mnuConnectServer.Click += mnuConnectServer_Click;
            mnuNetwork_StartStopServer.Click += mnuNetwork_StartStopServer_Click;
            mnuNetwork_GetLast.Click += mnuNetwork_GetLast_Click;

            ucTestCase.OnTestCaseChanged += ucTestCase_OnTestCaseChanged;
            ucTestCase.OnStepChanged += ucTestCase_OnStepChanged;
            ucTestCase.OnActionChanged += ucTestCase_OnActionChanged;

            Loaded += delegate
            {
                Model = new TestModel();
                Keyboard.Focus(ucTestCase.treeTestCase);

                loadRecentMenu();
                Execute.MainWindowHandler = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            };

            Closing += delegate
            {
                if (_Server != null)
                    _Server.Stop();
            };
        }

        #endregion

        #region Fields

        private Thread _ThreadRun;
        private NetworkServer _Server;
        private NetworkClient _Client;
        private bool _IsServiceStarted;
        private bool _IsRunning;
        private Execute _Run = new Execute();

        private TestModel __Model;
        private TestModel Model
        {
            get { return __Model; }
            set
            {
                __Model = value;
                setFileNameTitle(Model.FileName);

                ucTestCase.Model = value;
                ucGlobalVariables.ItemsSource = value.GlobalVariables;
                ucBeforeRunning.GlobalVariables = value.GlobalVariables;
                ucAfterRunning.GlobalVariables = value.GlobalVariables;
                ucFunctionParas.TestCase = null;
                clearTextMark();

                if (_Server != null)
                    _Server.Stop();
                _Server = new NetworkServer(value);
            }
        }

        #endregion

        #region Events

        #region NENU

        void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        void mnuStart_MarkStart_Click(object sender, RoutedEventArgs e)
        {
            ucTestCase.MarkCurrentStepAsStart();
            if (Model.StartStep != null)
            {
                Binding bind = new Binding("Title");
                bind.Mode = BindingMode.OneWay;
                bind.Source = Model.StartStep;
                BindingOperations.SetBinding(txtMarkStart, Label.ContentProperty, bind);
            }
            else
                MessageBox.Show("You have to select a Step", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        void mnuStart_MarkEnd_Click(object sender, RoutedEventArgs e)
        {
            ucTestCase.MarkCurrentStepAsEnd();
            if (Model.EndStep != null)
            {
                Binding bind = new Binding("Title");
                bind.Mode = BindingMode.OneWay;
                bind.Source = Model.EndStep;
                BindingOperations.SetBinding(txtMarkEnd, Label.ContentProperty, bind);
            }
            else
                MessageBox.Show("You have to select a Step", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void mnuLoadUnitTests_Click(object sender, RoutedEventArgs e)
        {
            List<TestCase> testCases = ExternalFunction.ExternalFunction.LoadUnitTests();
            if (!testCases.IsEmpty())
                foreach (TestCase tc in testCases)
                    __Model.TestCases.Add(tc);
        }

        void mnuGlobalVariables_Click(object sender, RoutedEventArgs e)
        {
            mainTabButton_PreviewMouseLeftButtonDown(btnGlobalVariables, null);
        }

        void mnuLogFile_Click(object sender, RoutedEventArgs e)
        {
            btnTestCaseDetail_PreviewMouseLeftButtonDown(btnLog, null);
        }

        void mnuCheckPoints_Click(object sender, RoutedEventArgs e)
        {
            CheckPoints windowCheckPoints = new CheckPoints();
            windowCheckPoints.DataSource = Model.CheckPoints;
            windowCheckPoints.CheckPointsFolder = Model.CheckPointsFolder;
            windowCheckPoints.Owner = this;
            windowCheckPoints.Show();
        }

        #endregion

        #region TOOLBARS

        void btnClearEndStep_Click(object sender, RoutedEventArgs e)
        {
            BindingOperations.ClearBinding(txtMarkEnd, Label.ContentProperty);
            ucTestCase.ClearMarkEnd();
        }
        void btnClearStarStep_Click(object sender, RoutedEventArgs e)
        {
            BindingOperations.ClearBinding(txtMarkStart, Label.ContentProperty);
            ucTestCase.ClearMarkStart();
        }

        #endregion

        #region USER CONTROL

        void ucTestCase_OnTestCaseChanged(TestCase e)
        {
            ucFunctionParas.TestCase = e;
            ucAfterRunning.TestCase = e;
            ucBeforeRunning.TestCase = e;

            if (e != null && btnLog.IsChecked == true && ucLogFile.txtLog.LineCount > e.LogRowIndex)
                ucLogFile.txtLog.ScrollToLine(e.LogRowIndex);

            Binding bind = new Binding("Title");
            bind.Mode = BindingMode.OneWay;
            bind.Source = e;
            BindingOperations.SetBinding(lblTestCaseDetail, Label.ContentProperty, bind);
        }
        void ucTestCase_OnStepChanged(TestStep e)
        {
            refreshStepDetails();

            if (e != null && btnLog.IsChecked == true && ucLogFile.txtLog.LineCount > e.LogRowIndex)
                ucLogFile.txtLog.ScrollToLine(e.LogRowIndex);

            Binding bind = new Binding("Title");
            bind.Mode = BindingMode.OneWay;
            bind.Source = e;
            BindingOperations.SetBinding(lblStepDetail, Label.ContentProperty, bind);
        }
        void ucTestCase_OnActionChanged(ActionBase e)
        {
            refreshStepDetails();
        }
        void tabStepDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshStepDetails();
        }

        #endregion

        #region BUILD

        void mnuFile_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        #endregion

        #region NETWORK

        void mnuNetwork_StartStopServer_Click(object sender, RoutedEventArgs e)
        {
            _IsServiceStarted = !_IsServiceStarted;
            mnuNetwork_StartStopServer.Header = _IsServiceStarted ? "Stop Server" : "Start Server";
            if (_Server == null)
                _Server = new NetworkServer(Model);

            if (_IsServiceStarted)
                _Server.Start();
            else
                _Server.Stop();
        }
        void mnuNetwork_GetLast_Click(object sender, RoutedEventArgs e)
        {
            _Client.GetLast(Model);

            foreach (TestCase tc in Model.TestCases)
            {
                var o = ucTestCase.treeTestCase.ItemContainerGenerator.ContainerFromItem(tc);
                BindingOperations.GetBindingExpressionBase(o, ItemsControl.ItemsSourceProperty).UpdateTarget();
            }

        }
        void mnuConnectServer_Click(object sender, RoutedEventArgs e)
        {
            ConnectServer connectSever = new ConnectServer();
            if (_Client != null)
                connectSever.Client = _Client;
            connectSever.Owner = this;
            connectSever.ShowDialog();
            _Client = connectSever.Client;
        }
        void mnuAddServer_Click(object sender, RoutedEventArgs e)
        {
            CreateServer creatSever = new CreateServer(_Server);
            creatSever.Owner = this;
            creatSever.Show();
        }

        #endregion

        #region OTHERS

        private void window_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string file = files.Length > 0 ? files[0] : null;
                if (!string.IsNullOrEmpty(file))
                    this.OpenFile(file);
            }
        }

        private void mainTabButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as RadioButton).IsChecked = true;
            if (sender == btnTestCaseDesign)
            {
                ucTestCase.Visibility = Visibility.Visible;
                ucGlobalVariables.Visibility = Visibility.Collapsed;

                if (!toolBarTray.ToolBars.Contains(testCaseToolBar))
                    toolBarTray.ToolBars.Add(testCaseToolBar);
                if (toolBarTray.ToolBars.Contains(ucGlobalVariables.mainToolBar))
                    toolBarTray.ToolBars.Remove(ucGlobalVariables.mainToolBar);

                Keyboard.Focus(ucTestCase.treeTestCase);
            }
            else
            {
                ucTestCase.Visibility = Visibility.Collapsed;
                ucGlobalVariables.Visibility = Visibility.Visible;

                toolBarTray.ToolBars.Remove(testCaseToolBar);
                if (ucGlobalVariables.toolBarTray.ToolBars.Contains(ucGlobalVariables.mainToolBar))
                {
                    ucGlobalVariables.toolBarTray.ToolBars.Remove(ucGlobalVariables.mainToolBar);
                    ucGlobalVariables.mainToolBar.Margin = new Thickness(0);
                    foreach (object o in ucGlobalVariables.mainToolBar.Items)
                    {
                        if (o is Button)
                        {
                            Image img = (o as Button).Content as Image;
                            img.SetResourceReference(Image.StyleProperty, "toobarImage");
                        }
                    }
                    ucGlobalVariables.toolBarTray.Visibility = Visibility.Collapsed;
                }
                if (!toolBarTray.ToolBars.Contains(ucGlobalVariables.mainToolBar))
                    toolBarTray.ToolBars.Add(ucGlobalVariables.mainToolBar);

                Keyboard.Focus(ucGlobalVariables.treeView);
            }
            if (e != null)
                e.Handled = true;
        }

        private void btnStepDetail_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as RadioButton).IsChecked = true;
            refreshStepDetails();
        }

        private void btnTestCaseDetail_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as RadioButton).IsChecked = true;
        }

        #endregion

        #endregion

        #region Methods

        private void compile(Action<bool> doNext = null)
        {
            this.Cursor = Cursors.Wait;
            string error = null;
            bool isBefore = true;

            Task.Factory.StartNew(() =>
            {
                _ThreadRun = Thread.CurrentThread;
                _Run.Compile(ucTestCase.treeTestCase.ItemsSource as IEnumerable<TestCase>, out error, out isBefore);
            })
            .ContinueWith<bool>((res) =>
            {
                this.Cursor = null;
                if (!error.IsEmpty())
                {
                    if (isBefore)
                        btnBeforeRunning.IsChecked = true;
                    else
                        btnAfterRunning.IsChecked = true;
                    refreshStepDetails();
                    MessageBox.Show(this, error, "Compile Fail");
                }

                return error.IsEmpty();
            }, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith((res) =>
            {
                _ThreadRun = Thread.CurrentThread;
                if (doNext != null)
                    doNext(res.Result);
            });

        }
        private void run(RunType type)
        {
            Log.Init(ucLogFile.txtLog);
            _IsRunning = true;

            Action<bool> start = (compileResult) =>
            {
                if (compileResult)
                {
                    initGlobalTableVariables();
                    object runTarget = type == RunType.RunCurrentStep ? ucTestCase.CurrentStep :
                        type == RunType.RunCurrentTestCase ? (object)ucTestCase.CurrentTestCase : ucTestCase.treeTestCase.ItemsSource;

                    try
                    {
                        if (type == RunType.RunCurrentStep)
                            _Run.RunStep(Model, ucTestCase.CurrentStep);
                        else if (type == RunType.RunCurrentTestCase)
                            _Run.RunTestCase(Model, ucTestCase.CurrentTestCase);
                        else
                            _Run.Run(Model, Model.StartStep, Model.EndStep);

                        Dispatcher.Invoke(() => MessageBox.Show(this, "Completed.", "Runnning"));
                    }
                    catch (Exception ex)
                    {
                        if (ex is ThreadAbortException)
                            Dispatcher.Invoke(() => MessageBox.Show(this, "Test is stoped.", "STOP"));
                        else if (ex is ExitAndStopException || ex is ThreadAbortException)
                            Dispatcher.Invoke(() => MessageBox.Show(this, "Test is stoped. " + (ex.Message ?? ""), "STOP"));
                        else if (ex is EndStepException)
                            Dispatcher.Invoke(() => MessageBox.Show(this, "Completed.", "Runnning"));
                        else
                            Dispatcher.Invoke(() => MessageBox.Show(this, (ex.InnerException ?? ex ?? new Exception("Unknow error")).Message, "Error"));
                        _IsRunning = false;
                    }
                }
                _IsRunning = false;
            };

            compile(start);
        }
        private void initGlobalTableVariables()
        {
            foreach (GlobalVariableGroup e in Model.GlobalVariables)
                moveFirstGlobalTableVariable(e);
        }
        private void moveFirstGlobalTableVariable(GlobalVariableGroup variable)
        {
            if (variable.DataTable != null)
                variable.DataTable.MoveFirst();

            if (variable.Children != null)
                foreach (GlobalVariableGroup e in variable.Children)
                    moveFirstGlobalTableVariable(e);
        }

        private void refreshStepDetails()
        {
            TestStep step = ucTestCase.CurrentStep;
            if (btnStepParameters.IsChecked.HasValue && btnStepParameters.IsChecked.Value)
            {
                gridStepContent.Children.Clear();
                if (step != null && step.Action != null)
                    gridStepContent.Children.Add(step.Action.UI ?? new UserControl());
            }
            else if (btnBeforeRunning.IsChecked.HasValue && btnBeforeRunning.IsChecked.Value)
                ucBeforeRunning.Step = step;
            else if (btnAfterRunning.IsChecked.HasValue && btnAfterRunning.IsChecked.Value)
                ucAfterRunning.Step = step;
        }

        public void OpenFile(string filePath)
        {
            Loading loadingWindow = new Loading();
            try
            {
                this.Cursor = Cursors.Wait;

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    loadingWindow.Cursor = Cursors.Wait;
                    loadingWindow.Owner = this;
                    loadingWindow.ShowDialog();
                }));

                string logText = string.Empty;
                Task.Factory.StartNew<TestModel>(() =>
                {
                    TestModel model = loadXml(filePath, out logText);
                    model.FileName = filePath;
                    return model;
                }).ContinueWith(res =>
                {
                    this.Cursor = null;
                    loadingWindow.Close();
                    if (res.Exception == null)
                    {
                        Model = res.Result;
                        ucLogFile.txtLog.Text = logText;

                        RecentFileHelper.AddFile(filePath);
                        loadRecentMenu();
                        JumpTask recentTask = new JumpTask();
                        recentTask.Title = System.IO.Path.GetFileName(filePath);
                        recentTask.Arguments = filePath;
                        JumpList.AddToRecentCategory(recentTask);
                    }
                    else
                    {
                        if (res.Exception.InnerException is FileNotFoundException)
                        {
                            RecentFileHelper.RemoveFile(filePath);
                            loadRecentMenu();
                        }
                        MessageBox.Show(string.Format("Error while reading file", "Error.\n{0}", res.Exception.InnerException.Message), "Reading Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                this.Cursor = null;
                loadingWindow.Close();
                MessageBox.Show(string.Format("Cant open this file", "Error.\n{0}", ex.Message), "Reading Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void saveXmlToFile(string path, string log)
        {
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            xdoc.Add((Model as IPersistent).SaveXml());
            xdoc.Root.Add(new XElement("Log", new XCData(log)));
            xdoc.Save(path);
        }

        private TestModel loadXml(string path, out string log)
        {
            XDocument xDoc = XDocument.Load(path);
            XElement node = xDoc.Element("NeogovTest");
            TestModel model = new TestModel();
            (model as IPersistent).LoadXml(node);

            XElement logEle = node.Element("Log");
            if (logEle != null)
            {
                XNode cdataNode = logEle.FirstNode;
                if (logEle != null && cdataNode is XCData)
                    log = (cdataNode as XCData).Value;
                else
                    log = string.Empty;
            }
            else
                log = string.Empty;
            return model;
        }

        private TestStep markStep()
        {
            if (ucTestCase.CurrentStep != null)
                return ucTestCase.CurrentStep;
            else
                MessageBox.Show("You have to select a Step", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        private void saveAs()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".aut";
                dlg.Filter = "Data documents (.aut)|*.aut";
                if (dlg.ShowDialog() == true)
                {
                    this.Cursor = Cursors.Wait;
                    string path = dlg.FileName;
                    save(path);
                    setFileNameTitle(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = null;
            }
        }

        private void save(string path)
        {
            try
            {
                string logText = ucLogFile.txtLog.Text;
                Task.Factory.StartNew(() =>
                {
                    saveXmlToFile(path, logText);
                }).ContinueWith(res =>
                {
                    this.Cursor = null;
                    if (res.Exception != null)
                        MessageBox.Show(string.Format("Error while saving file", "Error.\n{0}", res.Exception.InnerException.Message), "Saving Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Model.FileName = path;
                        RecentFileHelper.AddFile(path);
                        loadRecentMenu();
                        MessageBox.Show("File was saved successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch
            {
            }
        }

        private void clearTextMark()
        {
            BindingOperations.ClearBinding(txtMarkStart, Label.ContentProperty);
            BindingOperations.ClearBinding(txtMarkEnd, Label.ContentProperty);

            if (Model.StartStep != null)
            {
                Binding bind = new Binding("Title");
                bind.Mode = BindingMode.OneWay;
                bind.Source = Model.StartStep;
                BindingOperations.SetBinding(txtMarkStart, Label.ContentProperty, bind);
            }
            if (Model.EndStep != null)
            {
                Binding bind = new Binding("Title");
                bind.Mode = BindingMode.OneWay;
                bind.Source = Model.EndStep;
                BindingOperations.SetBinding(txtMarkEnd, Label.ContentProperty, bind);
            }
        }

        private void setFileNameTitle(string fileName)
        {
            this.Title = "C04 AutoTest";
            if (!fileName.IsEmpty())
                this.Title += " (" + System.IO.Path.GetFileName(fileName) + " )";
        }

        private void loadRecentMenu()
        {
            int exitSepIndex = mnuFile.Items.IndexOf(sepFile_Exit);
            int exitMenuIndex = mnuFile.Items.IndexOf(mnuFile_Exit);
            for (int i = exitMenuIndex - 1; i > exitSepIndex; i--)
            {
                mnuFile.Items.RemoveAt(i);
            }
            List<string> recentFiles = RecentFileHelper.GetRecentFiles();
            if (recentFiles.Count > 0)
            {
                exitMenuIndex = mnuFile.Items.IndexOf(mnuFile_Exit);
                for (int i = 0; i < recentFiles.Count; i++)
                {
                    string file = recentFiles[i];
                    MenuItem recentMenu = new MenuItem
                    {
                        Header = string.Format("_{0} {1}", i + 1, file)
                    };
                    recentMenu.Click += delegate
                    {
                        this.OpenFile(file);
                    };
                    mnuFile.Items.Insert(exitMenuIndex + i, recentMenu);
                }
                mnuFile.Items.Insert(exitMenuIndex + recentFiles.Count, new Separator());
            }
        }

        #endregion

        #region Commands

        private void newProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model = new TestModel();
            ucLogFile.txtLog.Text = "";
        }
        private void openProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".aut";
            dlg.Filter = "Data Documents (.aut)|*.aut";
            if (dlg.ShowDialog() == true)
                this.OpenFile(dlg.FileName);
        }
        private void saveAsProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            saveAs();
        }
        private void saveProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Model.FileName))
                saveAs();
            else
                save(Model.FileName);
        }

        private void compileProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            compile();
        }

        private void runAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_IsRunning;
        }
        private void runAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            run(RunType.Run);
        }

        private void runCurrentStep_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_IsRunning;
        }
        private void runCurrentStep_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            run(RunType.RunCurrentStep);
        }

        private void runCurrentTestCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !_IsRunning;
        }
        private void runCurrentTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            run(RunType.RunCurrentTestCase);
        }

        private void stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _IsRunning;
        }
        private void stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_ThreadRun != null)
                _ThreadRun.Abort();
        }

        private void editTestCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucTestCase != null ? ucTestCase.CanEditTestCaseCommand : false;
        }
        private void editTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ucTestCase.ExecuteEditTestCaseCommand();
        }

        private void addTestCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucTestCase != null ? ucTestCase.CanAddTestCaseCommand : false;
        }
        private void addTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ucTestCase.ExecuteAddTestCaseCommand();
        }

        private void addChildTestCase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucTestCase != null ? ucTestCase.CanAddChildTestCaseCommand : false;
        }
        private void addChildTestCase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ucTestCase.ExecuteAddChildTestCaseCommand();
        }

        private void addAction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucTestCase != null ? ucTestCase.CanAddActionCommand : false;
        }
        private void addAction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ucTestCase.ExecuteAddActionCommand();
        }

        private void insertItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ucTestCase != null ? ucTestCase.CanInsertItemCommand : false;
        }
        private void insertItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ucTestCase.ExecuteInsertItemCommand();
        }

        #endregion
    }
}
