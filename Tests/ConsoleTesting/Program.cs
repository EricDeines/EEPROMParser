using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using EEPROMParser.Model;
using Microsoft.VisualBasic;

public class Program
{
    public static async Task Main()
    {
        var ListRegionGroups = await XMLParser.CreateListRegionGroups();
        var list = await XMLParser.CreateListVariants(ListRegionGroups);

        foreach(var variant in list)
        {
            foreach(var group in variant.RegionGroups)
            {
                Console.WriteLine(group.Name);
            }
        }
    }
}