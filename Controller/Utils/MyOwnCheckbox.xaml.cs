using System.Windows;
using System.Windows.Controls;

namespace EEPROMParser.Controller;

public partial class MyOwnCheckbox : UserControl
{
    public MyOwnCheckbox()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextProperty = 
        DependencyProperty.Register("Text", typeof(string), typeof(MyOwnCheckbox));
    
    public string Text
    {
        get {return (string)GetValue(TextProperty);}
        set {SetValue(TextProperty, value);}
    }

    public static readonly DependencyProperty IsCheckedProperty = 
        DependencyProperty.Register("IsChecked", typeof(bool), typeof(MyOwnCheckbox));
    
    public bool IsChecked
    {
        get {return (bool)GetValue(TextProperty);}
        set {SetValue(TextProperty, value);}
    }
}