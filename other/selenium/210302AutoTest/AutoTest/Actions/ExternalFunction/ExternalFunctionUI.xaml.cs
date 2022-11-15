using AutoTest.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoTest.ExternalFunction
{
    public partial class ExternalFunctionUI : UserControl
    {
        public ExternalFunctionUI()
        {
            InitializeComponent();
            cmbAssembly.SelectionChanged += cmbAssembly_SelectionChanged;
            cmbClass.SelectionChanged += cmbClass_SelectionChanged;
            cmbFunction.SelectionChanged += cmbFunction_SelectionChanged;
        }

        #region Fields

        private bool _IsBinding;
        private ExternalFunction _Instance;
        private ExternalFunctionState _State;

        #endregion

        #region Events

        void cmbAssembly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssemblyWarp assemblyInfo = cmbAssembly.SelectedItem as AssemblyWarp;
            cmbClass.ItemsSource = assemblyInfo != null ? assemblyInfo.Classes.Values : null;
            cmbFunction.ItemsSource = null;
            if (!_IsBinding)
            {
                _State.AssemblyName = assemblyInfo != null ? assemblyInfo.Key : null;
                dataGrid.ItemsSource = _State.Variables = null;
            }
        }

        void cmbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassWarp classInfo = cmbClass.SelectedItem as ClassWarp;
            cmbFunction.ItemsSource = classInfo != null ? classInfo.Functions.Values : null;
            if (!_IsBinding)
            {
                _State.ClassName = classInfo != null ? classInfo.Key : null;
                dataGrid.ItemsSource = _State.Variables = null;
            }
        }

        void cmbFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FunctionWarp funcInfo = cmbFunction.SelectedItem as FunctionWarp;
            if (!_IsBinding)
            {
                _State.FunctionName = funcInfo != null ? funcInfo.Key : null;
                dataGrid.ItemsSource = _State.Variables = funcInfo != null ? funcInfo.Paras.ConvertAll<Variable>(p => new Variable() { Name = p }) : null;
            }
        }

        #endregion

        #region Methods

        internal void Bind(ExternalFunction instance)
        {
            var temp = _Instance;
            _Instance = instance;
            if (object.Equals(temp, instance) && object.Equals(temp.State, instance.State))
                return;

            _IsBinding = true;
            _Instance = instance;
            _State = (ExternalFunctionState)instance.State;

            cmbAssembly.Visibility = _State.IsUnitTest ? Visibility.Collapsed : Visibility.Visible;
            cmbClass.Visibility = _State.IsUnitTest ? Visibility.Collapsed : Visibility.Visible;
            cmbFunction.Visibility = _State.IsUnitTest ? Visibility.Collapsed : Visibility.Visible;
            lblAssembly.Visibility = _State.IsUnitTest ? Visibility.Visible : Visibility.Collapsed;
            lblClass.Visibility = _State.IsUnitTest ? Visibility.Visible : Visibility.Collapsed;
            lblFunction.Visibility = _State.IsUnitTest ? Visibility.Visible : Visibility.Collapsed;
            if (_State.IsUnitTest)
            {
                lblAssembly.Text = _State.AssemblyName;
                lblClass.Text = _State.ClassName;
                lblFunction.Text = _State.FunctionName;
                cmbAssembly.ItemsSource = null;
                cmbClass.ItemsSource = null;
                cmbFunction.ItemsSource = null;
            }
            else
            {
                cmbAssembly.ItemsSource = ExternalFunction.AssembliesLib.Values;
                cmbAssembly.SelectedValue = null;
                cmbClass.ItemsSource = null;
                cmbFunction.ItemsSource = null;

                if (!string.IsNullOrWhiteSpace(_State.AssemblyName))
                {
                    cmbAssembly.SelectedValue = _State.AssemblyName;
                    cmbClass.SelectedValue = _State.ClassName;
                    cmbFunction.SelectedValue = _State.FunctionName;
                }
            }
            dataGrid.ItemsSource = _State.Variables;
            _IsBinding = false;
        }

        #endregion
    }
}
