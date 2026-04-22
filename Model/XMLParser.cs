using System.Xml;

namespace EEPROMParser.Model;

public class XMLParser
{
    // ToDo: Make these files loadable through UI.
    private static string testUri = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListRegionGroups.xml";

    private static string testUri2 = "C:\\Entwicklung\\Python-Aufgaben\\dotnet-aufgaben\\EEPROMParser\\Model\\ListEEPROMVariants.xml";

    /// <summary>
    /// An asynchronous method that loads the Region groups defined in the XML-Config files and returns it.
    /// </summary>
    /// <returns> A Task representing the asynchronous method.</returns>
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

    /// <summary>
    /// An asynchronous method that loads the variants defined in the XML-Config files and returns it.
    /// This method requires a List of <c>RegionGroup</c> objects as input because the XML-Config files store
    /// for each Variant only the id of the Region groups associated with it.
    /// </summary>
    /// <param name="regionGroups"></param>
    /// <returns></returns>
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

    /// <summary>
    /// An asynchronous method that creates a new xml file with the given file name and writes data from the input dictionary into
    /// the xml file.
    /// </summary>
    /// <param name="newFileName">An absolute path to the new file.</param>
    /// <param name="bytesPerGroup">A Dictionary which contains the data to write into the xml file.</param>
    /// <returns></returns>
    public static async Task WriteNewXmlFile(string newFileName, Dictionary<string, string> bytesPerGroup)
    {
        using var writer = XmlWriter.Create(newFileName, new XmlWriterSettings
        {
            Async = true,
            Indent = true,
            NewLineHandling = NewLineHandling.None
        });

        await writer.WriteStartDocumentAsync();
        await writer.WriteStartElementAsync(null, "BytesPerRegionGroup", null);
        foreach (var pair in bytesPerGroup)
        {
            await writer.WriteStartElementAsync(null, pair.Key, null);
            await writer.WriteStringAsync("\r\n");
            await writer.WriteStringAsync(pair.Value);
            await writer.WriteWhitespaceAsync("  ");
            await writer.WriteEndElementAsync();
        }
        await writer.WriteEndElementAsync();
        await writer.WriteEndDocumentAsync();
    }
}