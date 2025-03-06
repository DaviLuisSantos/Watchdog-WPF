using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Policy;

namespace Watchdog.Models;

public partial class HttpWatchdogTask : WatchdogTask
{
    [ObservableProperty]
    private string _url;

    [ObservableProperty]
    private ReqMethod _httpRestMethod = ReqMethod.Get;

    [ObservableProperty]
    private int _interval;

    public override async Task<bool> CheckHealth()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                HttpMethod method = new(HttpRestMethod.ToString());
                HttpRequestMessage request = new HttpRequestMessage(method, Url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                LastCheckTime = DateTime.Now;
                LastSuccessTime = DateTime.Now;
                Status = $"OK - {response.StatusCode}";
                return true;
            }
        }
        catch (HttpRequestException ex)
        {
            LastCheckTime = DateTime.Now;
            LastFailureTime = DateTime.Now;
            Status = $"Error: {ex.Message}";
            return false;
        }
        catch (TaskCanceledException)
        {
            LastCheckTime = DateTime.Now;
            LastFailureTime = DateTime.Now;
            Status = "Timeout";
            return false;
        }
        catch (Exception ex)
        {
            LastCheckTime = DateTime.Now;
            LastFailureTime = DateTime.Now;
            Status = $"Unexpected Error: {ex.Message}";
            return false;
        }
    }

    public override string GetDetails()
    {
        return $"HTTP Watchdog: URL={Url}, Method={HttpRestMethod}";
    }
}

public enum ReqMethod
{
    Get,
    Post,
    Put,
    Delete
}
