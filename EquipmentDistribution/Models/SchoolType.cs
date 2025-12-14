using System.Collections.Generic;

namespace EquipmentDistribution.Models;

public class SchoolType
{
    public string Name { get; set; }
    public List<SchoolModel> Models { get; set; }

    public override string ToString() => Name;
}