using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    public partial class LogFileUI : UserControl
    {
        #region Fields

        private int _index = 0;
        private string _keySearch = string.Empty;

        #endregion

        #region Contructors

        public LogFileUI()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.txtSearch.KeyDown += txtSearch_KeyDown;
            this.txtLog.KeyDown += txtLog_KeyDown;
            this.txtLog.KeyUp += txtLog_KeyUp;
        }

        #endregion

        #region Events

        void txtLog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                txtSearch.Focus();
        }

        void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                searchText();
        }

        void txtLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                searchText();
            }
            else
                txtSearch.Focus();
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchText();
        }

        #endregion

        #region Methods

        private void searchText()
        {
            bool isNewSearchKey = false;
            if (string.IsNullOrEmpty(_keySearch) || this.txtSearch.Text.ToLower() != _keySearch.ToLower())
            {
                _keySearch = this.txtSearch.Text;
                _index = 0;
                isNewSearchKey = true;
            }
            if (!string.IsNullOrEmpty(this.txtLog.Text))
            {
                _index = this.txtLog.Text.IndexOf(txtSearch.Text.ToLower(), _index + this.txtSearch.Text.Length, StringComparison.OrdinalIgnoreCase);
                if (_index > 0)
                {
                    txtLog.SelectionStart = _index;
                    txtLog.SelectionLength = txtSearch.Text.Length;
                    txtLog.Focus();
                }
                if (isNewSearchKey && _index < 0)
                    MessageBox.Show("No Result");
            }
        }

        #endregion
    }
}
