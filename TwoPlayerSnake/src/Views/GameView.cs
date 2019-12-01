using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;

namespace TwoPlayerSnake.Views
{
    sealed class GameView : Control
    {
        #region AvaloniaProperties
        private CellStatus[,] _cells;

        [Content]
        public CellStatus[,] Cells
        {
            get { return _cells; }
            set
            {
                if (value == null)
                {
                    Program.Log(this).Warning("Setting Cells to null!");
                    return;
                    //throw new ArgumentException("Cannot set Cells to null");
                }
                if (value.GetLength(0) != Config.GridSize || value.GetLength(1) != Config.GridSize)
                {
                    throw new ArgumentException("Invalid array dimensions");
                }
                _cells = value;
                // Push re-render to the UIThread queue
                Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
            }
        }
        public Pen GridPen
        {
            get { return GetValue(GridPenProperty); }
            set { SetValue(GridPenProperty, value); }
        }
        public IBrush EmptyBrush
        {
            get { return GetValue(EmptyBrushProperty); }
            set { SetValue(EmptyBrushProperty, value); }
        }
        public IBrush FoodBrush
        {
            get { return GetValue(FoodBrushProperty); }
            set { SetValue(FoodBrushProperty, value); }
        }
        public IBrush FriendlyBrush
        {
            get { return GetValue(FriendlyBrushProperty); }
            set { SetValue(FriendlyBrushProperty, value); }
        }
        public IBrush HostileBrush
        {
            get { return GetValue(HostileBrushProperty); }
            set { SetValue(HostileBrushProperty, value); }
        }

        public static readonly StyledProperty<Pen> GridPenProperty =
            AvaloniaProperty.Register<GameView, Pen>(nameof(GridPen));

        public static readonly StyledProperty<IBrush> EmptyBrushProperty =
            AvaloniaProperty.Register<GameView, IBrush>(nameof(EmptyBrush));

        public static readonly StyledProperty<IBrush> FoodBrushProperty =
            AvaloniaProperty.Register<GameView, IBrush>(nameof(FoodBrush));

        public static readonly StyledProperty<IBrush> FriendlyBrushProperty =
            AvaloniaProperty.Register<GameView, IBrush>(nameof(FriendlyBrush));

        public static readonly StyledProperty<IBrush> HostileBrushProperty =
            AvaloniaProperty.Register<GameView, IBrush>(nameof(HostileBrush));

        public static readonly DirectProperty<GameView, CellStatus[,]> CellsProperty =
            AvaloniaProperty.RegisterDirect<GameView, CellStatus[,]>(
                nameof(Cells),
                o => o.Cells,
                (o, v) => o.Cells = v);

        #endregion

        public override void Render(DrawingContext context)
        {
            IBrush GetBrush(CellStatus status)
            {
                switch (status)
                {
                    case CellStatus.Empty: return EmptyBrush;
                    case CellStatus.Food: return FoodBrush;
                    case CellStatus.Friendly: return FriendlyBrush;
                    case CellStatus.Hostile: return HostileBrush;
                }
                throw new Exception("Received illegal CellStatus");
            }

            double cellWidth = Bounds.Width / Config.GridSize;
            double cellHeight = Bounds.Height / Config.GridSize;

            for (int x = 0; x < Config.GridSize; x++)
            {
                for (int y = 0; y < Config.GridSize; y++)
                {
                    CellStatus content = Cells[x, Config.GridSize - y - 1];

                    Point topLeft = new Point(x * cellWidth, y * cellHeight);
                    Point bottomLeft = topLeft + new Point(cellWidth, cellHeight);
                    Rect cell = new Rect(topLeft, bottomLeft);

                    context.FillRectangle(GetBrush(content), cell);
                    context.DrawRectangle(GridPen, cell);
                }
            }
            Program.Log(this).Debug("Rendered GameView");
        }
    }
}