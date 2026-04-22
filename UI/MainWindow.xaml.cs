using System.Windows;
using EEPROMParser.Controller;

namespace EEPROMParser.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel viewModel;
    public MainWindow()
    {
        viewModel = new MainViewModel();
        this.DataContext = viewModel;
        this.Loaded += LoadWindow;
        InitializeComponent();
    }

    /// <summary>
    /// An asynchronous method which loads data from the XML-Config files into the application. This method
    /// is used as a event handler for the <c>Loaded</c> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void LoadWindow(object sender, RoutedEventArgs e)
    {
        await viewModel.LoadRegionGroupsAsync();
    }
}