using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TwoPlayerSnake.Views
{
    public class PlayerListView : UserControl
    {
        public PlayerListView()
        {
            Program.Log(this).Information("Starting initialization");
            AvaloniaXamlLoader.Load(this);
            Program.Log(this).Information("Finished initialization");
        }
    }
}