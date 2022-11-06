using System.Runtime.InteropServices;

namespace Sparrow.Console;

public class ConsoleProcess
{
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    private delegate bool EventHandler(int sig);

    public void OnExit(CancellationTokenSource cancellationTokenSource)
    {
        var handler = new EventHandler(Handler);
        SetConsoleCtrlHandler(handler, true);
    }

    public bool Handler(int type)
    {
        System.Console.WriteLine("EXITED");
        Environment.Exit(-1);
        return true;
    }
}
