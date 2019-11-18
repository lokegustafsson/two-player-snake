using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace TwoPlayerSnake.GUI
{
    sealed class AppWindow : Window, IInputElement
    {
        internal AppWindow()
        {
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("AppWindow initialized");
        }

        internal T Locate<T>() where T : class, IControl
        {
            return this.FindControl<T>(typeof(T).Name);
        }
    }
}