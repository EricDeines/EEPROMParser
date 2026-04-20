



using Microsoft.VisualBasic;

namespace EEPROMParser.Model;


public class EEPROMReader
{

    public static string filePath = "C:\\Entwicklung\\Python-Aufgaben\\MCP_FW_MotorControl\\EEPROM_BGxxdMoveCO.bin";
    public static void ReadBytes()
    {
        using var reader = new BinaryReader(File.Open(filePath, FileMode.Open));

        byte[] bytes = reader.ReadBytes(10);

        for (int i = 0; i < bytes.Length; i++)
        {
            byte[] bytes1 = [bytes[i]];
            Console.WriteLine(Convert.ToHexString(bytes1));
        }
    }

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