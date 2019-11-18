namespace TwoPlayerSnake
{
    internal enum Direction { Left, Right, Up, Down }
    internal enum CellStatus { Empty, Food, Friendly, Hostile }
    internal enum MatchResult { Draw, FriendlyWon, HostileWon }
    internal enum InviteStatus { None, SentByUs, SentByRemote }
    internal enum InitRequest { Accept, Reject, Withdraw }
}