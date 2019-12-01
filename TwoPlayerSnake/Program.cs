using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Serilog;
using TwoPlayerSnake.ViewModels;
using TwoPlayerSnake.Views;

namespace TwoPlayerSnake
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "{Area}: {Message} {Exception}{NewLine}")
                .CreateLogger();
            SerilogLogger.Initialize(Serilog.Log.Logger);
            AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().Start(AppMain, args);
        }

        private static void AppMain(Application app, string[] args)
        {
            var VMs = new
            {
                GameViewModel = new GameViewModel(),
                List = new PlayerListViewModel(),
                Data = new PlayerDataViewModel()
            };
            AppWindow appWindow = new AppWindow()
            {
                DataContext = VMs
            };
            var gameCoordinator = new GameCoordinator(VMs.GameViewModel);
            appWindow.KeyDown += (o, e) => gameCoordinator.OnKeyDown(e);
            var playerCoordinator = new PlayerListCoordinator(VMs.List, VMs.Data, gameCoordinator.StartGame);

            gameCoordinator.Run(Config.GameStepTime);
            playerCoordinator.Run(Config.CommunicationStepTime);
            app.Run(appWindow);
        }

        /// <summary>
        /// Adds some context to all logging calls in the application
        /// </summary>
        /// <param name="sender">The `this` from where the method is called</param>
        /// <returns>The static, global logger with some sender-based context added</returns>
        internal static ILogger Log(object sender)
        {
            return Serilog.Log.ForContext("Area", sender.GetType().Name);
        }
    }
}
