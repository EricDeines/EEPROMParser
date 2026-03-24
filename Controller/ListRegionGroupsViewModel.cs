
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using EEPROMParser.Model;
using Microsoft.VisualBasic;

namespace EEPROMParser.Controller;


public class MainViewModel
{
    public ObservableCollection<string> RegionGroups {get;} = new();

    public ObservableCollection<string> Drives {get;} = new();

    public ObservableCollection<string> Firmwares {get;} = new();

    public ObservableCollection<string> Comms {get;} = new();

    public MainViewModel()
    {
        
    }


    public async Task LoadRegionGroupsAsync()
    {
        List<RegionGroup> list = await XMLParser.CreateListRegionGroups();
        foreach (var group in list)
        {
            if (!RegionGroups.Contains(group.Name))
            {
                RegionGroups.Add(group.Name);
            }
        }
        List<Variant> variants = await XMLParser.CreateListVariants(list);

        foreach (var variant in variants)
        {
            if (!Drives.Contains(variant.Motor))
            {
                Drives.Add(variant.Motor);
            }

            if (!Firmwares.Contains(variant.Firmware))
            {
                Firmwares.Add(variant.Firmware);
            }

            if (!Comms.Contains(variant.Comm))
            {
                Comms.Add(variant.Comm);
            }
        }
    }
}