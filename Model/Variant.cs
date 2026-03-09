namespace EEPROMParser.Model;

public enum MotorSizes
{
    Size45 = 45,
    Size66 = 66,
    Size75 = 75,
    Size95 = 95
}

public enum FirmWare
{
    dMove,
    dPro
}

public enum Communication
{
    IO,
    CO,
    EC,
    EI,
    PN
}
public class Variant
{
    public static readonly Dictionary<FirmWare, string> descFirmware = new()
    {
        {FirmWare.dMove, "dMove"},
        {FirmWare.dPro, "dPro"}
    };

    public static readonly Dictionary<Communication, string> descCommunication = new()
    {
        {Communication.IO, "IO"},
        {Communication.CO, "CO"},
        {Communication.EC, "EC"},
        {Communication.EI, "EI"},
        {Communication.PN, "PN"}
    };
    private MotorSizes motor;
    private FirmWare firmware;
    private Communication comm;

    public Variant()
    {
        
    }

}

