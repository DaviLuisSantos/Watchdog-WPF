using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchdog.Models;
namespace Watchdog.Data;

public class AppDbContext : DbContext
{
    public DbSet<WatchdogTask> WatchdogTasks { get; set; }
    public DbSet<HttpWatchdogTask> HttpWatchdogTasks { get; set; }
    public DbSet<UdpWatchdogTask> UdpWatchdogTasks { get; set; }
    public DbSet<RecoveryAction> RecoveryActions { get; set; }
    public DbSet<RestartProcessAction> RestartProcessActions { get; set; }
    public DbSet<RestartServiceAction> RestartServiceActions { get; set; }
    public DbSet<ExecuteCommandAction> ExecuteCommandActions { get; set; }

    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Processo> Processo { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WatchdogTask>()
            .HasDiscriminator<string>("TaskType")
            .HasValue<HttpWatchdogTask>("HTTP")
            .HasValue<UdpWatchdogTask>("UDP");

        modelBuilder.Entity<RecoveryAction>()
            .HasDiscriminator<string>("ActionType")
            .HasValue<RestartProcessAction>("RestartProcess")
            .HasValue<RestartServiceAction>("RestartService")
            .HasValue<ExecuteCommandAction>("ExecuteCommand");

        modelBuilder.Entity<WatchdogTask>()
            .HasMany(w => w.RecoveryActions)
            .WithOne(r => r.WatchdogTask)
            .HasForeignKey(r => r.WatchdogTaskId);

        base.OnModelCreating(modelBuilder);


    }
}
