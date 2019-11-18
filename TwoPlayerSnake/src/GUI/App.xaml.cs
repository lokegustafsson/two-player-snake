using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Logging.Serilog;
using Serilog;

namespace TwoPlayerSnake.GUI
{
    sealed class App : Application
    {
        public override void Initialize()
        {
            SerilogLogger.Initialize(Log.Logger);
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("App initialized");
        }
    }
}