using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwoPlayerSnake.Game
{
    sealed class Snake
    {
        private int _length;
        private LinkedList<Pos> _body;
        private CellStatus _faction;

        internal Snake(Pos startPos, CellStatus faction)
        {
            _length = 1;
            _body = new LinkedList<Pos>();
            _body.AddFirst(startPos);
            _faction = faction;
        }

        internal void Move(Direction direction)
        {
            _body.AddFirst(Head().Move(direction));
            while (_body.Count > _length)
            {
                _body.RemoveLast();
            }
        }

        internal void Fill(CellStatus[,] cells)
        {
            _body.AsParallel().ForAll(pos => cells[pos.X, pos.Y] = _faction);
        }

        internal void Grow()
        {
            _length++;
            Log.ForContext("Area", "Game").Information("Snake {faction} grew to {length}", _faction, _length);
        }

        internal Pos Head() => _body.First.Value;
        internal bool Contains(Pos pos) => _body.Any(x => x == pos);
        internal bool HasHitItself() => _body.Skip(1).Any(x => x == Head());
    }
}