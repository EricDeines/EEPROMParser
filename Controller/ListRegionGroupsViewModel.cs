
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using EEPROMParser.Model;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace EEPROMParser.Controller;


public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<StringItemViewModel> RegionGroups {get;} = new();

    public ICommand CheckAll {get;}

    public ICommand UncheckAll {get;}

    public ICommand SelectFile {get;}

    public ICommand ReadBinaryFile {get;}

    public string? SelectedFile {get; set;}

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
        CheckAll = new RelayCommand(CheckAllRegionGroups);
        UncheckAll = new RelayCommand(UncheckAllRegionGroups);
        SelectFile = new RelayCommand(SetSelectedFile);
        ReadBinaryFile = new AsyncRelayCommand(ReadEEPROMFile);
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

    private bool ValidateSelection(Variant? selectedVariant, List<string> selectedGroups)
    {
        if (selectedVariant is null)
        {
            return false;
        }
        if (selectedGroups.Count == 0)
        {
            return false;
        }
        bool result = true;
        foreach (var group in selectedGroups)
        {
            result &= selectedVariant.RegionGroups.Any(g => g.Name == group);
        }
        return result;
    }

    private List<string> GetSelectedRegionGroups()
    {
        List<string> result = new();
        foreach (StringItemViewModel group in RegionGroups)
        {
            if (group.IsChecked)
            {
                result.Add(group.Text);
            }
        }
        return result;
    }

    private Variant? GetSelectedVariant()
    {
        foreach (var variant in variants)
        {
            if (variant.Motor == SelectedDrive && variant.Firmware == SelectedFirmware && variant.Comm == SelectedComms)
            {
                return variant;
            }
        }
        return null;
    }

    private void CheckAllRegionGroups()
    {
        foreach (var group in RegionGroups) {
            group.IsChecked = true;
        }
    }

    private void UncheckAllRegionGroups()
    {
        foreach (var group in RegionGroups) {
            group.IsChecked = false;
        }
    }

    private void SetSelectedFile()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            SelectedFile = dialog.FileName;
            ValidateResult = $"Datei {dialog.FileName} geladen!";
        }
    }

    private async Task ReadEEPROMFile()
    {
        if (string.IsNullOrWhiteSpace(SelectedFile))
        {
            ValidateResult = "Keine Datei geladen!";
            return;
        }
        if (!ValidateSelection(GetSelectedVariant(), GetSelectedRegionGroups()))
        {
            ValidateResult = "Keine valide Auswahl!";
            return;
        }

        var BytesPerRegionGroup = EEPROMReader.ReadFile(SelectedFile, GetSelectedVariant(), GetSelectedRegionGroups());

        var FormattedHexStringPerRegionGroup = EEPROMReader.FormatBytes(BytesPerRegionGroup);

        SaveFileDialog dialog = new SaveFileDialog();
        dialog.FileName = "EEPROM";
        dialog.DefaultExt = ".xml";

        if (dialog.ShowDialog() == true)
        {
            await XMLParser.WriteNewXmlFile(dialog.FileName, FormattedHexStringPerRegionGroup);
            ValidateResult = "XML-Datei erfolgreich generiert!";
        }
    }
}