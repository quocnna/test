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

namespace AutoTest.DBAction
{
    /// <summary>
    /// Interaction logic for DBActionContent.xaml
    /// </summary>
    public partial class DBActionContent : UserControl
    {
        public DBActionContent()
        {
            InitializeComponent();
        }

        #region Properties

        public Dictionary<string, object> Data { get; set; }

        #endregion

    }
}
