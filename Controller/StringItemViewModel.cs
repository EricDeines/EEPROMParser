using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEPROMParser.Controller;

/// <summary>
/// A Viewmodel to be used for the <c>CheckboxTextControl</c> class.
/// </summary>
public class StringItemViewModel : INotifyPropertyChanged
{
    private string _text = string.Empty;

    private bool _isChecked;


    public string Text
    {
        get {return _text;}
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public bool IsChecked
    {
        get {return _isChecked;}
        set
        {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    #region PropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion

}