
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

    public ICommand CheckAll {get;}

    public ICommand UncheckAll {get;}

    public ICommand SelectFile {get;}

    public ICommand ReadBinaryFile {get;}

    public ObservableCollection<StringItemViewModel> RegionGroups {get;} = new();

    /// <summary>
    /// A string which stores the selected file path from the UI.
    /// </summary>
    public string? SelectedFile {get; set;}

    public ObservableCollection<string> Drives {get;} = new();

    /// <summary>
    /// A string which stores the selected Drive from the UI.
    /// </summary>
    public string? SelectedDrive {get; set;}

    public ObservableCollection<string> Firmwares {get;} = new();

    /// <summary>
    /// A string which stores the selected Firmware from the UI.
    /// </summary>
    public string? SelectedFirmware {get; set;}

    public ObservableCollection<string> Comms {get;} = new();

    /// <summary>
    /// A string which stores the selected Communication from the UI.
    /// </summary>
    public string? SelectedComms {get; set;}

    private string result = string.Empty;

    /// <summary>
    /// A string which is used to signal succesful or unsuccesful Operations to the User.
    /// </summary>
    public string ValidateResult
    {
        get {return result;}
        set
        {
            result = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// A list of <c>Variant</c> which stores all the variants defined in the XML-Config files.
    /// </summary>
    private List<Variant> variants = new();


    public MainViewModel()
    {
        CheckAll = new RelayCommand(CheckAllRegionGroups);
        UncheckAll = new RelayCommand(UncheckAllRegionGroups);
        SelectFile = new RelayCommand(SetSelectedFile);
        ReadBinaryFile = new AsyncRelayCommand(ReadEEPROMFile);
    }



    /// <summary>
    /// An asynchronous method which loads data from the XML-Config files for display in the UI.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// A static method which validates if a collection of Region groups is used in a given Variant.
    /// </summary>
    /// <param name="selectedVariant">The Variant to use for validation.</param>
    /// <param name="selectedGroups"> A list of strings representing the names of each Region group.</param>
    /// <returns> A <c>bool</c> indicating if Validation was succesful or not.</returns>
    private static bool ValidateSelection(Variant? selectedVariant, List<string> selectedGroups)
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

    /// <summary>
    /// Returns all selected Region groups in the UI.
    /// </summary>
    /// <returns> A <c>List</c> representing the names of each Region group</returns>
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

    /// <summary>
    /// Returs the currently selected <c>Variant</c> in the UI or null if no Variant is selected.
    /// </summary>
    /// <returns></returns>
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

    #region Command Methods

    /// <summary>
    /// Checks every Checkbox for each Region group.
    /// </summary>
    private void CheckAllRegionGroups()
    {
        foreach (var group in RegionGroups) {
            group.IsChecked = true;
        }
    }

    /// <summary>
    /// Unchecks every Checkbox for each Region group
    /// </summary>
    private void UncheckAllRegionGroups()
    {
        foreach (var group in RegionGroups) {
            group.IsChecked = false;
        }
    }

    /// <summary>
    /// Displays a <c>OpenFileDialog</c> for the user to select a file. 
    /// </summary>
    private void SetSelectedFile()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            SelectedFile = dialog.FileName;
            ValidateResult = $"Datei {dialog.FileName} geladen!";
        }
    }

    /// <summary>
    /// An asynchronous method that reads bytes from the currently selected file, formats it and saves it into a XML-File.
    /// </summary>
    /// <returns></returns>
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
    #endregion
    #region PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion
}