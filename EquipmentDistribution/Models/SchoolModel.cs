using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentDistribution.Models;

public class SchoolModel
{
    public string Name { get; set; }
    
    public Dictionary<int, int> Classes { get; set; }
    
    public Dictionary<string, int> Classrooms { get; set; }
    
    public override string ToString() => Name;

    public SchoolModel Clone() => new()
    {
        Name = Name,
        Classes = Classes.ToDictionary(entry => entry.Key, entry => entry.Value),
        Classrooms = Classrooms.ToDictionary(entry => entry.Key, entry => entry.Value),
    };
    
}