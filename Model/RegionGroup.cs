namespace EEPROMParser.Model;

public class RegionGroup
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

}