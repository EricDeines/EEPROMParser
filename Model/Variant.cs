namespace EEPROMParser.Model;


public class Variant : IEquatable<Variant>
{
    
    public string Motor {get; private set;}
    public string Firmware {get; private set;}
    public string Comm {get; private set;}

    public List<RegionGroup> RegionGroups {get; private set;} = new();

    public Variant(string motor, string firmware, string comm)
    {
        this.Motor = motor;
        this.Firmware = firmware;
        this.Comm = comm;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Variant other)
        {
            return false;
        }
        else
        {
            return Equals(other);
        }
    }

    public bool Equals(Variant? variant)
    {
        if (variant is null)
        {
            return false;
        }
        return (
            string.Equals(Motor, variant.Motor, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Firmware, variant.Firmware, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Comm, variant.Comm, StringComparison.OrdinalIgnoreCase) &&
            RegionGroups.SequenceEqual(variant.RegionGroups)
        );
    }

    public static bool operator ==(Variant left, Variant right) =>
        left.Equals(right);
    
    public static bool operator !=(Variant left, Variant right) =>
        left.Equals(right);

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(Motor.ToLowerInvariant());
        hash.Add(Firmware.ToLowerInvariant());
        hash.Add(Comm.ToLowerInvariant());

        foreach (var group in RegionGroups)
        {
            hash.Add(group);
        }

        return hash.ToHashCode();
    }

}

