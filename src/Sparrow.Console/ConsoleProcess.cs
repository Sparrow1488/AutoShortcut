using System.Runtime.InteropServices;

namespace Sparrow.Console;

public static class ConsoleProcess
{
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    private delegate bool EventHandler(int sig);

    public static void OnExit(CancellationTokenSource cancellationTokenSource)
    {
        SetConsoleCtrlHandler(type =>
        {
            System.Console.WriteLine("EXITED");
            cancellationTokenSource.Cancel();
            Environment.Exit(-1);
            return true;
        }, true);
    }

    //internal delegate bool ConsoleEventDelegate(int eventType);

    //[DllImport("kernel32.dll", SetLastError = true)]
    //private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

    //internal static void CancelSourceOnExit(CancellationTokenSource cancellationTokenSource)
    //{
    //    SetConsoleCtrlHandler((eventType) =>
    //    {
    //        cancellationTokenSource.Cancel();
    //        Environment.Exit(-1);
    //        return true;
    //    }, add: true);
    //}
}
