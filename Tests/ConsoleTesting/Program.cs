using System.Diagnostics;
using System.Threading.Tasks;
using EEPROMParser.Model;

public class Program
{
    public static async Task Main()
    {
        var list = await XMLParser.createListRegionGroups();
        foreach (RegionGroup group in list)
        {
            Console.WriteLine(group.Id);
        }
    }
}