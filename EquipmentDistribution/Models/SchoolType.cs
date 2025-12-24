using System.Collections.Generic;
using Newtonsoft.Json;

namespace EquipmentDistribution.Models;

public class SchoolType
{
    public string Name { get; set; }
    [JsonIgnore] public string Filename { get; set; }
    public List<SchoolModel> Models { get; set; }

    public override string ToString() => Name;
}