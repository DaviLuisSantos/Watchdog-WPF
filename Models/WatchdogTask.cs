using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Models;

public partial class WatchdogTask : ObservableObject
{
    [ObservableProperty]
    private string _url = string.Empty, _status = "Stopped";
    [ObservableProperty]
    private int _interval = 10;
    [ObservableProperty]
    private bool _isRunning = false;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public async Task Start()
    {
        if (IsRunning) return;

        IsRunning = true;
        Status = "Running";
        _cancellationTokenSource = new CancellationTokenSource();

        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(Url, _cancellationTokenSource.Token);
                    response.EnsureSuccessStatusCode(); // Lança exceção se não for 200-299
                    Status = $"OK - {response.StatusCode}";
                }
            }
            catch (HttpRequestException ex)
            {
                Status = $"Error: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled, exit loop
                Status = "Stopped";
                break;
            }
            catch (Exception ex)
            {
                Status = $"Unexpected Error: {ex.Message}";
            }

            try
            {
                await Task.Delay(Interval * 1000, _cancellationTokenSource.Token); // Converte segundos para milissegundos
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled during the delay, exit loop
                Status = "Stopped";
                break;
            }
        }
        IsRunning = false;

        if (Status != "Stopped")
        {
            Status = "Stopped";
        }
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
        IsRunning = false;
        Status = "Stopping...";
    }

}
