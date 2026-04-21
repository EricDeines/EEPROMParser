using System.Windows.Input;

namespace EEPROMParser.Controller;

/// <summary>
/// A generic Implementation of <c>ICommand</c> which can execute any object of type <c>Action</c>
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
        => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter)
        => _execute();

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
