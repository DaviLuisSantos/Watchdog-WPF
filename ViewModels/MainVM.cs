using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Watchdog.Data;
using Watchdog.Models;

namespace Watchdog.ViewModels;

public partial class MainVM: ObservableObject
{
    [ObservableProperty]
    private string endereco;

    [ObservableProperty]
    private Processo processo = new();

    [ObservableProperty]
    private ObservableCollection<Processo> processos;

    [RelayCommand]
    private async Task Teste() {
        MessageBox.Show($"{Endereco}");
    }

    [RelayCommand]
    private async Task Save()
    {
        var db = new AppDbContext();
        if (this.Processo.Id > 0)
        {
            db.Processo.Update(entity: Processo);
        }
        else
        {
            await db.Processo.AddAsync(Processo);
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
        
            db.Processo.Remove(this.Processo);

        var response = db.SaveChanges();

        bool isSucess = response > 0;
        if (isSucess)
            MessageBox.Show("Operação realizada com Suceso");
    }

    [RelayCommand]
    private async Task Read()
    {
        var db = new AppDbContext();
        Processos = new ObservableCollection<Processo>(db.Processo.ToList());
    }

    partial void OnEnderecoChanged(string? oldValue, string newValue)
    {
        MessageBox.Show($"O valor de {oldValue} mudou para {newValue}");
    }
}
