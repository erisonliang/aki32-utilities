using Livet.Commands;

namespace NodeGraph.Utilities;

public class ViewModelCommandHandler
{
    public ViewModelCommand Get(Action execute, Func<bool>? canExecute = null) => command ??= new ViewModelCommand(execute, canExecute);
    ViewModelCommand? command;
}

public class ViewModelCommandHandler<T>
{
    public ListenerCommand<T> Get(Action<T> execute) => command ??= new ListenerCommand<T>(execute);
    ListenerCommand<T>? command;
}
