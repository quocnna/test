using System.Windows;

namespace MainForm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AutoTest.Main mainWindow = new AutoTest.Main();
            mainWindow.Show();
            if (e.Args.Length > 0)
                mainWindow.OpenFile(e.Args[0]);
        }
    }
}
