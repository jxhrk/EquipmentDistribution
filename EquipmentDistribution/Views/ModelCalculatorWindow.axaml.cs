using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using EquipmentDistribution.Converters;
using EquipmentDistribution.Models;
using EquipmentDistribution.ViewModels;
using Newtonsoft.Json;

namespace EquipmentDistribution.Views;

public partial class ModelCalculatorWindow : Window
{
    private MainWindowViewModel vm;
    
    public ModelCalculatorWindow()
    {
        InitializeComponent();
    }

    public ModelCalculatorWindow(MainWindowViewModel vm) : this()
    {
        this.vm = vm;
        DataContext = vm;

        foreach (var classroom in vm.Classrooms)
        {
            HoursDataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = classroom.Name,
                IsReadOnly = false,
                Binding = new Binding("Hours") { Converter = new IndexerConverter(), ConverterParameter = classroom.Name},
            });
        }
        
        vm.CalculateClassrooms();
    }


    private async void ClassesCountDataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        // HACK: binding to source is not working with dictionary values
        if (e.Row.DataContext is KeyValuePair<int, int> pair && vm.SchoolModelOverride is not null && int.TryParse((e.EditingElement as TextBox)?.Text, out int value))
            vm.SchoolModelOverride.Classes[pair.Key] = value;
    }

    private void ClassesCountDataGrid_OnCellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        // HACK: binding updating is not working with dictionary
        ClassesCountDataGrid.ItemsSource = null;
        ClassesCountDataGrid.ItemsSource = vm.SchoolModelOverride?.Classes;
        
        vm.CalculateClassrooms();
    }

    private void HoursDataGrid_OnCellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        vm.CalculateClassrooms();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        vm.CalculateClassrooms();
    }

    private async void HoursSaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        HoursSaveStatus.Text = "Сохранение...";
        await File.WriteAllTextAsync("learning_hours.json", JsonConvert.SerializeObject(vm.LearningHoursInfo, Formatting.Indented));
        HoursSaveStatus.Text = "Сохранено";
        await Task.Delay(2000);
        HoursSaveStatus.Text = "";
    }

    private async void SaveModelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (vm.SelectedSchoolType is null || vm.SchoolModelOverride is null)
            return;
        
        DialogWindow dialog = new("Введите название модели", true);
        await dialog.ShowDialog(this);
        SaveModelStatus.Text = "Сохранение...";
        vm.SchoolModelOverride.Name = dialog.ReceivedInput;
        vm.SchoolModelOverride.Classrooms = vm.ClassroomCalculationResult.ToDictionary(o => o.Classroom, o => o.CalculatedCount);
        vm.SelectedSchoolType.Models.Add(vm.SchoolModelOverride);
        await File.WriteAllTextAsync($"{vm.SelectedSchoolType.Filename}", JsonConvert.SerializeObject(vm.SelectedSchoolType, Formatting.Indented));
        SaveModelStatus.Text = "Сохранено";
        await Task.Delay(2000);
        SaveModelStatus.Text = "";
    }
}