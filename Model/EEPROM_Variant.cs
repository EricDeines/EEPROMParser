
namespace EEPROMParser.Model;

public enum EEPROMCommunication
{
    CO,
    IO,
    IE
}

public class EEPROMVariant
{
    public MotorSizes Motor {get; set;}
    public FirmWare Firmware {get; set;}
    public EEPROMCommunication Comm {get; set;}

    public List<RegionGroup> RegionGroups {get; private set;}

    public EEPROMVariant(MotorSizes motor, FirmWare firmware, EEPROMCommunication comm, List<RegionGroup> regionGroups)
    {
        this.Motor = motor;
        this.Firmware = firmware;
        this.Comm = comm;
        this.RegionGroups = regionGroups;
    }

    public EEPROMVariant(Variant variant, List<RegionGroup> regionGroups)
    {
        this.Motor = variant.Motor;
        this.Firmware = variant.Firmware;
        var comm = variant.Comm switch
        {
            Communication.CO => EEPROMCommunication.CO,
            Communication.IO => EEPROMCommunication.IO,
            _ => EEPROMCommunication.IE,
        };
        this.Comm = comm;
        this.RegionGroups = regionGroups;
    }
}