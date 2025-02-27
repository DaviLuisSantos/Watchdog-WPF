using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Watchdog.Data;

namespace Watchdog.ViewModels;

public partial class MainVM: ObservableObject
{
    [ObservableProperty]
    private string endereco;

    [ObservableProperty]
    private Models.Process process = new();

    [RelayCommand]
    private async Task Teste() {
        MessageBox.Show($"{Endereco}");
    }

    [RelayCommand]
    private async Task Save()
    {
        var db = new AppDbContext();
        if (Process.Id > 0)
        {
            await db.Process.AddAsync(this.Process);
        }
        else
        {
            db.Process.Update(entity: Process);
        }

        var response = db.SaveChanges();

        bool isSucess = response > 0;
        if (isSucess)
            MessageBox.Show("Operação realizada com Suceso");
    }
    [RelayCommand]
    private async Task Delete()
    {
        var db = new AppDbContext();
        
            db.Process.Remove(this.Process);

        var response = db.SaveChanges();

        bool isSucess = response > 0;
        if (isSucess)
            MessageBox.Show("Operação realizada com Suceso");
    }

    [RelayCommand]
    private async Task Read()
    {

    }

    partial void OnEnderecoChanged(string? oldValue, string newValue)
    {
        MessageBox.Show($"O valor de {oldValue} mudou para {newValue}");
    }
}
