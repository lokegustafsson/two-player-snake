using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwoPlayerSnake.GUI
{
    sealed class AppWindow : Window, IInputElement
    {
        internal AppWindow()
        {
            AvaloniaXamlLoader.Load(this);
            Log.ForContext("Area", "GUI").Information("AppWindow initialized");
        }

        internal GameView GetGameView()
        {
            return (GameView)LogicalChildren.Where(x => x.GetType() == typeof(GameView)).Single();
        }
    }
}