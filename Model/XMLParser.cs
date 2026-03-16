using System.Xml;
using Microsoft.VisualBasic;

namespace EEPROMParser.Model;

public class XMLParser
{
    private static string testUri = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListRegionGroups.xml";

    private static string testUri2 = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListEEPROMVariants.xml";

    public static async Task<List<RegionGroup>> CreateListRegionGroups()
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
                var name = reader.GetAttribute("name");
                var size = Convert.ToInt32(reader.GetAttribute("size"));
                var id = Convert.ToInt32(reader.GetAttribute("id"));
                RegionGroup group = new(name, size, id);
                list.Add(group);
            }
        }
        return list;
    }

    public static async Task<List<Variant>> CreateListVariants(List<RegionGroup> regionGroups)
    {
        using var reader = XmlReader.Create(testUri2, new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            Async = true
        });
        List<Variant> list = [];

        string motor = "";
        string firmware = "";
        string communication = "";
        Variant variant = new("", "", "");
        List<RegionGroup> groups = new();

        while (await reader.ReadAsync())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "EEPROMVariant":
                        motor = reader.GetAttribute("motor");
                        firmware = reader.GetAttribute("firmware");
                        communication = reader.GetAttribute("communication");
                        variant = new Variant(motor, firmware, communication);
                        break;
                    case "ListRegionGroups":
                        groups = new();
                        break;
                    case "RegionGroup":
                        var group = RegionGroup.GetRegionGroupById(regionGroups, Convert.ToInt32(reader.GetAttribute("id")));
                        groups.Add(group);
                        break;
                    default:
                        break;
                }
            }

            if (reader.NodeType == XmlNodeType.EndElement)
            {
                switch (reader.Name)
                {
                    case "ListRegionGroups":
                        variant.RegionGroups.AddRange(groups);
                        break;
                    case "EEPROMVariant":
                        list.Add(variant);
                        break;
                    default:
                        break;
                }
            }
        }
        return list;
    }

}