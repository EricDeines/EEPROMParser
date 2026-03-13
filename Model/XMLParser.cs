using System.Xml;

namespace EEPROMParser.Model;

public class XMLParser
{
    private static string testUri = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListRegionGroups.xml";

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
                var name = reader.GetAttribute("name");
                var size = Convert.ToInt32(reader.GetAttribute("size"));
                var id = Convert.ToInt32(reader.GetAttribute("id"));
                RegionGroup group = new(name, size, id);
                list.Add(group);
            }
        }
        return list;
    }

}