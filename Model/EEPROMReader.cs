



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
}