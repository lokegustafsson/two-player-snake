using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Logging;
using Avalonia.Logging.Serilog;
using Serilog;
using System;
using System.Linq;

namespace TwoPlayerSnake.GUI
{
    sealed class App : Application
    {
        public override void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "{Area}: {Message} {Exception}{NewLine}")
                .CreateLogger();
            SerilogLogger.Initialize(Log.Logger);

            AvaloniaXamlLoader.Load(this);
            Log.ForContext("Area", "GUI").Information("App initialized");
        }
    }
}