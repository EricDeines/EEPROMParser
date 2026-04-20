using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using EEPROMParser.Model;
using Microsoft.VisualBasic;

public class Program
{
    public static async Task Main()
    {
        string filePath = "C:\\Entwicklung\\Python-Aufgaben\\MCP_FW_MotorControl\\EEPROM_BGxxdMoveCO.bin";
        List<string> strings = ["ALLOC_TABLE_CONTROL_CARD", "ABSOLUT_ENCODER", "TYPE_LABEL_DUNKER"];
        Variant variant = new("BG66", "dMove", "CO");
        variant.RegionGroups.Add(new RegionGroup("ALLOC_TABLE_CONTROL_CARD", 1024, 1));
        variant.RegionGroups.Add(new RegionGroup("ABSOLUT_ENCODER", 64, 2));
        variant.RegionGroups.Add(new RegionGroup("TYPE_LABEL_DUNKER", 64, 3));
        var result = EEPROMReader.ReadFile(filePath, variant, strings);
        var result2 = EEPROMReader.FormatBytes(result);

        await XMLParser.WriteNewXmlFile("test.xml", result2);
    }
}