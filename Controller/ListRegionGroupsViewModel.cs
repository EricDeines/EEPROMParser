
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EEPROMParser.Model;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace EEPROMParser.Controller;


public class MainViewModel : INotifyPropertyChanged
{

    public ICommand CheckAll {get;}

    public ICommand UncheckAll {get;}

    public ICommand ReadBinaryFile {get;}

    public ObservableCollection<StringItemViewModel> RegionGroups {get;} = new();

    public ObservableCollection<string> Drives {get;} = new();

    private string? _selectedDrive;

    /// <summary>
    /// A string which stores the selected Drive from the UI.
    /// </summary>
    public string? SelectedDrive
    {
        get => _selectedDrive;
        set
        {
            if (_selectedDrive != value)
            {
                _selectedDrive = value;
                SelectedVariant = GetSelectedVariant();    
            }
        }
    }

    public ObservableCollection<string> Firmwares {get;} = new();

    private string? _selectedFirmware;

    /// <summary>
    /// A string which stores the selected Firmware from the UI.
    /// </summary>
    public string? SelectedFirmware
    {
        get => _selectedFirmware;
        set
        {
            if (_selectedFirmware != value)
            {
                _selectedFirmware = value;
                SelectedVariant = GetSelectedVariant();    
            }
        }
    }

    public ObservableCollection<string> Comms {get;} = new();

    private string? _selectedComms;
    /// <summary>
    /// A string which stores the selected Communication from the UI.
    /// </summary>
    public string? SelectedComms
    {
        get => _selectedComms;
        set
        {
            if (_selectedComms != value)
            {
                _selectedComms = value;
                SelectedVariant = GetSelectedVariant();    
            }
        }
    }

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

    private Variant? _selectedVariant;

    /// <summary>
    /// The currently selected Variant in the UI.
    /// </summary>
    public Variant? SelectedVariant {
        get {return _selectedVariant;}
        set
        {
            _selectedVariant = value;
            RegionGroups.Clear();
            if (_selectedVariant is null)
            {
                return;
            }
            else
            {
                foreach (var group in _selectedVariant.RegionGroups)
                {
                    RegionGroups.Add(new StringItemViewModel
                    {
                        Text = group.Name,
                        IsChecked = false
                    });
                }
            }
        }
    }

    public string FilePathListRegionGroup {get; private set;} = Path.Combine(AppContext.BaseDirectory, "Resources", "ListRegionGroups.xml");

    public string FilePathListVariant {get; private set;} = Path.Combine(AppContext.BaseDirectory, "Resources", "ListEEPROMVariants.xml");


    public MainViewModel()
    {
        CheckAll = new RelayCommand(CheckAllRegionGroups);
        UncheckAll = new RelayCommand(UncheckAllRegionGroups);
        ReadBinaryFile = new AsyncRelayCommand(ReadEEPROMFile);
    }



    /// <summary>
    /// An asynchronous method which loads data from the XML-Config files for display in the UI.
    /// </summary>
    /// <returns></returns>
    public async Task LoadRegionGroupsAsync()
    {
        try
        {
            List<RegionGroup> list = await XMLParser.CreateListRegionGroups(FilePathListRegionGroup);
            variants = await XMLParser.CreateListVariants(FilePathListVariant, list);

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
        } catch (FileNotFoundException)
        {
            MessageBox.Show("Config Dateien konnten nicht gefunden werden", "Loading error", MessageBoxButton.OK, MessageBoxImage.Error);
        } catch (InvalidOperationException iE)
        {
            MessageBox.Show(iE.Message, "Loading error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        foreach (var group in RegionGroups)
        {
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
    /// An asynchronous method that asks the user to open a file, read content from it and write it into a new xml file.
    /// </summary>
    /// <returns></returns>
    private async Task ReadEEPROMFile()
    {
        if (!ValidateSelection(GetSelectedVariant(), GetSelectedRegionGroups()))
        {
            ValidateResult = "Keine valide Auswahl!";
            return;
        }

        OpenFileDialog openDialog = new OpenFileDialog();
        openDialog.Title = "Öffne Binär Datei";
        if (openDialog.ShowDialog() == true)
        {
            string fileName = openDialog.FileName;
            var BytesPerRegionGroup = EEPROMReader.ReadFile(fileName, GetSelectedVariant(), GetSelectedRegionGroups());

            var FormattedHexStringPerRegionGroup = EEPROMReader.FormatBytes(BytesPerRegionGroup);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = "EEPROM";
            saveDialog.DefaultExt = ".xml";

            if (saveDialog.ShowDialog() == true)
            {
                await XMLParser.WriteNewXmlFile(saveDialog.FileName, FormattedHexStringPerRegionGroup);
                ValidateResult = "XML-Datei erfolgreich generiert!";
            }
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