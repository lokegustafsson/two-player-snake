using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace TwoPlayerSnake.Views
{
    sealed class AppWindow : Window, IInputElement
    {
        public AppWindow()
        {
            Program.Log(this).Information("Starting initialization");
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("Finished initialization");
        }
    }
}