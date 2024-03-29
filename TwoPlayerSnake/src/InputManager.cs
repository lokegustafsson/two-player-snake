using Avalonia.Input;

namespace TwoPlayerSnake
{
    sealed class InputManager
    {
        internal Direction Direction { get; private set; }

        internal InputManager()
        {
        }
        internal void OnKeyDown(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Left: Direction = Direction.Left; break;
                case Key.Right: Direction = Direction.Right; break;
                case Key.Up: Direction = Direction.Up; break;
                case Key.Down: Direction = Direction.Down; break;
            }
        }
    }
}