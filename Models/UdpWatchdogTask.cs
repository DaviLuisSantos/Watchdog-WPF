using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Watchdog.Models;

public partial class UdpWatchdogTask : WatchdogTask
{
    [ObservableProperty]
    private string _host;

    [ObservableProperty]
    private int _port;

    [ObservableProperty]
    private string _sendData;

    [ObservableProperty]
    private string _expectedResponse;

    [ObservableProperty]
    private int _timeout;

    public override async Task<bool> CheckHealth()
    {
        try
        {
            using UdpClient UdpClient = new();
            UdpClient.Client.ReceiveTimeout = Timeout * 1000;
            IPEndPoint remoteEP = new(System.Net.IPAddress.Parse(Host), Port);
            byte[] sendBytes = Encoding.ASCII.GetBytes(SendData);

            await UdpClient.SendAsync(sendBytes, sendBytes.Length, remoteEP);

            var receiveResult = await UdpClient.ReceiveAsync();
            string receiveData = Encoding.ASCII.GetString(receiveResult.Buffer);

            LastCheckTime = DateTime.Now;

            if (receiveData.Contains(ExpectedResponse))
            {
                LastSuccessTime = DateTime.Now;
                Status = "OK - Response Matched";
                return true;
            }
            else
            {
                LastFailureTime = DateTime.Now;
                Status = $"Error: Expected '{ExpectedResponse}', got '{receiveData}'";
                return false;
            }
        }
        catch (Exception ex)
        {
            LastCheckTime = DateTime.Now;
            LastFailureTime = DateTime.Now;
            Status = $"Error: {ex.Message}";
            return false;
        }
    }
    public override string GetDetails()
    {
        return $"UDP Watchdog: Host={Host}, Port={Port}";
    }
}