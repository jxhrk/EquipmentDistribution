using System.Collections.Generic;

namespace EquipmentDistribution.Models;

public class Equipment
{
    public string Name { get; set; }
    public string Place { get; set; }
    public int CountPerClassroom { get; set; }
    public string Unit { get; set; }

    public Classroom? Classroom { get; set; }
}