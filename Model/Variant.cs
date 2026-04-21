namespace EEPROMParser.Model;

/// <summary>
/// This class represents a variant.
/// </summary>
public class Variant : IEquatable<Variant>
{
    /// <summary>
    /// The name of the motor as a string.
    /// </summary>
    public string Motor {get; private set;}

    /// <summary>
    /// The name of the firmware as a string.
    /// </summary>
    public string Firmware {get; private set;}

    /// <summary>
    /// The communication used as a string.
    /// </summary>
    public string Comm {get; private set;}

    /// <summary>
    /// A list of <c>RegionGroup</c> objects which represents the memory layout that this variant uses.
    /// </summary>
    public List<RegionGroup> RegionGroups {get; private set;} = new();

    /// <summary>
    /// Initializes a <c>Variant</c> object with the given parameters.
    /// </summary>
    /// <param name="motor"></param>
    /// <param name="firmware"></param>
    /// <param name="comm"></param>
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
        !left.Equals(right);

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

