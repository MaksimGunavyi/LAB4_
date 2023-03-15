public class StartOfWork
{
    public void Execute()
    {
        Console.WriteLine("Начало работы");
    }
}

public class MainPartOfWork
{
    public void Execute()
    {
        Console.WriteLine("Основная часть работы");
    }
}

public class EndOfWork
{
    public void Execute()
    {
        Console.WriteLine("Завершение работы");
    }
}
public class WorkflowEngine
{
    public delegate void WorkflowHandler();

    public event WorkflowHandler OnWorkflowEvent;

    public void Run()
    {
        OnWorkflowEvent?.Invoke();
    }
}
class Program
{
    static void Main(string[] args)
    {
        var startOfWork = new StartOfWork();
        var mainPartOfWork = new MainPartOfWork();
        var endOfWork = new EndOfWork();

        var workflowEngine = new WorkflowEngine();
        workflowEngine.OnWorkflowEvent += startOfWork.Execute;
        workflowEngine.OnWorkflowEvent += mainPartOfWork.Execute;
        workflowEngine.OnWorkflowEvent += endOfWork.Execute;

        workflowEngine.Run();
    }
}
