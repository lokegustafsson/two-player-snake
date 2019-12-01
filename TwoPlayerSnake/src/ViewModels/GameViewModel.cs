using System.Linq;
using Avalonia.Reactive;

namespace TwoPlayerSnake.ViewModels
{
    sealed class GameViewModel : ViewModel
    {
        private CellStatus[,] _cells;

        public CellStatus[,] Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                Notify();
            }
        }

        public GameViewModel()
        {
            Cells = new CellStatus[Config.GridSize, Config.GridSize];
            Cells[0, 0] = CellStatus.Food;
        }
    }
}   