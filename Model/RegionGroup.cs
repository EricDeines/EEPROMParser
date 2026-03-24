namespace EEPROMParser.Model;

public class RegionGroup : IEquatable<RegionGroup>
{
    public string Name {get; private set;}

    public int Size {get; private set;}

    public int Id {get; private set;}

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

    public static RegionGroup? GetRegionGroupById(List<RegionGroup> list, int id)
    {
        RegionGroup returnValue = null;
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