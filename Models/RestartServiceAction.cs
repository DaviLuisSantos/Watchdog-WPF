using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Models;

public class RestartServiceAction:RecoveryAction
{
    [Required]
    public string ServiceName { get; set; }

    public override async Task Execute()
    {
        try
        {
            ServiceController service = new(ServiceName);
            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error restarting service {ServiceName}: {ex.Message}");
        }
        await Task.CompletedTask;
    }
}
