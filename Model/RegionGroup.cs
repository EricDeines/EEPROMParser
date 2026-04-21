namespace EEPROMParser.Model;

/// <summary>
/// This class represents a memory area used by a <c>Variant</c> to store related parameters inside the EEPROM.
/// </summary>
public class RegionGroup : IEquatable<RegionGroup>
{
    /// <summary>
    /// The name of the Region group.
    /// </summary>
    public string Name {get; private set;}

    /// <summary>
    /// The size of the Region group in bytes.
    /// </summary>
    public int Size {get; private set;}

    /// <summary>
    /// The id of the Region group. The id is used to uniquely identify a Region group, so
    /// that two different Variants can use the same Region group. 
    /// </summary>
    public int Id {get; private set;}

    /// <summary>
    /// Initializes an instance of a <c>RegionGroup</c> object with the given parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="size"></param>
    /// <param name="id"></param>
    public RegionGroup(string name, int size, int id)
    {
        this.Name = name;
        this.Size = size;
        this.Id = id;
    }

    public bool Equals(RegionGroup? regionGroup)
    {
        if (regionGroup is null)
        {
            return false;
        }
        return string.Equals(Name, regionGroup.Name, StringComparison.OrdinalIgnoreCase) && regionGroup.Size == this.Size && regionGroup.Id == this.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not RegionGroup other)
        {
            return false;
        }
        else
        {
            return Equals(other);
        }
    }

    public static bool operator ==(RegionGroup left, RegionGroup right) =>
        left.Equals(right);

    public static bool operator !=(RegionGroup left, RegionGroup right) =>
        !left.Equals(right);


    public override int GetHashCode()
    {
        return HashCode.Combine(
            Name.ToLowerInvariant(),
            Size,
            Id
        );
    }

    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// A static method which filters a given list of Region groups by a given id.
    /// </summary>
    /// <param name="list">The list of Region groups to filter.</param>
    /// <param name="id">The id to filter by.</param>
    /// <returns>A <c>RegionGroup</c> object with the given id or null if there is no such Region group.</returns>
    public static RegionGroup? GetRegionGroupById(List<RegionGroup> list, int id)
    {
        RegionGroup? returnValue = null;
        foreach(var group in list)
        {
            if (group.Id == id)
            {
                returnValue = group;
            }
        }
        if (returnValue is null)
        {
            return null;
        }
        else
        {
            return returnValue;
        }
    }
}