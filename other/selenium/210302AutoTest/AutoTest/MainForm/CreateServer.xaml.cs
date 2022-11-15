using AutoTest.Core;
using AutoTest.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace AutoTest
{
    public partial class CreateServer : Window
    {
        #region Contructors

        public CreateServer()
        {
            InitializeComponent();
            gridNetwork.CellEditEnding += grdNetwork_CellEditEnding;
            gridNetwork.RowEditEnding += gridNetwork_RowEditEnding;
            gridNetwork.SelectionChanged += gridNetwork_SelectionChanged;
            this.Loaded += CreateServer_Loaded;
        }
        public CreateServer(NetworkServer server) : this()
        {
            gridNetwork.ItemsSource = server.Model.Users;
        }

        #endregion

        #region Fields

        private NetworkServer _Server { get; set; }
        public bool IsServiceStarted { get; set; }

        #endregion

        #region Events

        void gridNetwork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        void gridNetwork_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            string userName = (e.Row.Item as User).UserName;
            if (userName.IsEmpty())
            {
                MessageBox.Show("Please, enter Username", "Warning", MessageBoxButton.OK);
                e.Cancel = true;
            }
            else
            {
                if (_Server.Model.Users.Any(f => string.Compare(f.UserName, userName, true) == 0))
                {
                    MessageBox.Show("Username already exists", "Warning", MessageBoxButton.OK);
                    e.Cancel = true;
                }
                else
                {
                    User user = _Server.Model.Users.Where(f => string.Compare(f.UserName, userName, true) == 0).FirstOrDefault();
                    if (user != null && user.SessionId.IsEmpty())
                        user.SessionId = Guid.NewGuid().ToString();
                }
            }
        }

        void grdNetwork_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "UserName")
            {
                string userName = ((User)e.Row.Item).UserName;
                if (!userName.IsEmpty())
                {
                    if (_Server.Model.Users.Any(f => string.Compare(f.UserName, userName, true) == 0))
                    {
                        MessageBox.Show("Username already exists", "Warning", MessageBoxButton.OK);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_Server == null)
                _Server = new NetworkServer(null, int.Parse(txbPort.Text.Trim()));
            if (IsServiceStarted)
                _Server.Start();
            else
            {
                _Server.Stop();
            }
        }

        void CreateServer_Loaded(object sender, RoutedEventArgs e)
        {
            IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(f => f.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            if (ip != null)
                txbName.Text = ip.ToString();

            if (_Server != null && IsServiceStarted)
                btnStart.IsEnabled = false;
            else
                btnStart.IsEnabled = true;
        }

        #endregion
    }
}