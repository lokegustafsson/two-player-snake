using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Serilog;
using System;

namespace TwoPlayerSnake.GUI
{
    sealed class GameView : Control
    {
        private const int GridLineThickness = 5;

        private readonly Pen _borderPen;
        private readonly Brush _emptyBrush;
        private readonly Brush _foodBrush;
        private readonly Brush _friendlyBrush;
        private readonly Brush _hostileBrush;

        private CellStatus[,] _cells;

        public GameView()
        {
            _borderPen = new Pen(new SolidColorBrush(Colors.WhiteSmoke), thickness: GridLineThickness);
            _emptyBrush = new SolidColorBrush(Colors.DarkSlateGray);
            _foodBrush = new SolidColorBrush(Colors.IndianRed);
            _friendlyBrush = new SolidColorBrush(Colors.LimeGreen);
            _hostileBrush = new SolidColorBrush(Colors.DarkViolet);

            //  Initialize _cells so that GameView.Render() doesn't throw an exception.
            //  Note that Avalonia calls Render() in a context that silently ignores exceptions,
            //  so it would simply fail silently and mysteriously.
            _cells = new CellStatus[Config.GridSize, Config.GridSize];

            Program.Log(this).Information("GameView initialized");
        }

        internal void SetData(CellStatus[,] cells)
        {
            _cells = cells;
            // Push re-render to the UIThread queue
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }

        private Brush GetBrush(CellStatus status)
        {
            switch (status)
            {
                case CellStatus.Empty: return _emptyBrush;
                case CellStatus.Food: return _foodBrush;
                case CellStatus.Friendly: return _friendlyBrush;
                case CellStatus.Hostile: return _hostileBrush;
            }
            throw new Exception("Received illegal CellStatus");
        }

        public override void Render(DrawingContext context)
        {
            double cellWidth = Bounds.Width / Config.GridSize;
            double cellHeight = Bounds.Height / Config.GridSize;

            for (int x = 0; x < Config.GridSize; x++)
            {
                for (int y = 0; y < Config.GridSize; y++)
                {
                    CellStatus content = _cells[x, Config.GridSize - y - 1];

                    Point topLeft = new Point(x * cellWidth, y * cellHeight);
                    Point bottomLeft = topLeft + new Point(cellWidth, cellHeight);
                    Rect cell = new Rect(topLeft, bottomLeft);

                    context.FillRectangle(GetBrush(content), cell);
                    context.DrawRectangle(_borderPen, cell);
                }
            }
            Program.Log(this).Debug("Rendered GameView");
        }
    }
}