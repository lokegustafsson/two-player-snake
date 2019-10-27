namespace TwoPlayerSnake.Game
{
    internal enum Direction { Left, Right, Up, Down }
    internal enum CellStatus { Empty, Food, Friendly, Hostile }
    internal enum MatchResult { Draw, friendlyWon, hostileWon }
}