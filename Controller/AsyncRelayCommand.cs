using System.Windows.Input;


namespace EEPROMParser.Controller;

/// <summary>
/// A generic Implementation of <c>ICommand</c> which can execute any object of type <c>Task</c>
/// </summary>
public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public async void Execute(object? parameter)
    {
        await _execute();
    }

    public bool CanExecute(object? parameter) => true;

    public event EventHandler? CanExecuteChanged;
}