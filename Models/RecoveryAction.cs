using System.ComponentModel.DataAnnotations;

namespace Watchdog.Models;

public abstract class RecoveryAction
{
    [Key]
    public int Id { get; set; }
    public int WatchdogTaskId { get; set; }
    public WatchdogTask WatchdogTask { get; set; }

    public abstract Task Execute();
}
