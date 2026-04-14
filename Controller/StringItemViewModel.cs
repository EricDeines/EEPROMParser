using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEPROMParser.Controller;

public class StringItemViewModel : INotifyPropertyChanged
{
    private string? _text;

    private bool _isChecked;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

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


}