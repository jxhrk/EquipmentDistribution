using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EquipmentDistribution.Models;

namespace EquipmentDistribution.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private List<Classroom> classrooms;

    [ObservableProperty] private List<SchoolType> schoolTypes;

    [ObservableProperty] private List<SchoolModel> availableModels;

    [ObservableProperty] private SchoolType? selectedSchoolType;
    
    [ObservableProperty] private SchoolModel? selectedSchoolModel;
    
    [ObservableProperty] private List<Equipment> equipment = [];
    
    [ObservableProperty] private List<DistributedEquipment> distributionResult = [];
    
    [ObservableProperty] private string statusText;
    

    partial void OnSelectedSchoolTypeChanged(SchoolType? value)
    {
        if (value == null) return;
        AvailableModels = value.Models;
    }

    public void Calculate()
    {
        DistributionResult = Equipment.Select(o => new DistributedEquipment()
        {
            Equipment = o,
            Count = (SelectedSchoolModel?.Classrooms.GetValueOrDefault(o.Classroom?.Name ?? "") ?? 0) * o.CountPerClassroom
        }).Where(o => o.Count > 0).OrderBy(o => o.Equipment.Classroom?.Name).ToList();
    }
}