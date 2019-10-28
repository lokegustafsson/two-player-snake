using System;
using Xunit;

using TwoPlayerSnake.Game;

namespace TwoPlayerSnake.Test.Game
{
    public class SnakeTests
    {
        static Snake getSnake() => new Snake(new Pos(0, 0), CellStatus.Friendly);

        [Fact]
        public void MoveMovesSameAsPos()
        {
            Snake snake = getSnake();
            snake.Move(Direction.Right);
            Assert.Equal(snake.Head(), new Pos(0, 0).Move(Direction.Right));
        }
        [Fact]
        public void DoesNotHitItselfMovingInCircle()
        {
            Snake snake = getSnake();
            for (int i = 0; i < 3; i++) { snake.Grow(); }
            // length 4
            snake.Move(Direction.Right);
            snake.Move(Direction.Up);
            snake.Move(Direction.Left);
            snake.Move(Direction.Down);
            Assert.False(snake.HasHitItself());
        }
        [Fact]
        public void DoesHitItselfMovingInCircle()
        {
            Snake snake = getSnake();
            for (int i = 0; i < 4; i++) { snake.Grow(); }
            // length 5
            snake.Move(Direction.Right);
            snake.Move(Direction.Up);
            snake.Move(Direction.Left);
            snake.Move(Direction.Down);
            Assert.True(snake.HasHitItself());
        }
        [Fact]
        public void ContainsOwnHead()
        {
            Snake snake = getSnake();
            Assert.True(snake.Contains(snake.Head()));
        }
        [Fact]
        public void FillsInCircle()
        {
            Snake snake = getSnake();
            for (int i = 0; i < 3; i++) { snake.Grow(); }
            // Now length 4
            snake.Move(Direction.Right);
            snake.Move(Direction.Up);
            snake.Move(Direction.Left);
            
            CellStatus[,] grid = new CellStatus[2,2];
            snake.Fill(grid);
            foreach (CellStatus cell in grid)
            {
                Assert.Equal(CellStatus.Friendly, cell);
            }
        }
    }
}