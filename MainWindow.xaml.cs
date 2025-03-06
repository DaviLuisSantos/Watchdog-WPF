using System.Text;
using System.Windows;
using Watchdog.ViewModels;

namespace Watchdog;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //DataContext = viewModel;
    }
}