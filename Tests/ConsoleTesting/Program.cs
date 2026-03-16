using System.Diagnostics;
using System.Threading.Tasks;
using EEPROMParser.Model;

public class Program
{
    public static void Main()
    {
        // var list = await XMLParser.createListRegionGroups();
        // foreach (RegionGroup group in list)
        // {
        //     Console.WriteLine(group.Id);
        // }

        // var variant = new Variant("45", "dPro", "CO");
        // variant.RegionGroups.AddRange(list);
        RegionGroup group = new("MOTOR", 50, 1);
        RegionGroup group2 = new("motor", 50, 1);

        Console.WriteLine(group.Equals(group2));

    }
}