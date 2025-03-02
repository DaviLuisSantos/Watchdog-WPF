using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Models;

public class ExecuteCommandAction:RecoveryAction
{
    [Required]
    public string Command { get; set; }

    public override async Task Execute()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {Command}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine($"Command Output: {output}");
            Console.WriteLine($"Command Error: {error}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error executing command {Command}: {ex.Message}");
        }
        await Task.CompletedTask;
    }
}
