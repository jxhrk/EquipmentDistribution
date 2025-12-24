using System.Collections.Generic;

namespace EquipmentDistribution.Models;

public class LearningHoursInfo
{
    public List<LearningHoursInfoClass> Classes { get; set; }
}

public class LearningHoursInfoClass
{
    public int Year { get; set; }
    public Dictionary<string, int> Hours { get; set; }
}