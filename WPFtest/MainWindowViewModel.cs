using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFtest
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private Int32 _prova;

        public Int32 Prova
        {
            get => _prova;
            set
            {
                if (value == _prova) return;
                _prova = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProvaText));
            }
        }

        public String ProvaText => Prova.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            Application.Current.Dispatcher.Invoke(() => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void StartTask()
        {
            var task = new Task(Action);
            task.Start();
        }

        private async void Action()
        {
            for (int i = 0; i <= 100; i++)
            {
                Prova = i;
                await Task.Delay(20);
            }
        }
    }
}
