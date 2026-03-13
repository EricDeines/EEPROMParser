using System.Threading.Tasks;
using System.Xml;

namespace EEPROMParser.Model;

public class XMLParser
{
    private static string testUri = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListRegionGroups.xml";
    public static void test()
    {
        using var reader = XmlReader.Create(testUri, new XmlReaderSettings {
            IgnoreComments = true,
            IgnoreWhitespace = true
        });
    }

    public static async Task<List<RegionGroup>> createListRegionGroups()
    {
        using var reader = XmlReader.Create(testUri, new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            Async = true
        });
        List<RegionGroup> list = [];

        while (await reader.ReadAsync())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "RegionGroup")
            {
                RegionGroup group = (RegionGroup)await reader.ReadContentAsAsync(typeof(RegionGroup), null);
                list.Add(group);
            }
        }
        return list;
    }

    // public static async Task<List<EEPROMVariant>> createListEEPROMVariants()
    // {
    //     using var reader = XmlReader.Create(testUri, new XmlReaderSettings
    //     {
    //         IgnoreComments = true,
    //         IgnoreWhitespace = true,
    //         Async = true
    //     });
    //     List<EEPROMVariant> list = [];

    //     while (await reader.ReadAsync())
    //     {
    //         switch (reader.NodeType)
    //         {
                
    //         }
    //     }
    // }

    public static async Task Main()
    {
        Console.WriteLine(await createListRegionGroups());
    }
}