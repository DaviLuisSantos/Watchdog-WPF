using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Models;

public class RestartProcessAction:RecoveryAction
{
    [Required]
    public string ProcessName { get; set; }

    public override async Task Execute()
    {
        try
        {
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach(var process in processes)
            {
                process.Kill();
                process.WaitForExit();

                Process.Start(ProcessName);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error restarting process {ProcessName}: {ex.Message}");
        }
        await Task.CompletedTask;
    }
}
