using System;
using System.Linq;
using Xunit;

using TwoPlayerSnake.Game;

namespace TwoPlayerSnake.Test.Game
{
    public class PosTests
    {
        private static Pos zero() => new Pos(0, 0);

        [Fact]
        public void Move_Loopback()
        {
            Pos pos = zero();
            Pos posRight = zero();
            Pos posLeft = zero();
            Pos posUp = zero();
            Pos posDown = zero();

            for (int i = 0; i < Config.GridSize; i++)
            {
                posRight = posRight.Move(Direction.Right);
                posLeft = posLeft.Move(Direction.Left);
                posUp = posUp.Move(Direction.Up);
                posDown = posDown.Move(Direction.Down);
            }
            Assert.Equal(pos, posRight);
            Assert.Equal(pos, posLeft);
            Assert.Equal(pos, posUp);
            Assert.Equal(pos, posDown);
        }

        [Fact]
        public void Move_BeAssociative()
        {
            Assert.Equal(zero().Move(Direction.Up).Move(Direction.Right),
                         zero().Move(Direction.Right).Move(Direction.Up));
        }

        [Fact]
        public void Move_ThrowWhenGivenInvalidDirection()
        {
            Assert.Throws<ArgumentException>(() => zero().Move((Direction)12345));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(10000, -1000)]
        public void Construct_SameWayFromIntAndMod(int x, int y)
        {
            Pos pos1 = new Pos(x, y);
            Pos pos2 = new Pos((Pos.Mod)x, (Pos.Mod)y);
            Pos pos3 = new Pos(x % Config.GridSize, y % Config.GridSize);
            Assert.Single(new Pos[] { pos1, pos2, pos3 }.Distinct());
        }

        [Fact]
        public void Random_DoesNotThrow()
        {
            Pos.Random(new Random());
        }

        [Theory]
        [InlineData(3, 4, 5, 3)]
        [InlineData(3, 4, 3, 4)]
        [InlineData(1, -1, -1, 1)]
        public void EqualXorNotEqualIsTrue(int x1, int y1, int x2, int y2)
        {
            Pos pos1 = new Pos(x1, y1);
            Pos pos2 = new Pos(x2, y2);
            Assert.True((pos1 == pos2) ^ (pos1 != pos2));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        public void AdjacentAfterMove(int x, int y)
        {
            Pos pos = new Pos(x, y);
            Assert.True(pos.IsAdjacent(pos.Move(Direction.Up)));
            Assert.True(pos.IsAdjacent(pos.Move(Direction.Down)));
            Assert.True(pos.IsAdjacent(pos.Move(Direction.Left)));
            Assert.True(pos.IsAdjacent(pos.Move(Direction.Right)));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        [InlineData(2, 2)]
        public void AdjacentCommutativeWhenAdjacent(int x, int y)
        {
            Pos pos1 = new Pos(x, y);
            Pos pos2 = new Pos(x + 1, y);
            Assert.True(pos1.IsAdjacent(pos2));
            Assert.True(pos2.IsAdjacent(pos1));
        }
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        [InlineData(2, 2)]
        public void AdjacentCommutativeWhenNotAdjacent(int x, int y)
        {
            Pos pos1 = new Pos(x, y);
            Pos pos2 = new Pos(x + 1, y + 1);
            Assert.False(pos1.IsAdjacent(pos2));
            Assert.False(pos2.IsAdjacent(pos1));
        }
    }
}