using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Watchdog.Models;
using System.Windows.Input;
using Watchdog.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Xml.Linq;
using Watchdog.Converters;
using static Watchdog.Models.HttpWatchdogTask;

namespace Watchdog.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly AppDbContext _context;
    private CancellationTokenSource _cancellationTokenSource;

    public ObservableCollection<WatchdogTask> WatchdogTasks { get; } = new ObservableCollection<WatchdogTask>();

    [ObservableProperty]
    private string _newHttpUrl;

    [ObservableProperty]
    private int _newHttpInterval = 60;

    [ObservableProperty]
    private HttpMethod _newHttpMethod = HttpMethod.Get;

    [ObservableProperty]
    private string _newUdpHost;

    [ObservableProperty]
    private int _newUdpPort = 53; // Porta padrão do DNS

    [ObservableProperty]
    private string _newUdpSendData = "Ping";

    [ObservableProperty]
    private string _newUdpExpectedResponse = "Pong";

    [ObservableProperty]
    private int _newUdpTimeout = 5;

    [ObservableProperty]
    private WatchdogTask? _selectedTask;

    public static Array AvailableHttpMethods => Enum.GetValues(typeof(HttpMethod));
    public MainViewModel(AppDbContext context)
    {
        _context = context;
        _cancellationTokenSource = new CancellationTokenSource();

        LoadWatchdogTasks();
    }

    private async void LoadWatchdogTasks()
    {
        WatchdogTasks.Clear();
        var tasks = await _context.WatchdogTasks
            .Include(t => t.RecoveryActions) // Carrega as RecoveryActions relacionadas
            .ToListAsync();
        foreach (var task in tasks)
        {
            WatchdogTasks.Add(task);
        }
    }

    [RelayCommand]
    private async void AddHttpWatchdogTask()
    {
        if (!string.IsNullOrWhiteSpace(NewHttpUrl))
        {
            var newTask = new HttpWatchdogTask { Name = "HTTP Task", UrlWatchdog = NewHttpUrl, Interval = NewHttpInterval, HttpRestMethod = NewHttpMethod, IsEnabled = true };
            _context.WatchdogTasks.Add(newTask);
            WatchdogTasks.Add(newTask); // Adiciona à coleção para exibição imediata

            NewHttpUrl = string.Empty;
            NewHttpInterval = 60;
            await _context.SaveChangesAsync();
        }
    }

    [RelayCommand]
    private async void AddUdpWatchdogTask()
    {
        if (!string.IsNullOrWhiteSpace(NewUdpHost))
        {
            var newTask = new UdpWatchdogTask
            {
                Name = "UDP Task",
                Host = NewUdpHost,
                Port = NewUdpPort,
                SendData = NewUdpSendData,
                ExpectedResponse = NewUdpExpectedResponse,
                Timeout = NewUdpTimeout,
                IsEnabled = true
            };
            _context.WatchdogTasks.Add(newTask);
            WatchdogTasks.Add(newTask); // Adiciona à coleção para exibição imediata
            NewUdpHost = string.Empty;
            NewUdpPort = 53;
            NewUdpSendData = "Ping";
            NewUdpExpectedResponse = "Pong";
            NewUdpTimeout = 5;
            await _context.SaveChangesAsync();
        }
    }

    [RelayCommand]
    private async void StartWatchdogTask(WatchdogTask? task)
    {
        if (task != null && task.IsEnabled && !task.IsRunning)
        {
            //task.Start(); //Nao precisamos mais usar o start diretamente
            await RunWatchdogTaskAsync(task);
        }
    }

    [RelayCommand]
    private void StopWatchdogTask(WatchdogTask? task)
    {
        if (task != null && task.IsRunning)
        {
            _cancellationTokenSource.Cancel();
            task.Status = "Stopping...";
        }
    }

    [RelayCommand]
    private async void RemoveWatchdogTask(WatchdogTask? task)
    {
        if (task != null)
        {
            StopWatchdogTask(task); // Para a tarefa antes de remover
            _context.WatchdogTasks.Remove(task);
            WatchdogTasks.Remove(task); // Remove da coleção
            await _context.SaveChangesAsync();
        }
    }

    [RelayCommand]
    private async void SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    private async Task RunWatchdogTaskAsync(WatchdogTask task)
    {
        if (task == null || !task.IsEnabled || task.IsRunning) return;

        task.IsRunning = true;
        _cancellationTokenSource = new CancellationTokenSource(); // Reset the token source

        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            bool isHealthy = false;
            try
            {
                isHealthy = await task.CheckHealth();
                if (!isHealthy)
                {
                    await task.ExecuteRecoveryActions();
                }
            }
            catch (Exception ex)
            {
                task.Status = $"Error: {ex.Message}";
                // Log the exception
                Console.WriteLine(ex.ToString());
            }

            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                task.Status = "Stopped";
                break; // Exit the loop if cancellation is requested
            }

            try
            {
                // Determine the delay based on the task type
                int delayInSeconds = (task is HttpWatchdogTask httpTask) ? httpTask.Interval :
                                    (task is UdpWatchdogTask) ? 5 : 60; // Default interval

                await Task.Delay(delayInSeconds * 1000, _cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled during the delay
                task.Status = "Stopped";
                break;
            }
        }
        task.IsRunning = false;
    }
}