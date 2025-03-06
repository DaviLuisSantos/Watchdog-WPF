using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Collections.Generic;

namespace Watchdog.Models;

public abstract partial class WatchdogTask : ObservableObject 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private bool _isRunning;

    [ObservableProperty]
    private string _status;

    public DateTime LastCheckTime { get; set; }
    public DateTime LastSuccessTime { get; set; }
    public DateTime LastFailureTime { get; set; }

    public List<RecoveryAction> RecoveryActions { get; set; } = new List<RecoveryAction>();

    public abstract Task<bool> CheckHealth();

    public virtual string GetDetails()
    {
        return string.Empty;
    }

    public async Task ExecuteRecoveryActions()
    {
        foreach (var action in RecoveryActions)
        {
            await action.Execute();
        }
    }
}