using System.Runtime.InteropServices;

namespace Sparrow.Console;

public class ConsoleProcess
{
    public static bool ConsoleEventCallback(int eventType)
    {
        if (eventType == 2)
        {
            System.Console.WriteLine("Console window closing, death imminent");
        }
        return false;
    }

    public delegate bool ConsoleEventDelegate(int eventType);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

    public void OnExit(CancellationTokenSource cancellationTokenSource)
    {
        var handler = new ConsoleEventDelegate(Handler);
        SetConsoleCtrlHandler(handler, true);
    }

    public bool Handler(int type)
    {
        System.Console.WriteLine("EXITED");
        Environment.Exit(-1);
        return true;
    }
}
