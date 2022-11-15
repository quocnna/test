using System.Reflection;
using System.Windows;

namespace AutoTest
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            lblVersion.Text = string.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
