using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using log4net;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
       
        }

        public async Task UploadFilesAsync(string folderPath)
        {
            ConsoleWriteLine(folderPath);
            var files = Directory.GetFiles(folderPath);

            await Task.Run(() =>
            {
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async (file) =>
                {
                    // Upload the file using your preferred method
                    await UploadFileAsync(file);
                    ConsoleWriteLine(folderPath);
                });
            });
        }

        private async Task UploadFileAsync(string filePath)
        {
            // Implement your file upload logic here
            ConsoleWriteLine(filePath);
        }

        static void ConsoleWriteLine(string str)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            log.Info("th:"+ threadId);

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            log.Info("aaaa");
            await UploadFilesAsync("D:\\test_upload");
        }

        //private void UploadFolder(string folderPath, int numThreads)
        //{

        //    var fileNames = Directory.GetFiles(folderPath);
        //    var chunks = Chunkify(fileNames, numThreads);

        //    var semaphore = new SemaphoreSlim(numThreads);
        //    var completionEvents = new List<ManualResetEvent>();

        //    foreach (var chunk in chunks)
        //    {
        //        semaphore.Wait();

        //        var completionEvent = new ManualResetEvent(false);
        //        completionEvents.Add(completionEvent);

        //        var thread = new Thread(() =>
        //        {
        //            try
        //            {
        //                UploadFiles(chunk);
        //            }
        //            finally
        //            {
        //                semaphore.Release();
        //                completionEvent.Set();
        //            }
        //        });

        //        thread.SetApartmentState(ApartmentState.STA);
        //        thread.Start();

        //    }


        //    // Wait for all threads to complete
        //    WaitHandle.WaitAll(completionEvents.ToArray());

        //    // Update UI with completion message
        //    MessageBox.Show("Upload complete!");
        //}

        //private IEnumerable<string[]> Chunkify(string[] items, int numChunks)
        //{
        //    var chunkSize = (int)Math.Ceiling((double)items.Length / numChunks);

        //    return Enumerable.Range(0, numChunks)
        //        .Select(i => items.Skip(i * chunkSize).Take(chunkSize).ToArray())
        //        .Where(chunk => chunk.Length > 0);
        //}

        //private void UploadFiles(string[] fileNames)
        //{
        //    // Upload logic goes here
        //}
    }




}
