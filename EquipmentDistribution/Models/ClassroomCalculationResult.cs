namespace EquipmentDistribution.Models;

public class ClassroomCalculationResult
{
    public string Classroom { get; set; }
    public int OriginalCount { get; set; }
    public int CalculatedCount { get; set; }
    
    public int Difference => CalculatedCount - OriginalCount;
}