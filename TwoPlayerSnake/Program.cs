using Avalonia;
using Avalonia.Logging.Serilog;
using Serilog;
using System;
using System.Timers;
using TwoPlayerSnake.GUI;

namespace TwoPlayerSnake
{
    static class Program
    {
        private static void Main(string[] args)
        {
            AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().Start(AppMain, args);
        }
        private static void AppMain(Application app, string[] args)
        {
            AppWindow appWindow = new AppWindow();

            Coordinator coordinator = new Coordinator(appWindow);

            Timer timer = new Timer(Config.UpdateTimeMilliseconds);
            timer.Elapsed += (sender, elapsedArgs) =>
            {
                // System.Timers.Timer silently swallows exceptions,
                // so we need to explicitly log any errors and exit manually
                try { coordinator.Update(); }
                catch (Exception e)
                {
                    Log.ForContext("Area", "Program").Fatal(e, "Something unexpected happened:");
                    timer.Stop();
                    System.Environment.Exit(1);
                }
            };
            timer.Start();

            app.Run(appWindow);
        }
    }
}
