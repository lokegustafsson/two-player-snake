using Avalonia;
using Avalonia.Markup.Xaml;
using Serilog;

namespace TwoPlayerSnake
{
    sealed class App : Application
    {
        public override void Initialize()
        {
            Program.Log(this).Information("Starting initialization");
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("Finished initialization");
        }
    }
}