

namespace EEPROMParser.Model;


public class EEPROMReader
{

    /// <summary>
    /// This method reads the binary content specified by the groups and the variant from a binary file specified by its file path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="variant"></param>
    /// <param name="groups"></param>
    /// <returns>A Dictionary which matches the name of each Region group to its byte array.</returns>
    public static Dictionary<string, byte[]> ReadFile(string filePath, Variant variant, List<string> groups)
    {
        using var reader = new BinaryReader(File.Open(filePath, FileMode.Open));

        Dictionary<string, byte[]> BytesPerGroup = new();
        foreach (var group in variant.RegionGroups)
        {
            byte[] bytes = reader.ReadBytes(group.Size);
            if (groups.Contains(group.Name))
            {
                BytesPerGroup.Add(group.Name, bytes);
            }
        }
        return BytesPerGroup;
    }

    /// <summary>
    /// This method converts the byte arrays from an incoming Dictionary to a formatted Hex string
    /// and returns a new Dictionary. 
    /// </summary>
    /// <param name="bytesPerGroup"></param>
    /// <returns></returns>
    public static Dictionary<string, string> FormatBytes(Dictionary<string, byte[]> bytesPerGroup)
    {
        Dictionary<string, string> result = new();
        foreach (var pair in bytesPerGroup)
        {
            string newValue = Convert.ToHexString(pair.Value);
            int i = 1;
            while (true)
            {
                try
                {
                    if (i % 16 == 0)
                    {
                        newValue = newValue.Insert(3*i-1, "\n");
                    }
                    else
                    {
                        newValue = newValue.Insert(3*i-1, " ");
                    }
                    i++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }
            result.Add(pair.Key, newValue);
        }
        return result;
    }
}