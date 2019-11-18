using System;

namespace TwoPlayerSnake.Game
{
    sealed class Match
    {
        private Snake _friendly;
        private Snake _hostile;
        private Pos _food;

        private Random _rng;

        internal event Action<MatchResult> MatchFinishedEvent;

        internal Match()
        {
            _rng = new Random();
            _friendly = new Snake(Pos.Random(_rng), CellStatus.Friendly);
            _hostile = new Snake(Pos.Random(_rng), CellStatus.Hostile);
            _food = Pos.Random(_rng);
        }
        internal CellStatus[,] GetCells()
        {
            CellStatus[,] cells = new CellStatus[Config.GridSize, Config.GridSize];
            _friendly.Fill(cells);
            _hostile.Fill(cells);
            cells[_food.X, _food.Y] = CellStatus.Food;
            return cells;
        }
        internal void ApplyTurn(Direction friendlyMove, Direction hostileMove)
        {
            // Special case: Head-on collision leads to draw
            if (_friendly.Head().Move(friendlyMove) == _hostile.Head() &&
                _hostile.Head().Move(hostileMove) == _friendly.Head())
            {
                MatchFinishedEvent(MatchResult.Draw);
            }

            // Movement
            _friendly.Move(friendlyMove);
            _hostile.Move(hostileMove);

            // Player deaths
            bool friendlyDead = _friendly.HasHitItself() || _hostile.Contains(_friendly.Head());
            bool hostileDead = _hostile.HasHitItself() || _friendly.Contains(_hostile.Head());

            if (friendlyDead && hostileDead)
            {
                MatchFinishedEvent(MatchResult.Draw);
            }
            else if (hostileDead)
            {
                MatchFinishedEvent(MatchResult.FriendlyWon);
            }
            else if (friendlyDead)
            {
                MatchFinishedEvent(MatchResult.HostileWon);
            }

            // Food
            if (_friendly.Head() == _food)
            {
                _food = Pos.Random(_rng);
                _friendly.Grow();
            }
            if (_hostile.Head() == _food)
            {
                _food = Pos.Random(_rng);
                _hostile.Grow();
            }
        }
    }
}