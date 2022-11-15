using AutoTest.Data;
using System;
using System.Collections;
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

namespace AutoTest.UserControls
{

    public partial class TestCaseDetailUI : UserControl
    {
        #region Constructors

        public TestCaseDetailUI()
        {
            InitializeComponent();

            chkIsAllowCall.Click += delegate
            {
                ucVariable.ShowColumnPublic = (TestCase.IsFunction = chkIsAllowCall.IsChecked == true);
            };
        }

        #endregion

        #region Properites

        private TestCase _TestCase;
        public TestCase TestCase
        {
            get { return _TestCase; }
            set
            {
                this.Visibility = value == null ? Visibility.Hidden : Visibility.Visible;
                if (value != null)
                {
                    _TestCase = value;
                    chkIsAllowCall.IsChecked = ucVariable.ShowColumnPublic = value.IsFunction;
                    ucVariable.ItemsSource = TestCase.Data;
                }
            }
        }

        #endregion
    }
}
