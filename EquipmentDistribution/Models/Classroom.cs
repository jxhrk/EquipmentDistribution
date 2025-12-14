namespace EquipmentDistribution.Models;

public class Classroom
{
    public string Name { get; set; }

    public string[] Aliases { get; set; } = [];

    public Classroom() { }
    
    public Classroom(string name, string[] aliases)
    {
        Name = name;
        Aliases = aliases;
    }
}