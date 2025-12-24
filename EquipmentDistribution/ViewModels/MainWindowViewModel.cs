using System;
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
    [ObservableProperty] private SchoolModel? schoolModelOverride;
    
    [ObservableProperty] private LearningHoursInfo learningHoursInfo;
    
    [ObservableProperty] private List<Equipment> equipment = [];
    
    [ObservableProperty] private List<DistributedEquipment> distributionResult = [];
    
    [ObservableProperty] private List<ClassroomCalculationResult> classroomCalculationResult = [];
    [ObservableProperty] private int originalClassroomCount;
    [ObservableProperty] private int calculatedClassroomCount;
    [ObservableProperty] private bool classroomCalculationFailed;
    
    [ObservableProperty] private string statusText;
    
    partial void OnSelectedSchoolModelChanged(SchoolModel? value)
    {
        SchoolModelOverride = value?.Clone();
    }

    partial void OnSelectedSchoolTypeChanged(SchoolType? value)
    {
        if (value == null) return;
        AvailableModels = value.Models;
    }

    public void CalculateEquipment()
    {
        DistributionResult = Equipment.Select(o => new DistributedEquipment()
        {
            Equipment = o,
            Count = (SelectedSchoolModel?.Classrooms.GetValueOrDefault(o.Classroom?.Name ?? "") ?? 0) * o.CountPerClassroom
        }).Where(o => o.Count > 0).OrderBy(o => o.Equipment.Classroom?.Name).ToList();
    }

    public void CalculateClassrooms()
    {
        ClassroomCalculationResult = SelectedSchoolModel?.Classrooms.Select(o =>
        {
            int classroomUsageHours = 0;
            foreach (var hoursInfo in LearningHoursInfo.Classes)
                classroomUsageHours += hoursInfo.Hours[o.Key] * SchoolModelOverride?.Classes[hoursInfo.Year] ?? 0;

            return new ClassroomCalculationResult()
            {
                Classroom = o.Key,
                OriginalCount = o.Value,
                CalculatedCount = Convert.ToInt32(Math.Ceiling(classroomUsageHours / 30.0)),
            };
        }).ToList() ?? [];
        
        OriginalClassroomCount = ClassroomCalculationResult.Select(o => o.OriginalCount).Sum();
        CalculatedClassroomCount = ClassroomCalculationResult.Select(o => o.CalculatedCount).Sum();
        ClassroomCalculationFailed = CalculatedClassroomCount > OriginalClassroomCount;
    }
}