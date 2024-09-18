using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DragDropExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> DownloadList => [
            "https://link.testfile.org/30MB",
            "https://link.testfile.org/60MB",
            "https://link.testfile.org/15MB",
            "https://link.testfile.org/150MB",
            "https://link.testfile.org/500MB",
            "https://link.testfile.org/5GB-zip",
            "https://link.testfile.org/10GB",
        ];
        public ObservableCollection<string> Downloads { get; } = [];


        public MainWindow()
        {
            InitializeComponent();

            foreach (var dwnList in DownloadList)
            {
                Downloads.Add(dwnList);
            }
        }
        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ListBox.SelectedItem is string str)
            {
                
            }
        }

        private void DownloadFileToStream(string url,ref Stream stream)
        {
            using (var webClient = new WebClient())
            {
                // Scarica il file e scrivi direttamente nello stream
                var data = webClient.DownloadData(new Uri(url));
                stream.Write(data, 0, data.Length);
            }
        }
    }
}