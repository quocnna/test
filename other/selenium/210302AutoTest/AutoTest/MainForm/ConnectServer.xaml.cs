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

namespace AutoTest
{
    public partial class ConnectServer : Window
    {
        public NetworkClient Client { get; set; }

        private static string _ServerName;

        public ConnectServer()
        {
            InitializeComponent();

            btnClose.Click += btnClose_Click;
            btnConnect.Click += btnConnect_Click;
            this.Loaded += ConnectServer_Loaded;
        }

        void ConnectServer_Loaded(object sender, RoutedEventArgs e)
        {
            txtServer.Text = _ServerName;
            if (Client != null)
            {
                btnConnect.Content = "Disconnect";
                txtServer.IsEnabled = false;
                txtPort.IsEnabled = false;
            }
        }

        void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Client == null)
            {
                if (txtServer.Text.Trim().IsEmpty())
                {
                    MessageBox.Show("Please, enter Connect Server", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtServer.Focus();
                    return;
                }
                if (txtPort.Text.Trim().IsEmpty())
                {
                    MessageBox.Show("Please, enter Port", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPort.Focus();
                    return;
                }
                else
                {
                    int number = 0;
                    if (!int.TryParse(txtPort.Text, out _))
                    {
                        MessageBox.Show("Port must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtPort.Focus();
                        return;
                    }
                }
                _ServerName = txtServer.Text.Trim();
                Client = new NetworkClient(txtServer.Text, int.Parse(txtPort.Text));
                string message = Client.Login(txtUsername.Text, txtPassword.Password);
                if (message.IsEmpty())
                    this.Close();
                else
                {
                    MessageBox.Show(message, "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Client = null;
                }
            }
            else
            {
                btnConnect.IsEnabled = true;
                btnConnect.Content = "Connect";
                txtServer.IsEnabled = true;
                txtPort.IsEnabled = true;
                Client = null;
            }
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
