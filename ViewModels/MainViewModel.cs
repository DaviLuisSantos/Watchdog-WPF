using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Watchdog.Models;
using System.Windows.Input;

namespace Watchdog.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<WatchdogTask> WatchdogTasks { get; } = new ObservableCollection<WatchdogTask>();

        [ObservableProperty]
        private string _newUrl;

        [ObservableProperty]
        private int _newInterval = 60;

        public ICommand AddWatchdogTaskCommand { get; }
        public ICommand StartWatchdogTaskCommand { get; }
        public ICommand StopWatchdogTaskCommand { get; }
        public ICommand RemoveWatchdogTaskCommand { get; }

        public MainViewModel()
        {
            AddWatchdogTaskCommand = new RelayCommand(AddWatchdogTask);
            StartWatchdogTaskCommand = new RelayCommand<WatchdogTask>(StartWatchdogTask);
            StopWatchdogTaskCommand = new RelayCommand<WatchdogTask>(StopWatchdogTask);
            RemoveWatchdogTaskCommand = new RelayCommand<WatchdogTask>(RemoveWatchdogTask);
        }

        private void AddWatchdogTask()
        {
            if (!string.IsNullOrWhiteSpace(NewUrl))
            {
                var newTask = new WatchdogTask { Url = NewUrl, Interval = NewInterval };
                WatchdogTasks.Add(newTask);
                NewUrl = string.Empty; // Limpa o campo de entrada
                NewInterval = 60;
            }
        }

        private void StartWatchdogTask(WatchdogTask task)
        {
            if (task != null && !task.IsRunning)
            {
                Task.Run(() => task.Start()); // Executa em uma thread separada para não bloquear a UI
            }
        }

        private void StopWatchdogTask(WatchdogTask task)
        {
            if (task != null && task.IsRunning)
            {
                task.Stop();
            }
        }

        private void RemoveWatchdogTask(WatchdogTask task)
        {
            if (task != null)
            {
                task.Stop(); // Garante que a tarefa seja parada antes de remover
                WatchdogTasks.Remove(task);
            }
        }
    }
}