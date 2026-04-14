
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using EEPROMParser.Model;
using Microsoft.VisualBasic;
using System.Windows;

namespace EEPROMParser.Controller;


public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<StringItemViewModel> RegionGroups {get;} = new();

    public string? SelectedRegionGroup {get; set;}

    public ObservableCollection<string> Drives {get;} = new();

    public string? SelectedDrive {get; set;}

    public ObservableCollection<string> Firmwares {get;} = new();

    public string? SelectedFirmware {get; set;}

    public ObservableCollection<string> Comms {get;} = new();

    public string? SelectedComms {get; set;}

    private string result = "";
    public string ValidateResult
    {
        get {return result;}
        set
        {
            result = value;
            OnPropertyChanged();
        }
    }

    private List<Variant> variants = new();

    public MainViewModel()
    {
        
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    public async Task LoadRegionGroupsAsync()
    {
        List<RegionGroup> list = await XMLParser.CreateListRegionGroups();
        foreach (var group in list)
        {
            if (!RegionGroups.Any(item => item.Text == group.Name))
            {
                var item = new StringItemViewModel
                {
                    Text = group.Name,
                    IsChecked = false
                };
                RegionGroups.Add(item);
            }
        }
        variants = await XMLParser.CreateListVariants(list);

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

    public bool ValidateSelection()
    {
        foreach (var variant in variants)
        {
            if (variant.Motor == SelectedDrive && variant.Firmware == SelectedFirmware && variant.Comm == SelectedComms)
            {
                foreach (var group in variant.RegionGroups)
                {
                    if (group.Name == SelectedRegionGroup)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}