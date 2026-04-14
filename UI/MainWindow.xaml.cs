using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EEPROMParser.Controller;

namespace EEPROMParser.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel viewModel;
    public MainWindow()
    {
        viewModel = new MainViewModel();
        this.DataContext = viewModel;
        this.Loaded += LoadWindow;
        InitializeComponent();
    }

    private async void LoadWindow(object sender, RoutedEventArgs e)
    {
        await viewModel.LoadRegionGroupsAsync();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        viewModel.ValidateResult = viewModel.ValidateSelection().ToString();
    }
    
}