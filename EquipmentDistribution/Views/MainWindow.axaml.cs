using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using EquipmentDistribution.Models;
using EquipmentDistribution.ViewModels;
using Newtonsoft.Json;

namespace EquipmentDistribution.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel vm;
    
    public MainWindow()
    {
        InitializeComponent();

        if (!Directory.Exists("SchoolModels"))
            Directory.CreateDirectory("SchoolModels");
    }

    protected override async void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        vm = DataContext as MainWindowViewModel;

        try
        {
            vm.Classrooms = JsonConvert.DeserializeObject<ClassroomTypes>(File.ReadAllText("classrooms.json"))
                ?.Classrooms ?? [];

            vm.SchoolTypes = Directory.GetFiles("SchoolModels", "*.json")
                .Select(o => JsonConvert.DeserializeObject<SchoolType>(File.ReadAllText(o)) ?? new()).ToList();
        }
        catch (Exception ex)
        {
            await new DialogWindow($"Не удалось загрузить данные кабинетов и моделей:\n{ex.Message}").ShowDialog(this);
        }
    }


    private async void LoadEquipment_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var files = await StorageProvider.OpenFilePickerAsync(
                new()
                {
                    AllowMultiple = false,
                    FileTypeFilter = [new("Excel file") { Patterns = ["*.xlsx"] }, FilePickerFileTypes.All]
                });

            if (files.Count != 0)
            {
                vm.StatusText = "Чтение...";
                vm.Equipment = await Task.Run(() => SpreadsheetActions.ReadEquipmentSpreadsheet(files[0].Path.LocalPath));

                foreach (var equipment in vm.Equipment)
                {
                    equipment.Classroom = vm.Classrooms.FirstOrDefault(o =>
                        o.Aliases.Any(a => equipment.Place.Contains(a, StringComparison.OrdinalIgnoreCase)));
                }
            }
        }
        catch (Exception ex)
        {
            await new DialogWindow($"Не удалось прочитать файл:\n{ex.Message}").ShowDialog(this);
        }
        vm.StatusText = "";
    }
    
    private async void Export_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var file = await StorageProvider.SaveFilePickerAsync(
                new()
                {
                    FileTypeChoices = [new("Excel file") { Patterns = ["*.xlsx"] }]
                });
            
            vm.StatusText = "Экспорт...";
            
            if (file != null)
                await Task.Run(() => SpreadsheetActions.SaveEquipmentSpreadsheet(vm.DistributionResult,
                    $"{vm.SelectedSchoolType} ({vm.SelectedSchoolModel})", file.Path.LocalPath));
        }
        catch (Exception ex)
        {
            await new DialogWindow($"Не удалось сохранить файл:\n{ex.Message}").ShowDialog(this);
        }
        vm.StatusText = "";
    }
}