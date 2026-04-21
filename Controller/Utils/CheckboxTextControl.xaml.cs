using System.Windows;
using System.Windows.Controls;

namespace EEPROMParser.Controller;

/// <summary>
/// A new control which uses a Checkbox and a Textbox.
/// </summary>
public partial class CheckboxTextControl : UserControl
{
    public CheckboxTextControl()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextProperty = 
        DependencyProperty.Register("Text", typeof(string), typeof(CheckboxTextControl));
    
    /// <summary>
    /// A string which represents the Text inside the Textbox.
    /// </summary>
    public string Text
    {
        get {return (string)GetValue(TextProperty);}
        set {SetValue(TextProperty, value);}
    }

    public static readonly DependencyProperty IsCheckedProperty = 
        DependencyProperty.Register("IsChecked", typeof(bool), typeof(CheckboxTextControl));
    
    /// <summary>
    /// A boolean which represents the state of the Checkbox.
    /// </summary>
    public bool IsChecked
    {
        get {return (bool)GetValue(TextProperty);}
        set {SetValue(TextProperty, value);}
    }
}