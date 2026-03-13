namespace EEPROMParser.Model;

public enum MotorSizes
{
    Size45,
    Size66,
    Size75,
    Size95,
    SizeBGE
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
    public MotorSizes Motor {get; set;}
    public FirmWare Firmware {get; set;}
    public Communication Comm {get; set;}

    public Variant(MotorSizes motor, FirmWare firmware, Communication comm)
    {
        this.Motor = motor;
        this.Firmware = firmware;
        this.Comm = comm;
    }

    public bool ValidateVariant()
    {
        bool returnValue;
        switch (Firmware)
        {
            case FirmWare.dMove:
                if ((Comm == Communication.IO || Comm == Communication.CO) && Motor != MotorSizes.SizeBGE)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
                break;
            case FirmWare.dPro:
                if (Comm == Communication.IO)
                {
                    returnValue = false;
                }
                else
                {
                    returnValue = true;
                }
                break;
            default:
                returnValue = false;
                break;
        }
        return returnValue;
    }

}

