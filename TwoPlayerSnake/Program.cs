using Avalonia;
using Avalonia.Logging.Serilog;
using Serilog;
using TwoPlayerSnake.GUI;

namespace TwoPlayerSnake
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Area}: {Message} {Exception}{NewLine}")
                .CreateLogger();
            AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().Start(AppMain, args);
        }

        private static void AppMain(Application app, string[] args)
        {
            AppWindow appWindow = new AppWindow();
            var gameCoordinator = new GameCoordinator(appWindow);
            var playerCoordinator = new PlayerListCoordinator(appWindow, gameCoordinator.StartGame);

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
