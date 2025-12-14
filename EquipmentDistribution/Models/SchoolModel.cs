using System.Collections.Generic;

namespace EquipmentDistribution.Models;

public class SchoolModel
{
    public string Name { get; set; }
    
    public Dictionary<string, int> Classrooms { get; set; }
    
    public override string ToString() => Name;
}