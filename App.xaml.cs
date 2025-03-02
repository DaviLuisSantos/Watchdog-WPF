using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Watchdog.Data;
using Watchdog.ViewModels;

namespace Watchdog
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuração do EF Core com SQLite
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=watchdog.db"); // Nome do arquivo do banco de dados
            });

            // Registrar ViewModels
            services.AddSingleton<MainViewModel>();

            // Registrar a MainWindow
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Criar e migrar o banco de dados no início
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate(); // Aplica as migrações pendentes
            }

            // Obter e mostrar a MainWindow
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}