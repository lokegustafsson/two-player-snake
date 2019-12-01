using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TwoPlayerSnake.Views
{
    public class PlayerDataView : UserControl
    {
        public PlayerDataView()
        {
            Program.Log(this).Information("Starting initialization");
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("Finished initialization");
        }
    }
}