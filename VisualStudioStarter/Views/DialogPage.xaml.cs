using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using UserControl = System.Windows.Controls.UserControl;

namespace VisualStudioStarter.Views
{
    /// <summary>
    /// Interaction logic for DialogPage.xaml
    /// </summary>
    public partial class DialogPage : INotifyPropertyChanged
    {
        private string _solutionName;

        public string SolutionName
        {
            get => _solutionName;
            set => SetField(ref _solutionName, value);
        }

        public DialogPage(string solutionName)
        {
            InitializeComponent();
            SolutionName = solutionName;
        }

        private void BtnYes_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.GetDialogSession("SolutionDialogBox")?.Close(DialogResult.Yes);
        }

        private void BtnNo_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.GetDialogSession("SolutionDialogBox")?.Close(DialogResult.No);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
